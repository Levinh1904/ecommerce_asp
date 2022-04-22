
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.ViewModels.Catalog.Producers;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Http;
using eShopSolution.Application.Common;

namespace eShopSolution.Application.Catalog.Produ
{
    public class ProducerService:IProducerService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";

        public ProducerService(EShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<int> Create(ProducerCreateRequest request)
        {
            var producer = new Producer()
            {
                // id tự tăng
                Name = request.Name,

            };

            //Save thumbnail image
            if (request.Image != null)
            {
                producer.Image = await this.SaveFile(request.Image);
            }
            else
            {
                producer.Image = "/user-content/no-image.png";
            }

   
            _context.Producers.Add(producer);

            //trả về số lượng bản ghi maybe
            await _context.SaveChangesAsync();
            return producer.Id;
        }

        public async Task<int> Update(ProducerUpdateRequest request)
        {
            var producer = await _context.Producers.FindAsync(request.Id);
            if (producer == null) throw new EShopException($"Không thể tìm thương hiệu có ID: {request.Id} ");

            producer.Name = request.Name;
            //Save thumbnail image
            if (request.Image != null)
            {
                producer.Image = await this.SaveFile(request.Image);
            }
            else
            {
                producer.Image = "/user-content/no-image.png";
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int producerId)
        {
            var producer = await _context.Producers.FindAsync(producerId);
            if (producer == null) throw new EShopException($"Không thể tìm thương hiệu có ID: {producerId} ");

            var images = _context.Producers.Where(i => i.Id == producerId);
            foreach (var image in images)
            {
                await _storageService.DeleteFileAsync(image.Image);
            }

            _context.Producers.Remove(producer);

            return await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<ProducerViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            var query = from c in _context.Producers
                        select new { c };

            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.c.Name.Contains(request.Keyword));

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProducerViewModel()
                {
                    Id = x.c.Id,
                    Name = x.c.Name,
                    Image = x.c.Image,
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<ProducerViewModel>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<List<ProducerViewModel>> GetAll()
        {
            var query = from c in _context.Producers
                        select new { c };

            return await query.Select(x => new ProducerViewModel()
            {
                Id = x.c.Id,
                Name = x.c.Name,
                Image = x.c.Image,
            }).ToListAsync();
        }

        public async Task<ProducerViewModel> GetById(int id)
        {
            var query = from c in _context.Producers
                        where c.Id == id
                        select new { c };

            return await query.Select(x => new ProducerViewModel()
            {
                Id = x.c.Id,
                Name = x.c.Name,
                Image = x.c.Image,
            }).FirstOrDefaultAsync();
        }
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }
    }
}
