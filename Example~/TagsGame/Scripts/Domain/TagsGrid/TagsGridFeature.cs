using System.Threading.Tasks;
using Lukomor.Application.Features;
using Lukomor.Example.Domain.TagsGrid.Interactors;

namespace Lukomor.Example.Domain.TagsGrid
{
	public class TagsGridFeature : Feature
	{
		public IGetTagsGridDataInteractor GetTagsGridData { get; private set; }
		public IMoveCellInteractor MoveCell { get; private set; }
		public IReloadTagsFeatureInteractor ReloadFeature { get; private set; }

		private TagsGridRepository _repository;

		protected override Task InitializeInternal()
		{
			var gridData = new TagsGrid(4);
			gridData.Randomize();
			
			_repository = new TagsGridRepository(gridData);

			GetTagsGridData = new GetTagsGridDataInteractor(_repository);
			MoveCell = new MoveCellInteractor(_repository);
			ReloadFeature = new ReloadTagsFeatureInteractor(_repository);
			
			return Task.CompletedTask;
		}
	}
}