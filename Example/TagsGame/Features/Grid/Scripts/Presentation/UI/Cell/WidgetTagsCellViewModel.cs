using Lukomor.Common.DIContainer;
using Lukomor.Common.Utils.Observables;
using Lukomor.Presentation.Views.Widgets;
using Lukomor.TagsGame.Grid.Domain;

namespace Lukomor.TagsGame.Grid.Presentation.UI
{
	public class WidgetTagsCellViewModel : WidgetViewModel
	{
		public static class Keys
		{
			public static string PayloadTagCellKey = nameof(PayloadTagCellKey);
		}
		
		public ObservableVariable<TagsCell> Data { get; } = new ObservableVariable<TagsCell>();

		private readonly DIVar<TagsGridFeature> _tagsGridFeature = new DIVar<TagsGridFeature>();

		protected override void PayloadsAdded()
		{
			base.PayloadsAdded();
			
			SetTagsCellFromPayload(Keys.PayloadTagCellKey);
		}
		
		private void SetTagsCellFromPayload(string payloadKey)
		{
			TryGetPayload(payloadKey, out TagsCell cell);

			if (cell != null)
			{
				Data.Value = cell;
				Data.ForceChangedEventInvoke();
			}
		}
		
		public void RequestMoveCell()
		{
			_tagsGridFeature.Value.MoveCell.Execute(Data.Value);
		}
	}
}