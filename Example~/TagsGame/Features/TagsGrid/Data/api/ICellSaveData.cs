using UnityEngine;

namespace Lukomor.TagsGame.TagsGrid.Data
{
    public interface ICellSaveData
    {
        int CellId { get; }
        Vector2Int Position { get; set; }
        int Number { get; set; }
    }
}