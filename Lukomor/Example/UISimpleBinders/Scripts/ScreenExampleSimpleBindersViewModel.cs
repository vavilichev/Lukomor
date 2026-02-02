using System;
using System.Reactive.Subjects;
using Lukomor.MVVM;

namespace Lukomor.Example.UISimpleBinders
{
    public class ScreenExampleSimpleBindersViewModel : ViewModel
    {
        private readonly BehaviorSubject<bool> _booleanValue = new(false);
        
        public IObservable<bool> BooleanValue => _booleanValue;
        public ICommand CmdSwitchBoolean { get; private set; }
        
        public ScreenExampleSimpleBindersViewModel()
        {
            CmdSwitchBoolean = new Command(SwitchBoolean);
            SwitchBoolean();
        }
        
        private void SwitchBoolean()
        {
            _booleanValue.OnNext(!_booleanValue.Value);
        }
    }
}