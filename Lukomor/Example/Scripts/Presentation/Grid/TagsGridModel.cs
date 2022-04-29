using Lukomor.Example.Domain.TagsGrid;
using Lukomor.Presentation.Models;
using VavilichevGD.Utils.Observables;

namespace Lukomor.Example.Presentation.Grid {
	public class TagsGridModel : Model {
		public ObservableVariable<TagsGrid> GridData { get; } = new ObservableVariable<TagsGrid>();
	}
}