using UnityEngine;

namespace Lukomor.Presentation.Views.Windows
{
    public class WindowViewModel : ViewModel
    {
        [SerializeField] private WindowSettings _windowSettings;

        public WindowSettings WindowSettings => _windowSettings;
        public IWindow Window {
            get
            {
                if (_window == null)
                {
                    _window = (IWindow) View;
                }

                return _window;
            }
        }

        private IWindow _window;
    }
}