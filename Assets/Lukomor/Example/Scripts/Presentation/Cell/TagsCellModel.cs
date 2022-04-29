using Lukomor.Example.Domain.TagsGrid;
using Lukomor.Presentation.Models;
using VavilichevGD.Utils.Observables;

namespace Lukomor.Example.Presentation.Cell
{
	public class TagsCellModel : Model
	{
		public ObservableVariable<TagsCell> Data { get; } = new ObservableVariable<TagsCell>();
		public ObservableCommand<TagsCell> MoveCell { get; } = new ObservableCommand<TagsCell>();
	}
}