using eShopSolution.ViewModels.Catalog.Contacts;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Contacts
{
    public interface IContactService
    {
        Task<int> Create(ContactCreateRequest request);

        // Create và Update truyền 1 Dtos vào phương thức
        // Dtos là Data transfer object ( giống view model truyền cho 1 view )
        Task<int> Update(ContactUpdateRequest request);

        // để xóa thì ta chỉ cần truyền vào 1 product id
        Task<int> Delete(int blogId);
        Task<PagedResult<ContactViewModel>> GetAllPaging(GetManageProductPagingRequest request);

        Task<ContactViewModel> GetById(int id);

        Task<List<ContactViewModel>> GetAll();
    }
}
