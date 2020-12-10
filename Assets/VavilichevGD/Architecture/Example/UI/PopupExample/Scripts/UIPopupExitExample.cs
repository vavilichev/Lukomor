using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture.UI.Utils;

namespace VavilichevGD.Architecture.UI.Example {
    public class UIPopupExitExample : UIPopup {
        
        [Space]
        [SerializeField] private Button buttnoYes;

        private void OnEnable() {
            this.buttnoYes.AddListener(this.OnYesButtonClick);            
        }

        private void OnDisable() {
            this.buttnoYes.RemoveListener(this.OnYesButtonClick);
        }

        #region EVENTS

        private void OnYesButtonClick() {
            EditorApplication.isPlaying = false;
        }

        #endregion
    }
}