using eShopSolution.Application.Common;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.ViewModels.Catalog.Contacts;
using eShopSolution.ViewModels.Catalog.Products;
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

namespace eShopSolution.Application.Catalog.Contacts
{
    public class ContactService: IContactService
    {
        private readonly EShopDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IStorageService _storageService;
        private readonly IConfiguration _configuration;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";

        // dependency injection, truyền context vào để thao tác CRUD
        public ContactService(EShopDbContext context, IStorageService storageService, IConfiguration configuration,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _storageService = storageService;
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<int> Create(ContactCreateRequest request)
        {
            var contact = new Contact()
            {
                // id tự tăng
                Name = request.Name,
                Tille = request.Tille,
                Gmail= request.Gmail,
                SDT = request.SDT,
                Status = true,
            };

            //Save thumbnail image
            if (request.Image != null)
            {
                contact.Image = await SaveFile(request.Image);
            }
            else
            {
                contact.Image = "/user-content/no-image.png";
            }


            _context.Contacts.Add(contact);

            //trả về số lượng bản ghi maybe
            await _context.SaveChangesAsync();
            return contact.Id;
        }

        public async Task<int> Update(ContactUpdateRequest request)
        {
            var slide = await _context.Contacts.FindAsync(request.Id);
            if (slide == null) throw new EShopException($"Không thể tìm Contact có ID: {request.Id} ");

            slide.Name = request.Name;
            slide.Tille = request.Tille;
            slide.Gmail = request.Gmail;
            slide.SDT = request.SDT;
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

        public async Task<int> Delete(int contactId)
        {
            var contact = await _context.Contacts.FindAsync(contactId);
            if (contact == null) throw new EShopException($"Không thể tìm Slide có ID: {contactId}");

            var images = _context.Contacts.Where(i => i.Id == contactId);
            foreach (var image in images)
            {
                await _storageService.DeleteFileAsync(image.Image);
                ;
            }

            _context.Contacts.Remove(contact);

            return await _context.SaveChangesAsync();
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }

        public async Task<ContactViewModel> GetById(int id)
        {
            var query = from c in _context.Contacts
                        where c.Id == id
                        select new { c };

            return await query.Select(x => new ContactViewModel()
            {
                Id = x.c.Id,
                Name = x.c.Name,
                Gmail = x.c.Gmail,
                SDT = x.c.SDT,
                Image = x.c.Image,
                Tille = x.c.Tille,
                Status = true,
            }).FirstOrDefaultAsync();
        }
        public async Task<List<ContactViewModel>> GetAll()
        {
            var query = from c in _context.Contacts
                        select new { c };

            return await query.Select(x => new ContactViewModel()
            {
                Id = x.c.Id,
                Name = x.c.Name,
                Gmail = x.c.Gmail,
                SDT = x.c.SDT,
                Image = x.c.Image,
                Tille = x.c.Tille,
                Status = true,
            }).ToListAsync();
        }

        public async Task<PagedResult<ContactViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            var query = from c in _context.Contacts
                        select new { c };

            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.c.Name.Contains(request.Keyword));

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ContactViewModel()
                {
                    Id = x.c.Id,
                    Name = x.c.Name,
                    Gmail = x.c.Gmail,
                    SDT = x.c.SDT,
                    Image = x.c.Image,
                    Tille = x.c.Tille,
                    Status = true,
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<ContactViewModel>()
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

