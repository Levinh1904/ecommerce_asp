using eShopSolution.ApiIntegration;
using eShopSolution.ViewModels.Catalog.Blogs;
using eShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Controllers
{
    public class BlogController : BaseController
    {
        private readonly IBlogApiClient _blogApiClient;

        public BlogController(IBlogApiClient blogApiClient)
        {
            _blogApiClient = blogApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 6)
        {
            var request = new GetManageProductPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };

            var data = await _blogApiClient.GetAllPaging(request);
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] BlogCreateRequest request)
        {

            var result = await _blogApiClient.CreateBlog(request);
            if (result)
            {
                TempData["CreateBlogSuccessful"] = "Thêm mới Blog thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Thêm Blog thất bại");
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var blog = await _blogApiClient.GetById(id);
            var editVm = new BlogUpdateRequest()
            {
                Id = blog.Id,
                Name = blog.Name,
                Tille = blog.Tille,
            };

            return View(editVm);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit([FromForm] BlogUpdateRequest request)
        {


            var result = await _blogApiClient.UpdateBlog(request);
            if (result)
            {
                TempData["UpdateProductSuccessful"] = "Cập nhật Blog thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Cập nhật sản phẩm thất bại");
            return View(request);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(new BlogDeleteRequest()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(BlogDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _blogApiClient.DeleteBlog(request.Id);
            if (result)
            {
                TempData["DeleteBlogSuccessful"] = "Xóa Blog thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Xóa không thành công");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var blog = await _blogApiClient.GetById(id);


            var detailVm = new BlogViewModel()
            {

                Name = blog.Name,
                Tille = blog.Tille,
                Image = blog.Image,
                Status = blog.Status,
                Details = blog.Details,
                Starttime = blog.Starttime
            };

            return View(detailVm);
        }
    }
}

