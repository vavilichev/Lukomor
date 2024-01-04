using System;
using Lukomor.Reactive;

namespace Lukomor.Example
{
    public class ExamplePopupAreYouSureViewModel : ExampleWindowViewModel
    {
        public ReactiveProperty<string> Question { get; }
        
        private event Action _yesCallback;
        private event Action _noCallback;

        public ExamplePopupAreYouSureViewModel(string questionText, Action yesCallback, Action noCallback = null)
        {
            Question = new ReactiveProperty<string>(questionText);
            _yesCallback = yesCallback;
            _noCallback = noCallback;
        }

        public void YesButtonClicked()
        {
            _yesCallback?.Invoke();
            
            Close();
        }

        public void NoButtonClick()
        {
            _noCallback?.Invoke();
            
            Close();
        }
    }
}