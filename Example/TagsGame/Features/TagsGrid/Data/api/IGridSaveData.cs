using System.Collections.Generic;

namespace Lukomor.TagsGame.TagsGrid.Data
{
    public interface IGridSaveData
    {
        List<int> Cells { get; }
        int Size { get; }
    }
}