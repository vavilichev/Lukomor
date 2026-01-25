using System;
using Lukomor.MVVM;
using Lukomor.Reactive;

namespace Lukomor.Example
{
    public class TestViewModelLevel2 : ViewModel
    {
        private readonly ReactiveProperty<string> _text = new();
        private readonly ReactiveCollection<TestViewModelLevel3> _viewModels3 = new();

        public ReactiveProperty<TestViewModelLevel3> Level30 { get; } = new();
        public IObservable<string> Text => _text;
        public IReadOnlyReactiveCollection<TestViewModelLevel3> ViewModels3 => _viewModels3;

        public TestViewModelLevel2()
        {
            _text.Value = "Ololololo";
        }
    }
}