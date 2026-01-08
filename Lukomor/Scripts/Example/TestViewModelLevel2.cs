using Lukomor.MVVM;
using Lukomor.Reactive;

namespace Lukomor.Example
{
    public class TestViewModelLevel2 : ViewModel
    {
        public ReactiveProperty<TestViewModelLevel3> Level30 { get; } = new();
    }
}