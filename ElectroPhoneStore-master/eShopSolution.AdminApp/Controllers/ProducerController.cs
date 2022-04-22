using Microsoft.AspNetCore.Mvc;
using eShopSolution.ViewModels.Catalog.Producers;
using System.Threading.Tasks;
using eShopSolution.ApiIntegration;
using eShopSolution.ViewModels.Catalog.Products;

namespace eShopSolution.AdminApp.Controllers
{
    public class ProducerController : BaseController
    {
        private readonly IProducerApiClient _producerApiClient;

        public ProducerController(IProducerApiClient producerApiClient)
        {
            _producerApiClient = producerApiClient;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetManageProductPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };

            var data = await _producerApiClient.GetAllPaging(request);
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
        public async Task<IActionResult> Create([FromForm] ProducerCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _producerApiClient.CreateProducer(request);
            if (result)
            {
                TempData["CreateProductSuccessful"] = "Thêm mới sản phẩm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Thêm sản phẩm thất bại");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var producer = await _producerApiClient.GetById(id);

            var editVm = new ProducerUpdateRequest()
            {
                Id = id,
                Name = producer.Name
            };

            return View(editVm);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit([FromForm] ProducerUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _producerApiClient.UpdateProducer(request);
            if (result)
            {
                TempData["UpdateProducerSuccessful"] = "Cập nhật danh mục thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Cập nhật danh mục thất bại");
            return View(request);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(new ProducerDeleteRequest()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProducerDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _producerApiClient.DeleteProducer(request.Id);
            if (result)
            {
                TempData["DeleteProducerSuccessful"] = "Xóa danh mục thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Xóa danh mục không thành công");
            return View(request);
        }
    }
}
