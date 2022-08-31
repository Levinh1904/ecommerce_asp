using eShopSolution.ApiIntegration;
using eShopSolution.ViewModels.Catalog.Contacts;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Controllers
{
    public class ContactController : BaseController
    {
        private readonly IContactApiClient _contactApiClient;

        public ContactController(IContactApiClient contactApiClient)
        {
            _contactApiClient = contactApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetManageProductPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };

            var data = await _contactApiClient.GetAllPaging(request);
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
        public async Task<IActionResult> Create([FromForm] ContactCreateRequest request)
        {

            var result = await _contactApiClient.CreateContact(request);
            if (result)
            {
                TempData["CreateContactSuccessful"] = "Thêm mới Contact thành công";
                return RedirectToAction(nameof(SuccessRegistration));
            }

            ModelState.AddModelError("", "Thêm Contact thất bại");
            return View(request);
        }
        [HttpGet]
        public IActionResult SuccessRegistration()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var contact = await _contactApiClient.GetById(id);
            var editVm = new ContactUpdateRequest()
            {
                Id = contact.Id,
                Name = contact.Name,
                SDT = contact.SDT,
                Gmail = contact.Gmail,
                Tille = contact.Tille,
            };

            return View(editVm);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit([FromForm] ContactUpdateRequest request)
        {


            var result = await _contactApiClient.UpdateContact(request);
            if (result)
            {
                TempData["UpdateProductSuccessful"] = "Cập nhật Contact thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Cập nhật sản phẩm thất bại");
            return View(request);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(new ContactDeleteRequest()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ContactDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _contactApiClient.DeleteContact(request.Id);
            if (result)
            {
                TempData["DeleteContactSuccessful"] = "Xóa Contact thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Xóa không thành công");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var contact = await _contactApiClient.GetById(id);


            var detailVm = new ContactViewModel()
            {

                Name = contact.Name,
                Tille = contact.Tille,
                SDT = contact.SDT,
                Gmail = contact.Gmail,
                Image = contact.Image,
                Status = contact.Status
            };

            return View(detailVm);
        }
    }
}
