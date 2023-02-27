using UnityEngine;

namespace Lukomor.TagsGame.TagsGrid
{
    public interface ICell
    {
        int Id { get; }
        Vector2Int Position { get; set; }
        int Number { get; set; }
    }
}