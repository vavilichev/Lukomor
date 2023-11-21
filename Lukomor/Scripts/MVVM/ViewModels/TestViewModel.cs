using Lukomor.Reactive;

namespace Lukomor.MVVM.ViewModels
{
    public class TestViewModel : IViewModel
    {
        public TestSubViewModel SubViewModel { get; }
        public TestSubViewModel SubViewModel2 { get; }
        public TestViewModelOlolo SubViewModelsalkjdklsakhjkajshd { get; }
        public SingleReactiveProperty<int> SomeInt { get; } = new(1923);
        public IReactiveProperty<IViewModel> SomeModel => _someModel;

        public IReadOnlyReactiveCollection<IViewModel> SubViewModels => _subViewModels;

        private ReadOnlyReactiveCollection<IViewModel> _subViewModels = new();
        private ReactiveProperty<IViewModel> _someModel = new();
        
        public TestViewModel()
        {
            SubViewModel = new TestSubViewModel();
            
            _subViewModels.Add(new TestViewModel());
        }

        public void SomeMethod()
        {
            
        }
    }
}