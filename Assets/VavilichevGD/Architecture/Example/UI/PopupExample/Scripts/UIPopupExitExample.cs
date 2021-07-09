using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture.UserInterface.Utils;

namespace VavilichevGD.Architecture.UserInterface.Example {
    public class UIPopupExitExample : UIPopup {
        
        [Space]
        [SerializeField] private Button buttonYes;
        [SerializeField] private Button buttonNo;

        private void OnEnable() {
            this.buttonYes.AddListener(this.OnYesButtonClick);    
            this.buttonNo.AddListener(this.OnNoButtonClick);
        }

        private void OnDisable() {
            this.buttonYes.RemoveListener(this.OnYesButtonClick);
            this.buttonNo.RemoveListener(this.OnNoButtonClick);
        }

        #region EVENTS

        private void OnYesButtonClick() {
            EditorApplication.isPlaying = false;
        }

        private void OnNoButtonClick() {
            this.Hide();
        }

        #endregion
    }
}