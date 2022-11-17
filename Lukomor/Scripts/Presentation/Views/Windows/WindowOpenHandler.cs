using Lukomor.Common;

namespace Lukomor.Presentation.Views.Windows
{
    public class WindowOpenHandler : IWindowOpenHandler
    {
        public WindowViewModel OpeningWindowViewModel { get; }

        private UserInterface _ui;
        
        public WindowOpenHandler(WindowViewModel windowViewModel, UserInterface ui)
        {
            OpeningWindowViewModel = windowViewModel;
            _ui = ui;
        }

        public IWindowOpenHandler AddPayloads(params Payload[] payloads)
        {
            OpeningWindowViewModel.AddPayloads(payloads);

            return this;
        }

        public void WithBackDestination<TWindowViewModel>() where TWindowViewModel : WindowViewModel
        {
            _ui.SetBackDestination<TWindowViewModel>();
        }
    }
}