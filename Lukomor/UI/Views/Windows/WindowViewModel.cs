using UnityEngine;

namespace Lukomor.UI
{
    public class WindowViewModel : ViewModel
    {
        [SerializeField] private WindowSettings windowSettings;

        public WindowSettings WindowSettings => windowSettings;
        public IWindow Window {
            get
            {
                if (window == null)
                {
                    window = (IWindow) View;
                }

                return window;
            }
        }

        private IWindow window;
    }
}