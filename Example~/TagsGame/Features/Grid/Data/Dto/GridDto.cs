using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lukomor.TagsGame.Grid.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class GridDto : IGridSaveData
    {
        [JsonProperty(PropertyName = "cells")]
        public List<int> Cells { get; private set; }
        [JsonProperty(PropertyName = "size")]
        public int Size { get; private set; }

        public GridDto(int size, int[] cellIds)
        {
            Size = size;
            Cells = new List<int>(cellIds);
        }

        [JsonConstructor]
        public GridDto() { }
    }
}