using eShopSolution.ViewModels.Catalog.Blogs;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Blogs
{
    public interface IBlogService
    {
        Task<int> Create(BlogCreateRequest request);

        // Create và Update truyền 1 Dtos vào phương thức
        // Dtos là Data transfer object ( giống view model truyền cho 1 view )
        Task<int> Update(BlogUpdateRequest request);

        // để xóa thì ta chỉ cần truyền vào 1 product id
        Task<int> Delete(int blogId);
        Task<PagedResult<BlogViewModel>> GetAllPaging(GetManageProductPagingRequest request);

        Task<BlogViewModel> GetById(int id);

        Task<List<BlogViewModel>> GetAll();
    }
}
