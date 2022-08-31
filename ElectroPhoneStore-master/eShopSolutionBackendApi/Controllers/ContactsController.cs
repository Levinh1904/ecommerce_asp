using eShopSolution.Application.Catalog.Contacts;
using eShopSolution.ViewModels.Catalog.Contacts;
using eShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolutionBackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }
        [HttpGet("{contactId}")]
        public async Task<IActionResult> GetById(int contactId)
        {
            var contact = await _contactService.GetById(contactId);
            if (contact == null)
                return BadRequest("Không tìm thấy Contact");
            return Ok(contact);
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] ContactCreateRequest request)
        {
            //kiểm tra validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contactId = await _contactService.Create(request);
            if (contactId == 0)
            {
                return BadRequest();
            }

            var contact = await _contactService.GetById(contactId);

            return CreatedAtAction(nameof(GetById), new { id = contactId }, contact);
        }
        // HttpPut: update toàn phần
        [HttpPut("{contactId}")]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int contactId, [FromForm] ContactUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            request.Id = contactId;
            var affectedResult = await _contactService.Update(request);
            if (affectedResult == 0)
            {
                return BadRequest();
            }

            return Ok();
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var affectedResult = await _contactService.Delete(id);
            if (affectedResult == 0)
            {
                return BadRequest();
            }

            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contact = await _contactService.GetAll();
            return Ok(contact);
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetManageProductPagingRequest request)
        {
            var contact = await _contactService.GetAllPaging(request);
            return Ok(contact);
        }
    }
}
