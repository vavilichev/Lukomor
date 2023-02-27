namespace Lukomor.UI
{
    public interface IView<out TViewModel> : IView where TViewModel : ViewModel
    {
        TViewModel ViewModel { get; }
    }

    public interface IView
    {
        bool IsActive { get; }
    }
}