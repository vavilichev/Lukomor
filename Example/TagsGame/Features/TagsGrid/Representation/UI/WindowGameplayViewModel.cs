using Lukomor.TagsGame.TagsGrid;
using Lukomor.UI.Views.Windows;

namespace Lukomor.TagsGame.UI
{
	public sealed class WindowGameplayViewModel : WindowViewModel
	{
		private IGridFeature gridFeature;
		
		protected override void OnConstructed()
		{
			base.OnConstructed();

			gridFeature = DiContainer.Get<IGridFeature>();
		}

		public void RequestReload()
		{
			gridFeature.ReloadGrid();
		}
	}
}