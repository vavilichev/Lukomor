using Lukomor.Application.Signals;
using Lukomor.DIContainer;
using Lukomor.Example.Application.TagsGrid.Signals;

namespace Lukomor.Example.Domain.TagsGrid.Interactors
{
	public class ReloadTagsFeatureInteractor : IReloadTagsFeatureInteractor
	{
		private readonly DIVar<ISignalTower> _signalTower = new DIVar<ISignalTower>();
		private readonly TagsGridFeatureModel _model;

		public ReloadTagsFeatureInteractor(TagsGridFeatureModel model)
		{
			_model = model;
		}

		public void Execute()
		{
			_signalTower.Value.FireSignal(new TagsGridRebuildStartSignal());
			
			_model.Grid.Randomize();

			_signalTower.Value.FireSignal(new TagsGridRebuiltSignal());
		}
	}
}