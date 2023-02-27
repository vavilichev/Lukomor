using System;
using System.Threading.Tasks;

namespace Lukomor.Scenes
{
	public interface ISceneManager
	{
		event Action SceneLoadingStarted;
		event Action<bool> SceneLoaded;

		bool IsLoading { get; }

		Task LoadScene(int sceneIndex);
		Task LoadScene(string sceneName);
		Task ReloadScene();
	}
}