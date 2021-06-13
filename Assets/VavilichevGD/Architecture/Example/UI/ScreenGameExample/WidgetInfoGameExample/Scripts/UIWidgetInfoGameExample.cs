using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture.Example;
using VavilichevGD.Architecture.Extentions;

namespace VavilichevGD.Architecture.UI.Example {
    public class UIWidgetInfoGameExample : UIWidget {
        
        [SerializeField] private Text textLoaded;
        [SerializeField] private InputField inputField;
        [SerializeField] private Button buttonSave;

        private DummyRepository dummyRepository;

        private void Awake() {
            this.dummyRepository = this.GetRepository<DummyRepository>();
        }

        private void OnEnable() {
            this.buttonSave.onClick.AddListener(this.OnSaveButtonClick);
            this.UpdateLoadedString();
        }

        private void OnDisable() {
            this.buttonSave.onClick.RemoveListener(this.OnSaveButtonClick);
        }

        private void UpdateLoadedString() {
            // var loadedText = this.dummyRepository.repoEntity.exampleString;
            // this.textLoaded.text = loadedText;
        }


        #region EVENTS

        private void OnSaveButtonClick() {
            var newText = this.inputField.text;
            // var repoEntity = this.dummyRepository.repoEntity;
            // repoEntity.exampleString = newText;

            Game.SaveGame();
            this.UpdateLoadedString();
        }

        #endregion
    }
}
