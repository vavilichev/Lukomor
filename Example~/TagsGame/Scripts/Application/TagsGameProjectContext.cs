using System.Collections.Generic;
using Lukomor.Application.Contexts;
using Lukomor.Application.Signals;
using Lukomor.DIContainer;

namespace Lukomor.Example.Application
{
	public sealed class TagsGameProjectContext : ProjectContext
	{
		public TagsGameProjectContext()
		{
		}

		public TagsGameProjectContext(Dictionary<string, IContext> scenesContextMap) : base(scenesContextMap) { }

		protected override void InstallServices() { }

		protected override void InstallFeatures() { }
	}
}