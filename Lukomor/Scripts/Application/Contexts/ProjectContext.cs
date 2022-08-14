using System.Collections.Generic;
using Lukomor.Application.Signals;
using Lukomor.DIContainer;

namespace Lukomor.Application.Contexts
{
	public abstract class ProjectContext : ContextBase
	{
		public IContext CurrentSceneContext { get; private set; }

		private Dictionary<string, IContext> _scenesContextMap;

		public ProjectContext(Dictionary<string, IContext> scenesContextMap)
		{
			DI.Bind<ISignalTower>(new SignalTower());
			
			_scenesContextMap = scenesContextMap;
		}

		public ProjectContext()
		{
			DI.Bind<ISignalTower>(new SignalTower());

			_scenesContextMap = new Dictionary<string, IContext>();
		}

		public IContext GetSceneContext(string sceneName)
		{
			_scenesContextMap.TryGetValue(sceneName, out var sceneContext);

			return sceneContext;
		}
	}
}