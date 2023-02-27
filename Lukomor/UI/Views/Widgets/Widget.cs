namespace Lukomor.UI {
	public abstract class Widget<TWidgetViewModel> : View<TWidgetViewModel>, IWidget<TWidgetViewModel>
		where TWidgetViewModel : WidgetViewModel { }
}