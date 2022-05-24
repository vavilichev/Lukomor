namespace Lukomor.Example.Domain.TagsGrid
{
	public class TagsGridRepository
	{
		public TagsGrid Grid { get; }

		public TagsGridRepository(TagsGrid grid)
		{
			Grid = grid;
		}
	}
}