using Lukomor.Data;

namespace Lukomor.TagsGame.Grid.Data
{
    public interface IGridRepository : IRepository
    {
        IGridSaveData Create();
        IGridSaveData GetGrid();
        ICellSaveData GetCell(int cellId);
    }
}