using Lukomor.Application.Signals;
using Lukomor.Example.Domain.TagsGrid;
using UnityEngine;

namespace Lukomor.Example.Application.TagsGrid.Signals
{
	public readonly struct CellMovedSignal : ISignal
	{
		public TagsCell ClickedCell { get; }
		public Vector2Int ClickedCellPosition { get; }
		public Vector2Int EmptyCellPosition { get; }
		public bool Success { get; }

		public CellMovedSignal(TagsCell clickedCell, Vector2Int clickedCellPosition, Vector2Int emptyCellPosition, bool success)
		{
			ClickedCell = clickedCell;
			ClickedCellPosition = clickedCellPosition;
			EmptyCellPosition = emptyCellPosition;
			Success = success;
		}
	}
}