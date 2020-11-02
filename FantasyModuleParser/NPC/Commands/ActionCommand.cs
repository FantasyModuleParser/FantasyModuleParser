using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FantasyModuleParser.NPC.Commands
{
    public class ActionCommand : ICommand
    {
        private readonly Action<object> Action;
        private readonly Predicate<object> Predicate;

        public ActionCommand(Action<object> action) : this(action, x => true)
        {

        }
        public ActionCommand(Action<object> action, Predicate<object> predicate)
        {
            Action = action;
            Predicate = predicate;
        }

        public bool CanExecute(object parameter)
        {
            return Predicate(parameter);
        }
        public void Execute(object parameter)
        {
            Action(parameter);
        }

        //These lines are here to tie into WPF-s Can execute changed pipeline. Don't worry about it.
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }
    }
}
