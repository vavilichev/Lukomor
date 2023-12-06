using System;
using Lukomor.MVVM;
using PlasticGui.WorkspaceWindow.CodeReview.Summary;
using UnityEngine;

namespace Lukomor
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

            _uiRootViewModel = new ExampleUIRootViewModel(
                createMainMenuViewModel, 
                createGameplayScreenViewModel,
                createAreYouSureViewModel);

            _uiRootView.Bind(_uiRootViewModel);
            
            _uiRootViewModel.OpenMainMenuScreen();
        }
    }
}