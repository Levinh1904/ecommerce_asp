using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.Blogs;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntegration
{
    public class BlogApiClient : BaseApiClient, IBlogApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public BlogApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }


        public async Task<bool> CreateBlog(BlogCreateRequest request)
        {
            var sessions = _httpContextAccessor
                .HttpContext
                .Session
                .GetString(SystemConstants.AppSettings.Token);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var requestContent = new MultipartFormDataContent();

            if (request.Image != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.Image.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.Image.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "image", request.Image.FileName);
            }
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Name) ? " " : request.Name.ToString()), "name");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Tille) ? " " : request.Tille.ToString()), "tille");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Details) ? " " : request.Details.ToString()), "details");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Starttime.ToString()) ? " " : request.Starttime.ToString()), "starttime");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Status.ToString()) ? " " : request.Status.ToString()), "status");
            

            var response = await client.PostAsync($"/api/blogs/", requestContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateBlog(BlogUpdateRequest request)
        {
            var sessions = _httpContextAccessor
                .HttpContext
                .Session
                .GetString(SystemConstants.AppSettings.Token);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var requestContent = new MultipartFormDataContent();

            if (request.Image != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.Image.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.Image.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "thumbnailImage", request.Image.FileName);
            }
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Name) ? " " : request.Name.ToString()), "name");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Tille) ? " " : request.Tille.ToString()), "tille");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Status.ToString()) ? " " : request.Status.ToString()), "status");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Details) ? " " : request.Details.ToString()), "details");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Starttime.ToString()) ? " " : request.Starttime.ToString()), "starttime");

            var response = await client.PutAsync($"/api/blogs/" + request.Id, requestContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteBlog(int id)
        {
            return await Delete($"/api/blogs/" + id);
        }
        public async Task<List<BlogViewModel>> GetAll()
        {
            return await GetListAsync<BlogViewModel>("/api/blogs");
        }

        public async Task<BlogViewModel> GetById(int id)
        {
            return await GetAsync<BlogViewModel>($"/api/blogs/{id}");
        }
        public async Task<PagedResult<BlogViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            var data = await GetAsync<PagedResult<BlogViewModel>>(
               $"/api/blogs/paging?pageIndex={request.PageIndex}" +
               $"&pageSize={request.PageSize}" +
               $"&keyword={request.Keyword}&sortOption={request.SortOption}");

            return data;
        }
    }
}
