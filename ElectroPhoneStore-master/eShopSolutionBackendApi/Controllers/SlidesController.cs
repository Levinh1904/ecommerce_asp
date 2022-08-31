using eShopSolution.Application.Catalog.Slides;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Catalog.Slides;
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
    public class SlidesController : ControllerBase
    {
        private readonly ISlideService _slideService;

        public SlidesController(ISlideService slideService)
        {
            _slideService = slideService;
        }
        [HttpGet("{slideId}")]
        public async Task<IActionResult> GetById(int slideId)
        {
            var slide = await _slideService.GetById(slideId);
            if (slide == null)
                return BadRequest("Không tìm thấy Slide");
            return Ok(slide);
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] SlideCreateRequest request)
        {
            //kiểm tra validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var slideId = await _slideService.Create(request);
            if (slideId == 0)
            {
                return BadRequest();
            }

            var slide = await _slideService.GetById(slideId);

            return CreatedAtAction(nameof(GetById), new { id = slideId }, slide);
        }
        // HttpPut: update toàn phần
        [HttpPut("{slideId}")]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int slideId, [FromForm] SlideUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            request.Id = slideId;
            var affectedResult = await _slideService.Update(request);
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
            var affectedResult = await _slideService.Delete(id);
            if (affectedResult == 0)
            {
                return BadRequest();
            }

            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var slide = await _slideService.GetAll();
            return Ok(slide);
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetManageProductPagingRequest request)
        {
            var slide = await _slideService.GetAllPaging(request);
            return Ok(slide);
        }
    }
}
