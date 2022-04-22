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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _producerService.GetById(id);
            return Ok(category);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] ProducerCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var producerId = await _producerService.Create(request);

            if (producerId == 0)
                return BadRequest();

            var producer = await _producerService.GetById(producerId);

            return CreatedAtAction(nameof(GetById), new { id = producerId }, producer);
        }

        // HttpPut: update toàn phần
        [HttpPut("updateProducer")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] ProducerUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
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
    }
}
