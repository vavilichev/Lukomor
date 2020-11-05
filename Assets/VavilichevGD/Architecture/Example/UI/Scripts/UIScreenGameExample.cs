using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture.Extentions;
using VavilichevGD.Architecture.StorageSystem.Example;

namespace VavilichevGD.Architecture.Example {
	public class UIScreenGameExample : MonoBehaviour {

		public Text textLoaded;
		public InputField inputField;
		public Button buttonSave;

		private DummyRepository dummyRepository;

		private void Start() {
			Game.OnGameInitializedEvent += this.OnGameInitialized;
		}


		private void OnEnable() {
			this.buttonSave.onClick.AddListener(this.OnSaveButtonClick);
		}

		private void OnDisable() {
			this.buttonSave.onClick.RemoveListener(this.OnSaveButtonClick);
		}

		private void UpdateLoadedString() {
			var loadedText = this.dummyRepository.repoEntity.exampleString;
			this.textLoaded.text = loadedText;
		}


		#region EVENTS
		
		private void OnGameInitialized() {
			Game.OnGameInitializedEvent -= this.OnGameInitialized;

			this.dummyRepository = this.GetRepository<DummyRepository>();
			this.UpdateLoadedString();
		}

		private void OnSaveButtonClick() {
			var newText = this.inputField.text;
			var repoEntity = this.dummyRepository.repoEntity;
			repoEntity.exampleString = newText;
			
			Game.SaveGame();
			this.UpdateLoadedString();
		}

		#endregion

	}
}