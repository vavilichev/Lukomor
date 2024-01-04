using System;
using Lukomor.MVVM;
using UnityEngine;

namespace Lukomor.Example
{
    public class ExampleEntryPoint : MonoBehaviour
    {
        [SerializeField] private View _uiRootView;

        private ExampleUIRootViewModel _uiRootViewModel;
        
        private void Start()
        {
            Func<ExampleUIRootViewModel> getUIRootViewModel = () => _uiRootViewModel;
            Func<ExampleMainMenuViewModel> createMainMenuViewModel = () => new ExampleMainMenuViewModel(getUIRootViewModel());
            Func<string, ExampleScreenGamePlayViewModel> createGameplayScreenViewModel =
                text => new ExampleScreenGamePlayViewModel(getUIRootViewModel(), text);
            Func<string, Action, Action, ExamplePopupAreYouSureViewModel> createAreYouSureViewModel =
                (question, yesCallback, noCallback) =>
                    new ExamplePopupAreYouSureViewModel(question, yesCallback, noCallback);
            Func<ExampleScreenQuestsViewModel> createQuestsScreenViewModel = () => new ExampleScreenQuestsViewModel(getUIRootViewModel());

            _uiRootViewModel = new ExampleUIRootViewModel(
                createMainMenuViewModel, 
                createGameplayScreenViewModel,
                createAreYouSureViewModel,
                createQuestsScreenViewModel);

            _uiRootView.Bind(_uiRootViewModel);
            
            _uiRootViewModel.OpenMainMenuScreen();
        }
    }
}