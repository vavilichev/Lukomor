using Lukomor.DIContainer;
using Lukomor.Example.Domain.TagsGrid;
using Lukomor.Presentation.Controllers;

namespace Lukomor.Example.Presentation.Gameplay
{
	public class GameplayWindowController : Controller<GameplayWindowModel>
	{
		private readonly DIVar<TagsGridFeature> _tagsGridFeature = new DIVar<TagsGridFeature>();

		public override void Refresh(GameplayWindowModel model) { }

		public override void Subscribe(GameplayWindowModel model)
		{
			base.Subscribe(model);

			model.ReloadGrid.Requested += OnReloadGridRequested;
		}

		public override void Unsubscribe(GameplayWindowModel model)
		{
			base.Unsubscribe(model);

			model.ReloadGrid.Requested -= OnReloadGridRequested;
		}

		private void OnReloadGridRequested()
		{
			_tagsGridFeature.Value.ReloadFeature.Execute();
		}
	}
}