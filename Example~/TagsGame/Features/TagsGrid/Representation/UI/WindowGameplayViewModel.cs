using Lukomor.Common.DIContainer;
using Lukomor.Presentation.Views.Windows;
using Lukomor.TagsGame.TagsGrid;

namespace Lukomor.TagsGame.UI
{
	public sealed class WindowGameplayViewModel : WindowViewModel
	{
		private readonly DIVar<IGridFeature> _gridFeature = new DIVar<IGridFeature>();

		public void RequestReload()
		{
			_gridFeature.Value.ReloadGrid();
		}
	}
}