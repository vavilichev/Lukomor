using System;
using System.Collections.Generic;

namespace Lukomor.TagsGame.Grid.Data
{
    [Serializable]
    public sealed class GridData
    {
        public List<CellData> Cells;
        public int Size;
    }
}