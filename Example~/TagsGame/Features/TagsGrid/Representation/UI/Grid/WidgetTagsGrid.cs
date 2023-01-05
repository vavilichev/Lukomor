using System;
using System.Linq;
using Lukomor.Common;
using Lukomor.Common.DIContainer;
using Lukomor.Domain.Signals;
using Lukomor.Presentation.Views.Widgets;
using Lukomor.TagsGame.TagsGrid;
using Lukomor.TagsGame.TagsGrid.Signals;
using UnityEngine;

namespace Lukomor.TagsGame.UI
{
	public class WidgetTagsGrid : Widget<WidgetTagsGridViewModel>, ITagsGridRebuiltSignalObserver, ICellMovedSignalObserver
	{
		#region Fields

		[SerializeField] private WidgetTagsCell[] _widgets;

		private readonly DIVar<ISignalTower> _signalTower = new DIVar<ISignalTower>();

		#endregion

		#region Unity lifecycle

		public override void Subscribe()
		{
			base.Subscribe();
			
			_signalTower.Value.Register<TagsGridRebuiltSignal>(this);
			_signalTower.Value.Register<CellMovedSignal>(this);
			
			ViewModel.GridData.Changed += OnGridDataChanged;
		}

		public override void Unsubscribe()
		{
			base.Unsubscribe();

			_signalTower.Value.Unregister<TagsGridRebuiltSignal>(this);
			_signalTower.Value.Unregister<CellMovedSignal>(this);
			
			ViewModel.GridData.Changed -= OnGridDataChanged;
		}

		#endregion

		#region Events and signals

		public void ReceiveSignal(TagsGridRebuiltSignal signal)
		{
			ViewModel.RefreshGridData();
		}
		
		public void ReceiveSignal(CellMovedSignal signal)
		{
			if (signal.Success)
			{
				RefreshGrid(signal.Grid);
			}
		}
		
		private void OnGridDataChanged(IGrid gridData)
		{
			RefreshGrid(gridData);
		}

		#endregion

		#region Methods

		private void RefreshGrid(IGrid grid)
		{
			if (_widgets.Length != grid.Cells.Length)
			{
				throw new Exception("Widgets amount must be equal grid cells amount");
			}

			var amount = _widgets.Length;

			for (int i = 0; i < amount; i++)
			{
				var x = i / grid.Size;
				var y = i % grid.Size;
				var cell = grid.Cells.First(c => c.Position.x == x && c.Position.y == y);//[i];
				var widget = _widgets[i];
				
				widget.ViewModel.AddPayloads(new Payload(WidgetTagsCellViewModel.Keys.PayloadTagCellKey, cell));
			}
		}

		#endregion
		
	}
}