using eShopSolution.Application.Common;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Catalog.Slides;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Slides
{
    public class SlideService: ISlideService
    {
        private readonly EShopDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IStorageService _storageService;
        private readonly IConfiguration _configuration;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";

        // dependency injection, truyền context vào để thao tác CRUD
        public SlideService(EShopDbContext context, IStorageService storageService, IConfiguration configuration,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _storageService = storageService;
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<int> Create(SlideCreateRequest request)
        {
            var slide = new Slide()
            {
                // id tự tăng
                Name = request.Name,
                Tille = request.Tille,
                Color = request.Color,
                Status = request.Status
            };

            //Save thumbnail image
            if (request.Image != null)
            {
                slide.Image = await this.SaveFile(request.Image);
            }
            else
            {
                slide.Image = "/user-content/no-image.png";
            }

            
            _context.Slides.Add(slide);

            //trả về số lượng bản ghi maybe
            await _context.SaveChangesAsync();
            return slide.Id;
        }

        public async Task<int> Update(SlideUpdateRequest request)
        {
            var slide = await _context.Slides.FindAsync(request.Id);
            if (slide == null) throw new EShopException($"Không thể tìm sản phẩm có ID: {request.Id} ");

            slide.Name = request.Name;
            slide.Tille = request.Tille;
            slide.Color = request.Color;
            slide.Status = request.Status;

            //Save thumbnail image
            if (request.Image != null)
            {
                slide.Image = await this.SaveFile(request.Image);
            }
            else
            {
                slide.Image = "/user-content/no-image.png";
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int slideId)
        {
            var slide = await _context.Slides.FindAsync(slideId);
            if (slide == null) throw new EShopException($"Không thể tìm Slide có ID: {slideId}");

            var images = _context.Slides.Where(i => i.Id == slideId);
            foreach (var image in images)
            {
                await _storageService.DeleteFileAsync(image.Image);
;
            }

            _context.Slides.Remove(slide);

            return await _context.SaveChangesAsync();
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }

        public async Task<SlideViewModel> GetById(int id)
        {
            var query = from c in _context.Slides
                        where c.Id == id
                        select new { c };

            return await query.Select(x => new SlideViewModel()
            {
                Id = x.c.Id,
                Name = x.c.Name,
                Image = x.c.Image,
                Tille = x.c.Tille,
                Color = x.c.Color,
                Status =x.c.Status,
            }).FirstOrDefaultAsync();
        }
        public async Task<List<SlideViewModel>> GetAll()
        {
            var query = from c in _context.Slides
                        select new { c };

            return await query.Select(x => new SlideViewModel()
            {
                Id = x.c.Id,
                Name = x.c.Name,
                Image = x.c.Image,
                Tille = x.c.Tille,
                Color = x.c.Color,
                Status = x.c.Status,
            }).ToListAsync();
        }

        public async Task<PagedResult<SlideViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            var query = from c in _context.Slides
                        select new { c };

            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.c.Name.Contains(request.Keyword));

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new SlideViewModel()
                {
                    Id = x.c.Id,
                    Name = x.c.Name,
                    Image = x.c.Image,
                    Color = x.c.Color, 
                    Tille = x.c.Tille,
                    Status =x.c.Status,
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<SlideViewModel>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

    }
}

