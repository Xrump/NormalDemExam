using Microsoft.EntityFrameworkCore;
using Shirov.Lopushok.Domain.Entities;
using Shirov.Lopushok.Infrastructure.Persistence;
using Shirov.Lopushok.Presentation.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Shirov.Lopushok.Presentation.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _searchText;
        private List<Product> _product;

        public List<Product> Product { get => _product; set => Set(ref _product , value,nameof(Product)); }
        public List<string> Filter { get; set; } = new List<string>();
        public string SearchText { get => _searchText; set => Set(ref _searchText, value, nameof(SearchText)); }
        public ICommand Search => new SearchCommand(this);

        public MainWindowViewModel()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Product = context.Products
                    .Include(p => p.ProductType)
                    .ToList();

                Filter.Add("Все типы");
                List<ProductType> productType = context.ProductTypes
                    .ToList();

                foreach (ProductType productType1 in productType)
                    Filter.Add(productType1.Title);
            }
        }
    }
}
