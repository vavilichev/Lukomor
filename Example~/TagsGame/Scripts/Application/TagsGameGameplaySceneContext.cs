using Lukomor.Application.Contexts;
using Lukomor.Example.Application.TagsGrid;

namespace Lukomor.Example.Application
{
	public class TagsGameGameplaySceneContext : ContextBase
	{
		protected override void InstallServices() { }

		protected override void InstallFeatures()
		{
			AddFeature(this.InstallTagsGridFeature());
		}
	}
}