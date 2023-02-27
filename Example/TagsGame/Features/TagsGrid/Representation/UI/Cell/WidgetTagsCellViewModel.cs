using Lukomor.Common.Utils.Observables;
using Lukomor.DI;
using Lukomor.TagsGame.TagsGrid;
using Lukomor.UI.Widgets;

namespace Lukomor.TagsGame.UI
{
	public class WidgetTagsCellViewModel : WidgetViewModel
	{
		public static class Keys
		{
			public static string PayloadTagCellKey = nameof(PayloadTagCellKey);
		}
		
		public ObservableVariable<ICell> Data { get; } = new ObservableVariable<ICell>();

		private IGridFeature gridFeature;

		protected override void OnConstructed()
		{
			gridFeature = DiContainer.Get<IGridFeature>();
		}

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
			gridFeature.MoveCell(Data.Value);
		}
	}
}