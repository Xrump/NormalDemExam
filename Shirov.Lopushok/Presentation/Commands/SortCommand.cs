using Shirov.Lopushok.Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shirov.Lopushok.Presentation.Commands
{
    public class SortCommand:RelayCommand
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        public SortCommand(MainWindowViewModel viewModel)
        {
            _mainWindowViewModel = viewModel;
        }
        public override void Execute(object? parameter)
        {
            
        }
    }
}
