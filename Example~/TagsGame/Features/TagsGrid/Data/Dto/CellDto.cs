using Lukomor.TagsGame.TagsGrid.Data;
using Newtonsoft.Json;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Lukomor.TagsGame.TagsGrid.Dto
{
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class CellDto : ICellSaveData, ISerializationCallbackReceiver
    {
        [JsonProperty(PropertyName = "id")]
        public int CellId { get; private set; }
        public Vector2Int Position { get; set; }
        [JsonProperty(PropertyName = "numb")]
        public int Number { get; set; }

        [JsonProperty(PropertyName = "pos")]
        private Vector2 position;

        public CellDto(int id, int number, Vector2Int position)
        {
            CellId = id;
            Number = number;
            Position = position;
        }

        [JsonConstructor]
        public CellDto() { }

        public void OnBeforeSerialize()
        {
            position = new Vector2(Position.x, Position.y);
        }

        public void OnAfterDeserialize()
        {
            Position = new Vector2Int((int) position.X, (int) position.Y);
        }
    }
}