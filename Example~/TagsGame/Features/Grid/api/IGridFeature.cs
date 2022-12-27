using Lukomor.Domain.Features;

namespace Lukomor.TagsGame.Grid
{
    public interface IGridFeature : IFeature
    {
        IGrid GetGrid();
        void MoveCell(ICell clickedCell);
        void ReloadGrid();
    }
}
