namespace Lukomor.UI
{
    public abstract class WidgetViewModel : ViewModel
    {
        public IWidget Widget {
            get
            {
                if (widget == null)
                {
                    widget = (IWidget) View;
                }

                return widget;
            }
        }

        private IWidget widget;
    }
}