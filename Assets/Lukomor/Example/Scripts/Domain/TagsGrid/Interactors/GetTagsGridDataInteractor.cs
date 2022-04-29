namespace Lukomor.Example.Domain.TagsGrid.Interactors
{
	public class GetTagsGridDataInteractor : IGetTagsGridDataInteractor
	{
		private readonly TagsGridFeatureModel _model;

		public GetTagsGridDataInteractor(TagsGridFeatureModel model)
		{
			_model = model;
		}

		public TagsGrid Execute()
		{
			return _model.Grid;
		}
	}
}