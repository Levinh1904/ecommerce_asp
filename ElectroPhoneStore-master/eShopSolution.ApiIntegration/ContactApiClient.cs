using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.Contacts;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntegration
{
    public class ContactApiClient : BaseApiClient, IContactApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ContactApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }


        public async Task<bool> CreateContact(ContactCreateRequest request)
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
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.SDT) ? " " : request.Name.ToString()), "sdt");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Gmail) ? " " : request.Name.ToString()), "gmail");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Tille) ? " " : request.Tille.ToString()), "tille");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Status.ToString()) ? " " : request.Status.ToString()), "status");

            var response = await client.PostAsync($"/api/contacts/", requestContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateContact(ContactUpdateRequest request)
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
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.SDT) ? " " : request.Name.ToString()), "sdt");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Gmail) ? " " : request.Name.ToString()), "gmail");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Tille) ? " " : request.Tille.ToString()), "tille");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Status.ToString()) ? " " : request.Status.ToString()), "status");

            var response = await client.PutAsync($"/api/contacts/" + request.Id, requestContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteContact(int id)
        {
            return await Delete($"/api/contacts/" + id);
        }
        public async Task<List<ContactViewModel>> GetAll()
        {
            return await GetListAsync<ContactViewModel>("/api/contacts");
        }

        public async Task<ContactViewModel> GetById(int id)
        {
            return await GetAsync<ContactViewModel>($"/api/contacts/{id}");
        }
        public async Task<PagedResult<ContactViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            var data = await GetAsync<PagedResult<ContactViewModel>>(
               $"/api/contacts/paging?pageIndex={request.PageIndex}" +
               $"&pageSize={request.PageSize}" +
               $"&keyword={request.Keyword}&sortOption={request.SortOption}");

            return data;
        }
        public async Task<ApiResult<string>> ContactCreate(ContactCreateRequest contactRequest)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var json = JsonConvert.SerializeObject(contactRequest);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/contacts", httpContent);

            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<string>>(result);
        }
        public async Task<ApiResult<ContactViewModel>> GetByUserName(string userName)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/users/getByUserName/{userName}");

            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<ContactViewModel>>(body);

            return JsonConvert.DeserializeObject<ApiErrorResult<ContactViewModel>>(body);
        }

    }

}
