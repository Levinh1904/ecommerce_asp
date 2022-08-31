using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Catalog.Slides;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Slides
{
    public interface ISlideService
    {
        // trả về kiểu int là trả về mã SP ta vừa tạo
        // tham số không phải lúc nào cũng truyền vào 1 Product view model, nhiều khi sẽ bị thừa
        Task<int> Create(SlideCreateRequest request);

        // Create và Update truyền 1 Dtos vào phương thức
        // Dtos là Data transfer object ( giống view model truyền cho 1 view )
        Task<int> Update(SlideUpdateRequest request);

        // để xóa thì ta chỉ cần truyền vào 1 product id
        Task<int> Delete(int slideId);
        Task<PagedResult<SlideViewModel>> GetAllPaging(GetManageProductPagingRequest request);

        Task<SlideViewModel> GetById(int id);

        Task<List<SlideViewModel>> GetAll();
    }
}
