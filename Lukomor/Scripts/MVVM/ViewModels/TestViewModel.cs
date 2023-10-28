using Lukomor.Reactive;

namespace Lukomor.MVVM.ViewModels
{
    public class TestViewModel : IViewModel
    {
        public TestSubViewModel SubViewModel { get; }
        public TestSubViewModel SubViewModel2 { get; }
        public TestViewModelOlolo SubViewModelsalkjdklsakhjkajshd { get; }

        public TestViewModel()
        {
            SubViewModel = new TestSubViewModel();
        }
    }
}