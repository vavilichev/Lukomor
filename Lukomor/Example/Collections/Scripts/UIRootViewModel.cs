using System;
using System.Reactive.Subjects;
using Lukomor.MVVM;

namespace Lukomor.Example.Collections
{
    public class UIRootViewModel : ViewModel
    {
        private readonly BehaviorSubject<ScreenExampleCollectionBindersViewModel> _screen = new(null);

        public IObservable<ScreenExampleCollectionBindersViewModel> Screen => _screen;

        public UIRootViewModel()
        {
            _screen.OnNext(new ScreenExampleCollectionBindersViewModel());
        }
    }
}