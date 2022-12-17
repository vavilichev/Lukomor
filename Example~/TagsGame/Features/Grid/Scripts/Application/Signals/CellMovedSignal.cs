using Lukomor.Domain.Signals;
using Lukomor.TagsGame.Grid.Domain;

namespace Lukomor.TagsGame.Grid.Application
{
	public readonly struct CellMovedSignal : ISignal
	{
		public TagsGrid Grid { get; }
		public TagsCell ClickedCell { get; }
		public TagsCell EmptyCell { get; }
		public bool Success { get; }

		public CellMovedSignal(TagsGrid grid, TagsCell clickedCell, TagsCell emptyCell, bool success)
		{
			Grid = grid;
			ClickedCell = clickedCell;
			EmptyCell = emptyCell;
			Success = success;
		}
	}
}