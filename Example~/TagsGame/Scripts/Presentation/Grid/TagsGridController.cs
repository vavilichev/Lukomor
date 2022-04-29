using Lukomor.Application.Signals;
using Lukomor.DIContainer;
using Lukomor.Example.Application.TagsGrid.Signals;
using Lukomor.Example.Domain.TagsGrid;
using Lukomor.Example.Presentation.Cell;
using Lukomor.Presentation.Controllers;
using UnityEngine;

namespace Lukomor.Example.Presentation.Grid {
	public class TagsGridController : Controller<TagsGridModel>, ICellMovedSignalObserver, ITagsGridRebuiltSignalObserver
	{
		private readonly TagsCellWidget[] _nonOrderedCellWidgets;
		private TagsCellWidget[,] _cellWidgets;
		private Vector3[] _widgetsDefaultPositions;

		private readonly DIVar<ISignalTower> _signalTower = new DIVar<ISignalTower>();
		private readonly DIVar<TagsGridFeature> _tagsGridFeature = new DIVar<TagsGridFeature>();
		
		public TagsGridController(TagsCellWidget[] cellWidgets)
		{
			_nonOrderedCellWidgets = cellWidgets;

			CacheStartWidgetPositions(_nonOrderedCellWidgets);
		}

		public override void Subscribe(TagsGridModel model)
		{
			base.Subscribe(model);

			_signalTower.Value.Register<TagsGridRebuiltSignal>(this);
			_signalTower.Value.Register<CellMovedSignal>(this);
		}
		
		public override void Refresh(TagsGridModel model)
		{
			RefreshGrid();
		}

		public override void Unsubscribe(TagsGridModel model)
		{
			base.Unsubscribe(model);

			_signalTower.Value.Unregister<TagsGridRebuiltSignal>(this);
			_signalTower.Value.Unregister<CellMovedSignal>(this);
		}
		
		public void ReceiveSignal(TagsGridRebuiltSignal signal)
		{
			RefreshGrid();
		}
		
		public void ReceiveSignal(CellMovedSignal signal)
		{
			if (signal.Success)
			{
				var clickedCellWidget = _cellWidgets[signal.ClickedCellPosition.x, signal.ClickedCellPosition.y];
				var emptyCellWidget = _cellWidgets[signal.EmptyCellPosition.x, signal.EmptyCellPosition.y];
			
				SwitchWidgetsTransformPositions(clickedCellWidget, emptyCellWidget);
				SwitchWidgetsGridPositions(clickedCellWidget, emptyCellWidget, signal.ClickedCellPosition, signal.EmptyCellPosition);
			}
		}

		private void CacheStartWidgetPositions(TagsCellWidget[] cellWidgets)
		{
			var widgetsCount = cellWidgets.Length;

			_widgetsDefaultPositions = new Vector3[widgetsCount];

			for (int i = 0; i < widgetsCount; i++)
			{
				_widgetsDefaultPositions[i] = cellWidgets[i].transform.position;
			}
		}

		private void RefreshGrid()
		{
			Model.GridData.Value = _tagsGridFeature.Value.GetTagsGridData.Execute();

			var cellsData = Model.GridData.Value.CellsData;
			var gridSize = Model.GridData.Value.Size;
			
			_cellWidgets = new TagsCellWidget[gridSize.x, gridSize.y];

			for (int i = 0; i < gridSize.x; i++)
			{
				for (int j = 0; j < gridSize.y; j++)
				{
					var nonOrderedWidgetIndex = i * gridSize.x + j;
					var cellWidget = _nonOrderedCellWidgets[nonOrderedWidgetIndex];

					_cellWidgets[i, j] = cellWidget;
					cellWidget.AddPayload(TagsCellWidget.PayloadKeys.TagCellKey, cellsData[i, j]);
					cellWidget.transform.position = _widgetsDefaultPositions[nonOrderedWidgetIndex];
				}
			}
		}

		private void SwitchWidgetsTransformPositions(TagsCellWidget clickedCellWidget, TagsCellWidget emptyCellWidget)
		{
			var clickedCellWidgetTransform = clickedCellWidget.transform;
			var emptyCellWidgetTransform = emptyCellWidget.transform;
			var tempPosition = clickedCellWidgetTransform.position;

			clickedCellWidgetTransform.position = emptyCellWidgetTransform.position;
			emptyCellWidgetTransform.position = tempPosition;
		}

		private void SwitchWidgetsGridPositions(
			TagsCellWidget clickedCellWidget,
			TagsCellWidget emptyCellWidget,
			Vector2Int clickedCellPosition,
			Vector2Int emptyCellPosition)
		{
			_cellWidgets[clickedCellPosition.x, clickedCellPosition.y] = emptyCellWidget;
			_cellWidgets[emptyCellPosition.x, emptyCellPosition.y] = clickedCellWidget;
		}
	}
}