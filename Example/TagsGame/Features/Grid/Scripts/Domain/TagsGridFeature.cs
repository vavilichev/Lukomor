using System.Threading.Tasks;
using Lukomor.Domain.Features;
using Lukomor.TagsGame.Grid.Data;

namespace Lukomor.TagsGame.Grid.Domain
{
	public class TagsGridFeature : Feature
	{
		public IGetTagsGridDataInteractor GetTagsGridData { get; private set; }
		public IMoveCellInteractor MoveCell { get; private set; }
		public IReloadTagsFeatureInteractor ReloadFeature { get; private set; }

		private readonly TagsGridRepository _repository;

		public TagsGridFeature()
		{
			_repository = new TagsGridRepository();
		}

		public void Save()
		{
			_repository.Save();
		}

		protected override Task InitializeInternal()
		{
			_repository.Load();
			
			GetTagsGridData = new GetTagsGridDataInteractor(_repository);
			MoveCell = new MoveCellInteractor(this, GetTagsGridData);
			ReloadFeature = new ReloadTagsFeatureInteractor(this, GetTagsGridData);
			
			return Task.CompletedTask;
		}
	}
}