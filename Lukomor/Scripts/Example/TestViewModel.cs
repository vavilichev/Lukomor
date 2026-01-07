using Lukomor.MVVM;
using Lukomor.Reactive;

namespace Lukomor.Example.TestNamespaceLength
{
    public class TestViewModel : ViewModel
    {
        public ReactiveProperty<TestViewModelLevel2> TestPropertyLevel22 { get; } = new();
        public ReactiveProperty<TestViewModel2> Level2 { get; } = new();
        public ReactiveProperty<TestViewModelLevel2> Level22 { get; } = new();
    }
}