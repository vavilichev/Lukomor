using UnityEngine.Events;
using VavilichevGD.Core.Loadging;

namespace VavilichevGD.Architecture.Example {
	public sealed class GameExample : Game{
		
		public static void Run() {
			bool singletonCreated = CreateSingleton();
			if (singletonCreated) {
				OnGameInitializedEvent += OnGameInitialized;
				instance.Initialize();
			}
		}

		private static bool CreateSingleton() {
			if (instance != null)
				return false;
            
			instance = new GameExample();
			return true;
		}

		#region Events

		private static void OnGameInitialized() {
			OnGameInitializedEvent -= OnGameInitialized;
			LoadingScreen.Hide();
		}

		#endregion

		protected override SceneManagerBase CreateSceneManager() {
			return new SceneManagerExample();
		}

		protected override void LoadFirstScene(UnityAction<ISceneConfig> callback) {
			LoadingScreen.Show();
			sceneManager.InitializeCurrentScene(callback);
		}
	}
}