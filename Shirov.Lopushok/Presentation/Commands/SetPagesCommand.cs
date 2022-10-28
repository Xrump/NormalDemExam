using Shirov.Lopushok.Domain.Entities;
using Shirov.Lopushok.Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shirov.Lopushok.Presentation.Commands
{
    public class SetPagesCommand:Command
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        public SetPagesCommand(MainWindowViewModel viewModel)
        {
            _mainWindowViewModel = viewModel;
        }
        public override void Execute(object parameter)
        {
            List<List<Product>> pages=new List<List<Product>>();

            int pagesCount = (_mainWindowViewModel.Product.Count % 20 == 0) ? _mainWindowViewModel.Product.Count / 20 : _mainWindowViewModel.Product.Count / 20 + 1;
            int lastPageItemsCount = (_mainWindowViewModel.Product.Count % 20 == 0) ? 0 : _mainWindowViewModel.Product.Count % 20;

            for (int i = 0; i < pagesCount; i++)
            {
                pages.Add((_mainWindowViewModel.Product.Skip(20 * i).Count()) < 20 ? _mainWindowViewModel.Product.Skip(20 * i).Take(lastPageItemsCount).ToList() : _mainWindowViewModel.Product.Skip(20 * i).Take(20).ToList());
            }
            
            _mainWindowViewModel.ProductPages=pages;
            if(pages.Count>0)
                _mainWindowViewModel.Product = pages[0];
        }
    }
}
