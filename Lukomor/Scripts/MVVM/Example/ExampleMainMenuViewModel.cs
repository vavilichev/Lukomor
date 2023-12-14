using UnityEditor;

namespace Lukomor
{
    public class ExampleMainMenuViewModel : WindowViewModel
    {
        private readonly ExampleUIRootViewModel _uiRootViewModel;

        public ExampleMainMenuViewModel(ExampleUIRootViewModel uiRootViewModel)
        {
            _uiRootViewModel = uiRootViewModel;
        }
        
        public void OnNewGameButtonClick()
        {
            _uiRootViewModel.OpenAreYouSurePopup("Do you really want to start a new game?", () =>
            {
                _uiRootViewModel.OpenGameplayScreen("New Game Opened");
            });
        }

        public void OnQuestsButtonClick()
        {
            _uiRootViewModel.OpenQuestsScreen();
        }

        public void OnContinueButtonClick()
        {
            _uiRootViewModel.OpenGameplayScreen("Continue Game Opened");
        }

        public void OnExitButtonClick()
        {
            _uiRootViewModel.OpenAreYouSurePopup("Do you really want exit the game?", () =>
            {
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            });
        }
    }
}
