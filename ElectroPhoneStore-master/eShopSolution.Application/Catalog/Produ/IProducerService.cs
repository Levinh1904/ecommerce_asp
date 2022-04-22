using eShopSolution.ViewModels.Catalog.Producers;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Produ
{
    public interface IProducerService
    {
        Task<int> Create(ProducerCreateRequest request);

        Task<int> Update(ProducerUpdateRequest request);

        Task<int> Delete(int categoryId);

        Task<PagedResult<ProducerViewModel>> GetAllPaging(GetManageProductPagingRequest request);

        Task<ProducerViewModel> GetById(int id);

        Task<List<ProducerViewModel>> GetAll();
    }
}
