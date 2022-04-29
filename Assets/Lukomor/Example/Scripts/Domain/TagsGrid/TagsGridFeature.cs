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

		private TagsGridFeatureModel _model;

		protected override Task InitializeInternal()
		{
			var gridData = new Lukomor.Example.Domain.TagsGrid.TagsGrid(4);
			gridData.Randomize();
			
			_model = new TagsGridFeatureModel(gridData);

			GetTagsGridData = new GetTagsGridDataInteractor(_model);
			MoveCell = new MoveCellInteractor(_model);
			ReloadFeature = new ReloadTagsFeatureInteractor(_model);
			
			return Task.CompletedTask;
		}
	}
}