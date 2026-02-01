using System;
using System.Reactive.Subjects;
using Lukomor.MVVM;

namespace Lukomor.Example.SimpleBinders
{
    public class ScreenExampleSimpleBindersViewModel : ViewModel
    {
        private readonly BehaviorSubject<bool> _booleanValue = new(false);
        private readonly BehaviorSubject<string> _stringValue = new(string.Empty);
        
        public IObservable<bool> BooleanValue => _booleanValue;
        public IObservable<string> StringValue => _stringValue;
        public ICommand CmdSwitchBoolean { get; private set; }
        
        public ScreenExampleSimpleBindersViewModel()
        {
            CmdSwitchBoolean = new Command(SwitchBoolean);
            SwitchBoolean();
        }
        
        private void SwitchBoolean()
        {
            _booleanValue.OnNext(!_booleanValue.Value);
            var stringValue = _booleanValue.Value ? "True" :  "False";
            _stringValue.OnNext(stringValue);
        }
    }
}