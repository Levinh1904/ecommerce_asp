using eShopSolution.Application.Catalog.Produ;
using eShopSolution.ViewModels.Catalog.Producers;
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
    public class ProducersController : ControllerBase
    {
        private readonly IProducerService _producerService;

        public ProducersController(
            IProducerService producerService)
        {
            _producerService = producerService;
        }

        public async Task<IActionResult> GetById(int producerId)
        {
            var producer = await _producerService.GetById(producerId);
            if (producer == null)
                return BadRequest("Không tìm thấy Producer");
            return Ok(producer);
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] ProducerCreateRequest request)
        {
            //kiểm tra validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var producerId = await _producerService.Create(request);
            if (producerId == 0)
            {
                return BadRequest();
            }

            var producer = await _producerService.GetById(producerId);

            return CreatedAtAction(nameof(GetById), new { id = producerId }, producer);
        }
        // HttpPut: update toàn phần
        [HttpPut("{producerId}")]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int producerId, [FromForm] ProducerUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            request.Id = producerId;
            var affectedResult = await _producerService.Update(request);
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
            var affectedResult = await _producerService.Delete(id);
            if (affectedResult == 0)
            {
                return BadRequest();
            }

            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var producer = await _producerService.GetAll();
            return Ok(producer);
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetManageProductPagingRequest request)
        {
            var producer = await _producerService.GetAllPaging(request);
            return Ok(producer);
        }
    }
}
