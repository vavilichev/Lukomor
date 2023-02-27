using Lukomor.Common.Utils.Observables;
using Lukomor.TagsGame.TagsGrid;
using Lukomor.UI.Widgets;

namespace Lukomor.TagsGame.UI
{
	public class WidgetTagsGridViewModel : WidgetViewModel
	{
		public ObservableVariable<IGrid> GridData { get; } = new ObservableVariable<IGrid>();

		private IGridFeature gridFeature;

		protected override void OnConstructed()
		{
			base.OnConstructed();

			gridFeature = DiContainer.Get<IGridFeature>();
		}

		private void Start()
		{
			RefreshGridData();
		}
		
		public void RefreshGridData()
		{
			GridData.SetValue(gridFeature.GetGrid(), true);
		}
	}
}