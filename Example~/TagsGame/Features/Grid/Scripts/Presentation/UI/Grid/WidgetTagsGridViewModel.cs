using Lukomor.Common.DIContainer;
using Lukomor.Common.Utils.Observables;
using Lukomor.Presentation.Views.Widgets;
using Lukomor.TagsGame.Grid.Domain;

namespace Lukomor.TagsGame.Grid.Presentation.UI.Grid
{
	public class WidgetTagsGridViewModel : WidgetViewModel
	{
		public ObservableVariable<TagsGrid> GridData { get; } = new ObservableVariable<TagsGrid>();

		private readonly DIVar<TagsGridFeature> _tagsGridFeature = new DIVar<TagsGridFeature>();

		public void RefreshGridData()
		{
			GridData.SetValue(_tagsGridFeature.Value.GetTagsGridData.Execute(), true);
		}
	}
}