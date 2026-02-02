using System;
using System.Reactive.Subjects;
using Lukomor.MVVM;
using Lukomor.Reactive;

namespace Lukomor.Example.World
{
    public class UIRootViewModel : ViewModel
    {
        private readonly ObjectsService _objectsService;
        private readonly BehaviorSubject<ScreenExampleWorldViewModel> _screen = new(null);
        
        public IReadOnlyReactiveCollection<ObjectViewModel> Objects { get; }
        public IObservable<ScreenExampleWorldViewModel> Screen => _screen;

        public UIRootViewModel(ObjectsService objectsService)
        {
            Objects = objectsService.Objects;
            _screen.OnNext(new ScreenExampleWorldViewModel(objectsService));
        }
    }
}