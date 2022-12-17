using Lukomor.TagsGame.Grid.Data;

namespace Lukomor.TagsGame.Grid.Domain
{
	public class GetTagsGridDataInteractor : IGetTagsGridDataInteractor
	{
		private readonly TagsGridRepository _repository;
		
		private TagsGrid _cachedGrid;

		public GetTagsGridDataInteractor(TagsGridRepository repository)
		{
			_repository = repository;
		}

		public TagsGrid Execute()
		{
			if (_cachedGrid == null)
			{
				_cachedGrid = new TagsGrid(_repository.Get());
			}

			return _cachedGrid;
		}
	}
}