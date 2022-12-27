using Lukomor.TagsGame.Grid.Data;
using UnityEngine;

namespace Lukomor.TagsGame.Grid
{
	public class Cell : ICell
	{
		public int Id => _dto.CellId;
		public Vector2Int Position
		{
			get => _dto.Position;
			set => _dto.Position = value;
		}

		public int Number
		{
			get => _dto.Number;
			set => _dto.Number = value;
		}

		private ICellSaveData _dto;

		public Cell(ICellSaveData dto)
		{
			_dto = dto;
		}
	}
}