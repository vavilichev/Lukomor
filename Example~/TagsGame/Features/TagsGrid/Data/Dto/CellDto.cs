using Lukomor.TagsGame.TagsGrid.Data;
using Newtonsoft.Json;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Lukomor.TagsGame.TagsGrid.Dto
{
    [JsonObject(MemberSerialization.Fields)]
    public sealed class CellDto : ICellSaveData
    {
        [JsonProperty(PropertyName = "id")]
        public int CellId { get; private set; }

        public Vector2Int Position
        {
            get => myPosition;
            set
            {
                myPosition = value;
                position = new Vector2(value.x, value.y);
            }
        }

        [JsonProperty(PropertyName = "numb")]
        public int Number { get; set; }

        [JsonProperty(PropertyName = "pos")]
        private Vector2 position { get; set; }

        private Vector2Int myPosition;

        public CellDto(int id, int number, Vector2Int position)
        {
            CellId = id;
            Number = number;
            Position = position;
        }

        [JsonConstructor]
        public CellDto() { }
    }
}