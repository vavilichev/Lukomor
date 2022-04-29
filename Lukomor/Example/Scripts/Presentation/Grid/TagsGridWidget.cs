using Lukomor.Example.Presentation.Cell;
using Lukomor.Presentation.Controllers;
using Lukomor.Presentation.Views.Widgets;
using UnityEngine;

namespace Lukomor.Example.Presentation.Grid
{
	public class TagsGridWidget : Widget<TagsGridModel>
	{
		[SerializeField] private TagsCellWidget[] _widgets;

		protected override Controller<TagsGridModel> CreateController()
		{
			return new TagsGridController(_widgets);
		}
	}
}