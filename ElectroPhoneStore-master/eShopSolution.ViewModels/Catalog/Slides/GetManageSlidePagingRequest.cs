using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.Slides
{
    public class GetManageSlidePagingRequest
    {
        public string LanguageId { get; set; }
        public string Keyword { get; set; }
        // lấy trang số bao nhiêu
        public int PageIndex { get; set; }

        // kích cỡ của trang là bao nhiêu
        public int PageSize { get; set; }
        public string? SortOption { get; set; }
    }
}
