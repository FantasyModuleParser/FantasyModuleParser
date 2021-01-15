using System.Windows.Input;

namespace FantasyModuleParser.Tables.ViewModels
{
    public interface ICellViewModel
    {
        string Content { get; set; }
        ICommand ChangeCellStateCommand { get; }
    }
}
