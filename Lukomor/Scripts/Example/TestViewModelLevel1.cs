using Lukomor.MVVM;
using Lukomor.Reactive;

namespace Lukomor.Example
{
    public class TestViewModelLevel1 : ViewModel
    {
        public ReactiveProperty<TestViewModelLevel2> Level20 { get; } = new();
        public ReactiveProperty<TestViewModelLevel2> Level21 { get; } = new();
        public ReactiveProperty<TestViewModelLevel2> Level22 { get; } = new();
    }
}