using Lukomor.Domain.Signals;

namespace Lukomor.TagsGame.TagsGrid.Signals
{
	public readonly struct CellMovedSignal : ISignal
	{
		public IGrid Grid { get; }
		public ICell ClickedCell { get; }
		public ICell EmptyCell { get; }
		public bool Success { get; }

		public CellMovedSignal(IGrid grid, ICell clickedCell, ICell emptyCell, bool success)
		{
			Grid = grid;
			ClickedCell = clickedCell;
			EmptyCell = emptyCell;
			Success = success;
		}
	}
}