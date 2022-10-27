using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Shirov.Lopushok.Presentation.Commands
{
    public class RelayCommand : ICommand
    {
        private Action<object> _action;
        private Predicate<object> _predicate;

        public event EventHandler? CanExecuteChanged;
        public virtual bool CanExecute(object? parameter)
        {
            if (_predicate == null)
                return true;
            else
                return _predicate(parameter);
        }

        public virtual void Execute(object? parameter)
        {
            _action(parameter);
        }
    }
}
