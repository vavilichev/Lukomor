using System;
using System.Reactive.Subjects;
using Lukomor.MVVM;

namespace Lukomor.Example.SimpleBinders
{
    public class UIRootViewModel : ViewModel
    {
        private readonly BehaviorSubject<ScreenExampleSimpleBindersViewModel> _screen = new(null);

        public IObservable<ScreenExampleSimpleBindersViewModel> Screen => _screen;

        public UIRootViewModel()
        {
            _screen.OnNext(new ScreenExampleSimpleBindersViewModel());
        }
    }
}