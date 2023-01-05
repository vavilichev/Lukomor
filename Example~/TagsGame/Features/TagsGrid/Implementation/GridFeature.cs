using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lukomor.Common.DIContainer;
using Lukomor.Domain.Features;
using Lukomor.Domain.Signals;
using Lukomor.TagsGame.TagsGrid.Data;
using Lukomor.TagsGame.TagsGrid.Signals;

namespace Lukomor.TagsGame.TagsGrid
{
	public class GridFeature : Feature, IGridFeature
	{
		private readonly ISignalTower _signalTower;
		private readonly IGridRepository _repository;
		private IGrid _cachedGrid;

		public GridFeature(IGridRepository repository)
		{
			_repository = repository;

			_signalTower = DI.Get<ISignalTower>();
		}

		public IGrid GetGrid()
		{
			if (_cachedGrid == null)
			{
				_cachedGrid = CreateGrid();
			}

			return _cachedGrid;
		}

		public void MoveCell(ICell clickedCell)
		{
			var allCells = _cachedGrid.Cells;
			var success = false;
			
			ICell emptyCell = allCells.FirstOrDefault(c => c.Number == 0);

			if (UnityEngine.Mathf.Abs(emptyCell.Position.x - clickedCell.Position.x) <= 1 && emptyCell.Position.y == clickedCell.Position.y
			    || UnityEngine.Mathf.Abs(emptyCell.Position.y - clickedCell.Position.y) <= 1 && emptyCell.Position.x == clickedCell.Position.x)
			{
				success = true;
				
				var emptyCellPosition = emptyCell.Position;
				emptyCell.Position = clickedCell.Position;
				clickedCell.Position = emptyCellPosition;
				
				Save();
			}

			var signal = new CellMovedSignal(_cachedGrid, clickedCell, emptyCell, success);

			_signalTower.FireSignal(signal);
		}

		public void ReloadGrid()
		{
			_signalTower.FireSignal(new TagsGridRebuildStartSignal());

			_cachedGrid.Randomize();
			
			Save();

			_signalTower.FireSignal(new TagsGridRebuiltSignal());
		}

		private void Save()
		{
			_repository.Save();
		}

		protected override async Task InitializeInternal()
		{
			await _repository.Load();
		}

		private IGrid CreateGrid()
		{
			var gridSaveData = _repository.GetGrid();

			if (gridSaveData == null)
			{
				gridSaveData = _repository.Create();
			}
			
			var cells = new List<ICell>();

			foreach (int cellId in gridSaveData.Cells)
			{
				var cellSaveData = _repository.GetCell(cellId);
				
				cells.Add(CreateCell(cellSaveData));
			}

			var createdGrid = new GameGrid(gridSaveData, cells.ToArray());

			return createdGrid;
		}

		private ICell CreateCell(ICellSaveData saveData)
		{
			var createdCell = new Cell(saveData);

			return createdCell;
		}
	}
}