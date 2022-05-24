namespace Lukomor.Example.Domain.TagsGrid.Interactors
{
	public class GetTagsGridDataInteractor : IGetTagsGridDataInteractor
	{
		private readonly TagsGridRepository _repository;

		public GetTagsGridDataInteractor(TagsGridRepository repository)
		{
			_repository = repository;
		}

		public TagsGrid Execute()
		{
			return _repository.Grid;
		}
	}
}