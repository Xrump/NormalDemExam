using Microsoft.EntityFrameworkCore;
using Shirov.Lopushok.Domain.Entities;
using Shirov.Lopushok.Infrastructure.Persistence;
using Shirov.Lopushok.Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shirov.Lopushok.Presentation.Commands
{
    public class SearchCommand : RelayCommand
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        public SearchCommand(MainWindowViewModel viewModel)
        {
            _mainWindowViewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                if (_mainWindowViewModel.SearchText=="")
                {
                    _mainWindowViewModel.Product = context.Products
                         .Include(p => p.ProductType)
                         .ToList();
                }
                else 
                {
                    _mainWindowViewModel.Product = context.Products
                         .Include(p => p.ProductType)
                         .Where(p=>p.Title.ToLower().Contains(_mainWindowViewModel.SearchText.ToLower()))
                         .ToList();
                }
            }
        }
    }
}
