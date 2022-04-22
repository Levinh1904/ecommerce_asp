
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Catalog.Slides;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntegration
{
    public interface ISlideApiClient
    {
        Task<bool> CreateSlide(SlideCreateRequest request);

        Task <bool> UpdateSlide(SlideUpdateRequest request);

        Task<bool> DeleteSlide(int id);

        Task<SlideViewModel> GetById(int id);

        Task<List<SlideViewModel>> GetAll();
        Task<PagedResult<SlideViewModel>> GetAllPaging(GetManageProductPagingRequest request);
    }
}
