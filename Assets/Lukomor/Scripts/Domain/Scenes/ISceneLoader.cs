using System;
using System.Threading.Tasks;

namespace Lukomor.Domain.Scenes
{
	public struct SceneLoadingArgs
	{
		public string SceneName;
		public int SceneIndex;
		public bool Success;
	}

	public interface ISceneLoader
	{
		Task LoadScene(int sceneIndex, Action<SceneLoadingArgs> callback = null);
		Task LoadScene(string sceneName, Action<SceneLoadingArgs> callback = null);
		Task ReloadScene(Action<SceneLoadingArgs> callback = null);
	}
}