using Lukomor.Application.Contexts;
using Lukomor.Example.Domain.TagsGrid;

namespace Lukomor.Example.Application.TagsGrid
{
	public static class TagsGridFeatureInstaller
	{
		public static TagsGridFeature InstallTagsGridFeature(this IContext context)
		{
			var feature = new TagsGridFeature();

			return feature;
		}
	}
}