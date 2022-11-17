namespace Lukomor.Presentation.Views.Widgets {
	public abstract class Widget<TWidgetViewModel> : View<TWidgetViewModel>, IWidget<TWidgetViewModel>
		where TWidgetViewModel : WidgetViewModel { }
}