using System.Linq;
using Lukomor.Common.DIContainer;
using Lukomor.Domain.Signals;
using Lukomor.TagsGame.Grid.Application;
using Lukomor.TagsGame.Grid.Data;
using UnityEngine;

namespace Lukomor.TagsGame.Grid.Domain
{
	public class MoveCellInteractor : IMoveCellInteractor
	{
		private readonly DIVar<ISignalTower> _signalTower = new DIVar<ISignalTower>();
		private readonly TagsGridRepository _repository;
		private readonly IGetTagsGridDataInteractor _getTagsGridData;
		private readonly TagsGridFeature _feature;

		public MoveCellInteractor(TagsGridFeature feature, IGetTagsGridDataInteractor getTagsGridData)
		{
			_feature = feature;
			_getTagsGridData = getTagsGridData;
		}

		public void Execute(TagsCell clickedCell)
		{
			var grid = _getTagsGridData.Execute();
			var allCells = grid.Cells;
			var success = false;
			
			TagsCell emptyCell = allCells.FirstOrDefault(c => c.Number == 0);

			if (Mathf.Abs(emptyCell.Position.x - clickedCell.Position.x) <= 1 && emptyCell.Position.y == clickedCell.Position.y
			    || Mathf.Abs(emptyCell.Position.y - clickedCell.Position.y) <= 1 && emptyCell.Position.x == clickedCell.Position.x)
			{
				success = true;
				
				var emptyCellPosition = emptyCell.Position;
				emptyCell.Position = clickedCell.Position;
				clickedCell.Position = emptyCellPosition;
				
				_feature.Save();
			}

			var signal = new CellMovedSignal(grid, clickedCell, emptyCell, success);

			_signalTower.Value.FireSignal(signal);
		}
		
	}
}