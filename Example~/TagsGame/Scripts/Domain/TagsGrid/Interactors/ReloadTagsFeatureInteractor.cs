using Lukomor.Application.Signals;
using Lukomor.DIContainer;
using Lukomor.Example.Application.TagsGrid.Signals;

namespace Lukomor.Example.Domain.TagsGrid.Interactors
{
	public class ReloadTagsFeatureInteractor : IReloadTagsFeatureInteractor
	{
		private readonly DIVar<ISignalTower> _signalTower = new DIVar<ISignalTower>();
		private readonly TagsGridRepository _repository;

		public ReloadTagsFeatureInteractor(TagsGridRepository repository)
		{
			_repository = repository;
		}

		public void Execute()
		{
			_signalTower.Value.FireSignal(new TagsGridRebuildStartSignal());
			
			_repository.Grid.Randomize();

			_signalTower.Value.FireSignal(new TagsGridRebuiltSignal());
		}
	}
}