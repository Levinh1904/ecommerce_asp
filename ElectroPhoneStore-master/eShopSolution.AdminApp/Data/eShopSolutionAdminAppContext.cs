using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using eShopSolution.ViewModels.Catalog.Slides;

namespace eShopSolution.AdminApp.Data
{
    public class eShopSolutionAdminAppContext : DbContext
    {
        public eShopSolutionAdminAppContext (DbContextOptions<eShopSolutionAdminAppContext> options)
            : base(options)
        {
        }

        public DbSet<eShopSolution.ViewModels.Catalog.Slides.SlideViewModel> SlideViewModel { get; set; }
    }
}
