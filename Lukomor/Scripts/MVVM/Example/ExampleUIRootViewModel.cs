using System;
using Lukomor.MVVM;
using Lukomor.Reactive;

namespace Lukomor
{
    public class ExampleUIRootViewModel : IViewModel
    {
        public ReactiveProperty<WindowViewModel> OpenedScreen { get; } = new();
        public ReactiveProperty<WindowViewModel> OpenedPopup { get; } = new();

        private readonly Func<ExampleMainMenuViewModel> _createMainMenuViewModel;
        private readonly Func<string, ExampleScreenGamePlayViewModel> _createGameplayScreenViewModel;
        private readonly Func<string, Action, Action, ExamplePopupAreYouSureViewModel> _createAreYouSureViewModel;

        public ExampleUIRootViewModel(
            Func<ExampleMainMenuViewModel> createMainMenuViewModel, 
            Func<string, ExampleScreenGamePlayViewModel> createGameplayScreenViewModel,
            Func<string, Action, Action, ExamplePopupAreYouSureViewModel> createAreYouSureViewModel)
        {
            _createMainMenuViewModel = createMainMenuViewModel;
            _createGameplayScreenViewModel = createGameplayScreenViewModel;
            _createAreYouSureViewModel = createAreYouSureViewModel;
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