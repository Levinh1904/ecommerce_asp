using eShopSolution.ApiIntegration;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Catalog.Slides;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Controllers
{
    public class SlideController : BaseController
    {
        private readonly ISlideApiClient _slideApiClient;

        public SlideController(ISlideApiClient slideApiClient)
        {
            _slideApiClient = slideApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetManageProductPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };

            var data = await _slideApiClient.GetAllPaging(request);
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
        public async Task<IActionResult> Create([FromForm] SlideCreateRequest request)
        {

            var result = await _slideApiClient.CreateSlide(request);
            if (result)
            {
                TempData["CreateSlideSuccessful"] = "Thêm mới Slide thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Thêm Slide thất bại");
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var slide = await _slideApiClient.GetById(id);
            var editVm = new SlideUpdateRequest()
            {
                Id = slide.Id,
                Name = slide.Name,
                Tille = slide.Tille,
                Color = slide.Color,
                Status = slide.Status,
            };

            return View(editVm);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit([FromForm] SlideUpdateRequest request)
        {
       

            var result = await _slideApiClient.UpdateSlide(request);
            if (result)
            {
                TempData["UpdateProductSuccessful"] = "Cập nhật Slide thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Cập nhật sản phẩm thất bại");
            return View(request);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(new SlideDeleteRequest()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SlideDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _slideApiClient.DeleteSlide(request.Id);
            if (result)
            {
                TempData["DeleteSlideSuccessful"] = "Xóa Slide thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Xóa không thành công");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var slide = await _slideApiClient.GetById(id);


            var detailVm = new SlideViewModel()
            {
               
                Name = slide.Name,
                Tille = slide.Tille,
                Color = slide.Color,
                Image = slide.Image,
                Status = slide.Status
            };

            return View(detailVm);
        }
    }
}
