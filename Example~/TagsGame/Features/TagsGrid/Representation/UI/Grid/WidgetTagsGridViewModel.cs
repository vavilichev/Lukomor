using Lukomor.Common.DIContainer;
using Lukomor.Common.Utils.Observables;
using Lukomor.Presentation.Views.Widgets;
using Lukomor.TagsGame.TagsGrid;

namespace Lukomor.TagsGame.UI
{
	public class WidgetTagsGridViewModel : WidgetViewModel
	{
		public ObservableVariable<IGrid> GridData { get; } = new ObservableVariable<IGrid>();

		private readonly DIVar<IGridFeature> _gridFeature = new DIVar<IGridFeature>();

		private void Start()
		{
			RefreshGridData();
		}
		
		public void RefreshGridData()
		{
			GridData.SetValue(_gridFeature.Value.GetGrid(), true);
		}
	}
}