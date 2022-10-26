using Microsoft.EntityFrameworkCore;
using Shirov.Lopushok.Domain.Entities;
using Shirov.Lopushok.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shirov.Lopushok.Presentation.ViewModels
{
    public class MainWindowViewModel:ViewModelBase
    {
        public List<Product> Product { get; set; }
        public MainWindowViewModel()
        {
            using(ApplicationDbContext context=new ApplicationDbContext())
            {
                Product = context.Products
                    .Include(p=>p.ProductType)
                    .ToList();
            }
        }
    }
}
