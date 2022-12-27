using System.Collections.Generic;

namespace Lukomor.TagsGame.Grid.Data
{
    public interface IGridSaveData
    {
        List<int> Cells { get; }
        int Size { get; }
    }
}