using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture.Example;
using VavilichevGD.Architecture.Extentions;

namespace VavilichevGD.Architecture.UserInterface.Example {
    public class UIWidgetInfoGameExample : UIWidget {
        
        [SerializeField] private Text textLoaded;
        [SerializeField] private InputField inputField;
        [SerializeField] private Button buttonSave;

        private DummyRepository _dummyRepository;

        private void Awake() {
            this._dummyRepository = this.GetRepository<DummyRepository>();
        }

        private void OnEnable() {
            this.buttonSave.onClick.AddListener(this.OnSaveButtonClick);
        }

        private void OnDisable() {
            this.buttonSave.onClick.RemoveListener(this.OnSaveButtonClick);
        }

        private void UpdateLoadedString() {
            textLoaded.text = _dummyRepository.text;
        }

        public void OnStart() {
            this.UpdateLoadedString();
        }


        #region EVENTS

        private void OnSaveButtonClick() {
            var newText = this.inputField.text;
            _dummyRepository.text = newText;

            Game.SaveGame();
            this.UpdateLoadedString();
        }

        #endregion
    }
}
