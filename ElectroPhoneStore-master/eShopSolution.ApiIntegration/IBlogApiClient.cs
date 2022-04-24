using eShopSolution.ViewModels.Catalog.Blogs;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntegration
{
    public interface IBlogApiClient
    {
        Task<bool> CreateBlog(BlogCreateRequest request);

        Task<bool> UpdateBlog(BlogUpdateRequest request);

        Task<bool> DeleteBlog(int id);

        Task<BlogViewModel> GetById(int id);

        Task<List<BlogViewModel>> GetAll();
        Task<PagedResult<BlogViewModel>> GetAllPaging(GetManageProductPagingRequest request);
    }
}
