using Lukomor.DIContainer;
using Lukomor.Example.Domain.TagsGrid;
using Lukomor.Presentation.Controllers;

namespace Lukomor.Example.Presentation.Cell
{
	public class TagsCellController : Controller<TagsCellModel>
	{
		private readonly DIVar<TagsGridFeature> _tagsGridFeature = new DIVar<TagsGridFeature>();

		public override void Refresh(TagsCellModel model) { }

		public override void Subscribe(TagsCellModel model)
		{
			base.Subscribe(model);
			
			model.MoveCell.Requested += OnMoveCellOnRequested;
		}
		
		public override void Unsubscribe(TagsCellModel model)
		{
			base.Unsubscribe(model);
			
			model.MoveCell.Requested -= OnMoveCellOnRequested;
		}

		private void OnMoveCellOnRequested(TagsCell cell)
		{
			_tagsGridFeature.Value.MoveCell.Execute(cell);
		}
	}
}