using Lukomor.Common.DIContainer;
using Lukomor.Common.Utils.Observables;
using Lukomor.Presentation.Views.Widgets;
using Lukomor.TagsGame.TagsGrid;

namespace Lukomor.TagsGame.UI
{
	public class WidgetTagsCellViewModel : WidgetViewModel
	{
		public static class Keys
		{
			public static string PayloadTagCellKey = nameof(PayloadTagCellKey);
		}
		
		public ObservableVariable<ICell> Data { get; } = new ObservableVariable<ICell>();

		private readonly DIVar<IGridFeature> _gridFeature = new DIVar<IGridFeature>();

		protected override void PayloadsAdded()
		{
			base.PayloadsAdded();
			
			SetTagsCellFromPayload(Keys.PayloadTagCellKey);
		}
		
		private void SetTagsCellFromPayload(string payloadKey)
		{
			TryGetPayload(payloadKey, out ICell cell);

			if (cell != null)
			{
				Data.Value = cell;
				Data.ForceChangedEventInvoke();
			}
		}
		
		public void RequestMoveCell()
		{
			_gridFeature.Value.MoveCell(Data.Value);
		}
	}
}