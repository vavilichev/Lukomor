using Lukomor.TagsGame.Grid.Data;
using UnityEngine;

namespace Lukomor.TagsGame.Grid.Domain
{
	public class TagsCell
	{
		public Vector2Int Position
		{
			get => _position;
			set
			{
				_position = value;
				_data.PosX = value.x;
				_data.PosY = value.y;
			}
		}

		public int Number
		{
			get => _data.Number;
			set => _data.Number = value;
		}

		private CellData _data;
		private Vector2Int _position;

		public TagsCell(CellData data)
		{
			_data = data;
			_position = new Vector2Int(data.PosX, data.PosY);
		}
	}
}