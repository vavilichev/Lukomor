using Lukomor.Domain.Features;

namespace Lukomor.TagsGame.TagsGrid
{
    public interface IGridFeature : IFeature
    {
        IGrid GetGrid();
        void MoveCell(ICell clickedCell);
        void ReloadGrid();
    }
}
