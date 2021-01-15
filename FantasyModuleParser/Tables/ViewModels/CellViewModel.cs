using FantasyModuleParser.NPC.ViewModel;
using FantasyModuleParser.Tables.Models;
using System.Windows.Input;

namespace FantasyModuleParser.Tables.ViewModels
{
    public class CellViewModel : ViewModelBase, ICellViewModel
    {
        private string _content;
        public string Content { get { return _content; } set { SetProperty(ref _content, value); } }

        public CellViewModel(string content = null)
        {
            ChangeCellStateCommand = new DelegateCommand(ChangeCellState, CanChangeCellState);
            if (content != null)
                this.Content = content;
        }

        public ICommand ChangeCellStateCommand { get; }

        private void ChangeCellState(object obj)
        {
            if (Content != null)
            {
                // FIXME:  VALIDATE IF obj IS THE DATA WRITTEN ON THE GRID!!!
                Content = obj.ToString();
            }
                
        }

        private static bool CanChangeCellState(object obj)
        {
            return true;
        }
    }
}
