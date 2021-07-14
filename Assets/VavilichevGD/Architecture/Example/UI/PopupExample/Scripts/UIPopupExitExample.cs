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
            buttonYes.AddListener(OnYesButtonClick);    
            buttonNo.AddListener(OnNoButtonClick);
        }

        private void OnDisable() {
            buttonYes.RemoveListener(OnYesButtonClick);
            buttonNo.RemoveListener(OnNoButtonClick);
        }

        #region EVENTS

        private void OnYesButtonClick() {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void OnNoButtonClick() {
            Hide();
        }

        #endregion
    }
}