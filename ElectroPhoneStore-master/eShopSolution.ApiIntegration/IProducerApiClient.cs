using eShopSolution.ViewModels.Catalog.Producers;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntegration
{
    public interface IProducerApiClient
    {
        Task<PagedResult<ProducerViewModel>> GetAllPaging(GetManageProductPagingRequest request);

        Task<List<ProducerViewModel>> GetAll();

        Task<ProducerViewModel> GetById(int id);

        Task<bool> CreateProducer(ProducerCreateRequest request);

        Task<bool> UpdateProducer(ProducerUpdateRequest request);

        Task<bool> DeleteProducer(int id);
    }
}
