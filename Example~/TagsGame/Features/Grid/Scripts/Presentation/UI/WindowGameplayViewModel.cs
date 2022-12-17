using Lukomor.Common.DIContainer;
using Lukomor.Presentation.Views.Windows;
using Lukomor.TagsGame.Grid.Domain;

namespace Lukomor.TagsGame.Grid.Presentation.UI
{
	public sealed class WindowGameplayViewModel : WindowViewModel
	{
		private readonly DIVar<TagsGridFeature> _tagsGridFeature = new DIVar<TagsGridFeature>();

		public void RequestReload()
		{
			_tagsGridFeature.Value.ReloadFeature.Execute();
		}
	}
}