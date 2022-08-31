using eShopSolution.Application.Catalog.Blogs;
using eShopSolution.ViewModels.Catalog.Blogs;
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
    public class BlogsController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogsController(IBlogService blogService)
        {
            _blogService = blogService;
        }
        [HttpGet("{blogId}")]
        public async Task<IActionResult> GetById(int blogId)
        {
            var blog = await _blogService.GetById(blogId);
            if (blog == null)
                return BadRequest("Không tìm thấy Blog");
            return Ok(blog);
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] BlogCreateRequest request)
        {
            //kiểm tra validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var blogId = await _blogService.Create(request);
            if (blogId == 0)
            {
                return BadRequest();
            }

            var blog = await _blogService.GetById(blogId);

            return CreatedAtAction(nameof(GetById), new { id = blogId }, blog);
        }
        // HttpPut: update toàn phần
        [HttpPut("{blogId}")]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int blogId, [FromForm] BlogUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            request.Id = blogId;
            var affectedResult = await _blogService.Update(request);
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
            var affectedResult = await _blogService.Delete(id);
            if (affectedResult == 0)
            {
                return BadRequest();
            }

            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var blog = await _blogService.GetAll();
            return Ok(blog);
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetManageProductPagingRequest request)
        {
            var blog = await _blogService.GetAllPaging(request);
            return Ok(blog);
        }
    }
}
