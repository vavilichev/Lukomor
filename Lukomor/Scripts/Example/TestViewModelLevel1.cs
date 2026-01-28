using Lukomor.MVVM;
using Lukomor.Reactive;

namespace Lukomor.Example
{
    public class TestViewModelLevel1 : ViewModel
    {
        public ReactiveProperty<TestViewModelLevel2> Level20 { get; } = new();
        public ReactiveProperty<TestViewModelLevel2> Level21 { get; } = new();
        public ReactiveProperty<TestViewModelLevel2> Level22 { get; } = new();
        public ReactiveProperty<string> StringPropertyOne { get; } = new();
        public ReactiveProperty<string> StringPropertyTwo { get; } = new();
        public ReactiveProperty<int> IntPropertyOne { get; } = new();
        public ReactiveProperty<int> IntPropertyTwo { get; } = new();
        public ReactiveCollection<TestViewModelLevel1> Children { get; } = new();
        
        public ICommand EmptyCommand1 { get; }
        public ICommand EmptyCommand2 { get; }
        public ICommand<int> IntCommand1 { get; }
        public ICommand<int> IntCommand2 { get; }
        public ICommand<bool> BoolCommand1 { get; }
        public ICommand<bool> BoolCommand2 { get; }
    }
}