namespace Lukomor.UI
{
    public interface IWidget : IView { }

    public interface IWidget<TWidgetViewModel> : IView<TWidgetViewModel>, IWidget where TWidgetViewModel : WidgetViewModel { }
}