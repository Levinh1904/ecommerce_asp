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
    }
}
