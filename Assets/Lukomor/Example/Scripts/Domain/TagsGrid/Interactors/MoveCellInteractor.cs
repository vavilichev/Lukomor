using Lukomor.Application.Signals;
using Lukomor.DIContainer;
using Lukomor.Example.Application.TagsGrid.Signals;
using UnityEngine;
using VavilichevGD.Extensions;

namespace Lukomor.Example.Domain.TagsGrid.Interactors
{
	public class MoveCellInteractor : IMoveCellInteractor
	{
		private readonly DIVar<ISignalTower> _signalTower = new DIVar<ISignalTower>();
		private readonly TagsGridFeatureModel _model;

		public MoveCellInteractor(TagsGridFeatureModel model)
		{
			_model = model;
		}

		public void Execute(TagsCell clickedCell)
		{
			var cellsData = _model.Grid.CellsData;
			var rowsCount = cellsData.GetLength(0);
			var columnsCount = cellsData.GetLength(1);
			var clickedCellPosition = cellsData.GetCoordinates(clickedCell);
			var clickedY = clickedCellPosition.y;
			var clickedX = clickedCellPosition.x;
			var emptyCellX = -1;
			var emptyCellY = -1;
			var emptyCellDataFound = false;
			var success = false;
			
			TagsCell emptyCell = null;

			if (clickedX > 0)
			{
				emptyCellX = clickedX - 1;
				emptyCellY = clickedY;
				emptyCell = cellsData[emptyCellX, emptyCellY];
				emptyCellDataFound = emptyCell.Number == 0;
			}

			if (!emptyCellDataFound && clickedX < rowsCount - 1)
			{
				emptyCellX = clickedX + 1;
				emptyCellY = clickedY;
				emptyCell = cellsData[emptyCellX, emptyCellY];
				emptyCellDataFound = emptyCell.Number == 0;
			}

			if (!emptyCellDataFound && clickedY > 0)
			{
				emptyCellX = clickedX;
				emptyCellY = clickedY - 1;
				emptyCell = cellsData[emptyCellX, emptyCellY];
				emptyCellDataFound = emptyCell.Number == 0;
			}

			if (!emptyCellDataFound && clickedY < columnsCount - 1)
			{
				emptyCellX = clickedX;
				emptyCellY = clickedY + 1;
				emptyCell = cellsData[emptyCellX, emptyCellY];
				emptyCellDataFound = emptyCell.Number == 0;
			}

			if (emptyCellDataFound)
			{
				cellsData[clickedX, clickedY] = emptyCell;
				cellsData[emptyCellX, emptyCellY] = clickedCell;
				success = true;
			}

			var emptyCellPosition = new Vector2Int(emptyCellX, emptyCellY);
			var signal = new CellMovedSignal(clickedCell, clickedCellPosition, emptyCellPosition, success);

			_signalTower.Value.FireSignal(signal);
		}
	}
}