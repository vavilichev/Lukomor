using Lukomor.Presentation.Views.Windows;

namespace Lukomor.Common.Scenes
{
	public interface ISceneConfig
	{
		string SceneName { get; }
		IWindow[] WindowPrefabs { get; }
	}
}