using System.Collections;
using System.Collections.Generic;
using Lukomor.MVVM;
using Lukomor.Reactive;
using UnityEngine;

namespace Lukomor
{
    public class ExampleScreenGamePlayViewModel : WindowViewModel
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
