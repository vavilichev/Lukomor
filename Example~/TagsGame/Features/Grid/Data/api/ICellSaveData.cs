using UnityEngine;

namespace Lukomor.TagsGame.Grid.Data
{
    public interface ICellSaveData
    {
        int CellId { get; }
        Vector2Int Position { get; set; }
        int Number { get; set; }
    }
}