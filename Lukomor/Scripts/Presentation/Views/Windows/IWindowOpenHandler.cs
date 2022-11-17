using Lukomor.Common;

namespace Lukomor.Presentation.Views.Windows
{
    public interface IWindowOpenHandler
    {
        WindowViewModel OpeningWindowViewModel { get; }
        IWindow OpeningWindow => OpeningWindowViewModel.Window;
        IWindowOpenHandler AddPayloads(params Payload[] payloads);
        void WithBackDestination<TWindowViewModel>() where TWindowViewModel : WindowViewModel;
    }
}