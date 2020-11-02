namespace FantasyModuleParser.NPC.ViewModel
{
    public class NPCViewModel : ViewModelBase
    {
        private NPCModel _npcModel;
        public NPCModel NpcModel
        {
            get => _npcModel;
            set => SetProperty(ref _npcModel, value);
        }
    }
}
