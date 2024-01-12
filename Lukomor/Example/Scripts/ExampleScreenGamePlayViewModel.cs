using Lukomor.Reactive;

namespace Lukomor.Example
{
    public class ExampleScreenGamePlayViewModel : ExampleWindowViewModel
    {
        public ReactiveProperty<string> GameplayText { get; }
        
        private ExampleUIRootViewModel _uiRootViewModel;
        
        public ExampleScreenGamePlayViewModel(ExampleUIRootViewModel uiRootViewModel, string text)
        {
            _uiRootViewModel = uiRootViewModel;
            GameplayText = new ReactiveProperty<string>(text);
        }
        
        public void OnMainMenuButtonClick()
        {
            _uiRootViewModel.OpenMainMenuScreen();
        }
    }
}
