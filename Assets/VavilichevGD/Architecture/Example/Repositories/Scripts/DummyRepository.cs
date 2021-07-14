using VavilichevGD.Architecture.StorageSystem;

namespace VavilichevGD.Architecture.Example {
	public sealed class DummyRepository : Repository {

		#region CONSTANTS

		private const string KEY_EXAMPLE_TEXT = "EXAMPLE_TEXT_KEY";

		#endregion
		
		private Storage _sceneFileStorage;

		public string text {
			get => _sceneFileStorage.Get(KEY_EXAMPLE_TEXT, "EMPTY");
			set => _sceneFileStorage.Set(KEY_EXAMPLE_TEXT, value);
		}

		public override void OnInitialize() {
			_sceneFileStorage = Game.sceneManager.sceneActual.fileStorage;
		}
	}
}