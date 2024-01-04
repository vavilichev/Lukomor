using System;
using Lukomor.MVVM;
using Lukomor.Reactive;

namespace Lukomor.Example
{
    public class ExampleUIRootViewModel : IViewModel
    {
        public ReactiveProperty<ExampleWindowViewModel> OpenedScreen { get; } = new();
        public ReactiveProperty<ExampleWindowViewModel> OpenedPopup { get; } = new();
        
        private readonly Func<ExampleMainMenuViewModel> _createMainMenuViewModel;
        private readonly Func<string, ExampleScreenGamePlayViewModel> _createGameplayScreenViewModel;
        private readonly Func<string, Action, Action, ExamplePopupAreYouSureViewModel> _createAreYouSureViewModel;
        private readonly Func<ExampleScreenQuestsViewModel> _createQuestsScreenViewModel;

        public ExampleUIRootViewModel(
            Func<ExampleMainMenuViewModel> createMainMenuViewModel, 
            Func<string, ExampleScreenGamePlayViewModel> createGameplayScreenViewModel,
            Func<string, Action, Action, ExamplePopupAreYouSureViewModel> createAreYouSureViewModel,
            Func<ExampleScreenQuestsViewModel> createQuestsScreenViewModel)
        {
            _createMainMenuViewModel = createMainMenuViewModel;
            _createGameplayScreenViewModel = createGameplayScreenViewModel;
            _createAreYouSureViewModel = createAreYouSureViewModel;
            _createQuestsScreenViewModel = createQuestsScreenViewModel;
        }
        
        public void OpenMainMenuScreen()
        {
            CloseCurrentScreen();
            OpenedScreen.Value = _createMainMenuViewModel();
        }

        public void OpenGameplayScreen(string text)
        {
            CloseCurrentScreen();
            OpenedScreen.Value = _createGameplayScreenViewModel(text);
        }

        public void OpenQuestsScreen()
        {
            CloseCurrentScreen();
            OpenedScreen.Value = _createQuestsScreenViewModel();
        }

        public void OpenAreYouSurePopup(string text, Action yesCallback, Action noCallback = null)
        {
            CloseCurrentPopup();

            OpenedPopup.Value = _createAreYouSureViewModel(text, yesCallback, noCallback);
        }

        private void CloseCurrentScreen()
        {
            OpenedScreen.Value?.Close();
        }

        private void CloseCurrentPopup()
        {
            OpenedPopup.Value?.Close();
        }
    }
}