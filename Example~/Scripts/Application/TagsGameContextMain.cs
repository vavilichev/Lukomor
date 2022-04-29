using Lukomor.Application.Contexts;
using Lukomor.DIContainer;
using Lukomor.Example.Application.TagsGrid;

namespace Lukomor.Example.Application {
	public sealed class TagsGameContextMain : ContextBase {
		protected override void InstallServices() { }

		protected override void InstallFeatures() {
			DI.Bind(this.InstallTagsGridFeature());
		}
	}
}