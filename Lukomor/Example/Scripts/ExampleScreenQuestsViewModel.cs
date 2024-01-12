using Lukomor.Reactive;

namespace Lukomor.Example
{
    public class ExampleScreenQuestsViewModel : ExampleWindowViewModel
    {
        public ReadOnlyReactiveCollection<ExampleQuestViewModelBase> Quests { get; } = new();
        
        private readonly ExampleUIRootViewModel _uiRootViewModel;

        public ExampleScreenQuestsViewModel(ExampleUIRootViewModel uiRootViewModel)
        {
            _uiRootViewModel = uiRootViewModel;
            Quests.Add(new ExampleQuestViewModel1("Quest 11"));
            Quests.Add(new ExampleQuestViewModel1("Quest 12"));
            Quests.Add(new ExampleQuestViewModel2("Quest 2", "1"));
            Quests.Add(new ExampleQuestViewModel2("Quest 2", "2"));
            Quests.Add(new ExampleQuestViewModel1("Quest 13"));
        }

        public void OnMainMenuButtonClick()
        {
            _uiRootViewModel.OpenMainMenuScreen();
        }
    }
}