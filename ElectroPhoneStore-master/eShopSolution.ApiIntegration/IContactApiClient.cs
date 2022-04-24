using eShopSolution.ViewModels.Catalog.Contacts;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntegration
{
    public interface IContactApiClient
    {
        Task<bool> CreateContact(ContactCreateRequest request);

        Task<bool> UpdateContact(ContactUpdateRequest request);

        Task<bool> DeleteContact(int id);

        Task<ContactViewModel> GetById(int id);

        Task<List<ContactViewModel>> GetAll();
        Task<PagedResult<ContactViewModel>> GetAllPaging(GetManageProductPagingRequest request);
    }
}
