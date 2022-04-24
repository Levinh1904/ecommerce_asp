using eShopSolution.Application.Common;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.ViewModels.Catalog.Blogs;
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

namespace eShopSolution.Application.Catalog.Blogs
{
    public class BlogService: IBlogService
    {
        private readonly EShopDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IStorageService _storageService;
        private readonly IConfiguration _configuration;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";

        // dependency injection, truyền context vào để thao tác CRUD
        public BlogService(EShopDbContext context, IStorageService storageService, IConfiguration configuration,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _storageService = storageService;
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<int> Create(BlogCreateRequest request)
        {
            var blog = new Blog()
            {
                // id tự tăng
                Name = request.Name,
                Tille = request.Tille,
                Status =true,
            };

            //Save thumbnail image
            if (request.Image != null)
            {
                blog.Image = await SaveFile(request.Image);
            }
            else
            {
                blog.Image = "/user-content/no-image.png";
            }


            _context.Blogs.Add(blog);

            //trả về số lượng bản ghi maybe
            await _context.SaveChangesAsync();
            return blog.Id;
        }

        public async Task<int> Update(BlogUpdateRequest request)
        {
            var slide = await _context.Blogs.FindAsync(request.Id);
            if (slide == null) throw new EShopException($"Không thể tìm Blog có ID: {request.Id} ");

            slide.Name = request.Name;
            slide.Tille = request.Tille;
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

        public async Task<int> Delete(int blogId)
        {
            var blog = await _context.Blogs.FindAsync(blogId);
            if (blog == null) throw new EShopException($"Không thể tìm Slide có ID: {blogId}");

            var images = _context.Blogs.Where(i => i.Id == blogId);
            foreach (var image in images)
            {
                await _storageService.DeleteFileAsync(image.Image);
                ;
            }

            _context.Blogs.Remove(blog);

            return await _context.SaveChangesAsync();
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }

        public async Task<BlogViewModel> GetById(int id)
        {
            var query = from c in _context.Blogs
                        where c.Id == id
                        select new { c };

            return await query.Select(x => new BlogViewModel()
            {
                Id = x.c.Id,
                Name = x.c.Name,
                Image = x.c.Image,
                Tille = x.c.Tille,
                Status = "true",
            }).FirstOrDefaultAsync();
        }
        public async Task<List<BlogViewModel>> GetAll()
        {
            var query = from c in _context.Blogs
                        select new { c };

            return await query.Select(x => new BlogViewModel()
            {
                Id = x.c.Id,
                Name = x.c.Name,
                Image = x.c.Image,
                Tille = x.c.Tille,
                Status = "true",
            }).ToListAsync();
        }

        public async Task<PagedResult<BlogViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            var query = from c in _context.Blogs
                        select new { c };

            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.c.Name.Contains(request.Keyword));

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new BlogViewModel()
                {
                    Id = x.c.Id,
                    Name = x.c.Name,
                    Image = x.c.Image,
                    Tille = x.c.Tille,
                    Status = "true",
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<BlogViewModel>()
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
 
