namespace Lukomor.MVVM.ViewModels
{
    public class TestViewModel : IViewModel
    {
        public TestSubViewModel SubViewModel;

        public TestViewModel()
        {
            SubViewModel = new TestSubViewModel();
        }
    }
}