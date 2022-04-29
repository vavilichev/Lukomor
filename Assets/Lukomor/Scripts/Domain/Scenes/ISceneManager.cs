using System;
using System.Threading.Tasks;

namespace Lukomor.Domain.Scenes
{
	public interface ISceneManager
	{
		event Action SceneLoading;
		event Action<bool> SceneLoaded;

		bool IsLoading { get; }

		Task LoadScene(int sceneIndex);
		Task LoadScene(string sceneName);
		Task ReloadScene();
	}
}