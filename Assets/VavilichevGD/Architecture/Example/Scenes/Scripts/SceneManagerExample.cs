using VavilichevGD.Architecture.Scenes;

namespace VavilichevGD.Architecture.Example {
	public sealed class SceneManagerExample : SceneManagerBase{
		protected override void InitializeSceneConfigs() {
			this.scenesConfigMap[SceneConfigExample.SCENE_NAME] = new SceneConfigExample();
		}
	}
}