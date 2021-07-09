using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VavilichevGD.Core.Loadging {
    public class LoadingScreen : MonoBehaviour{

        #region CONSTANTS

        private const string PREF_PATH = "[LOADING_SCREEN]";

        #endregion

        
        #region EVENTS


        public event Action<object, LoadingScreen> OnLoadingScreenShownEvent;
        public event Action<object, LoadingScreen> OnLoadingScreenHideStartEvent;
        public event Action<object, LoadingScreen> OnLoadingScreenHiddenCompletelyEvent;

        #endregion

        public static LoadingScreen instance {
            get {
                if (_instance == null) {
                    var prefab = Resources.Load<LoadingScreen>(PREF_PATH);
                    _instance = Instantiate(prefab);
                    DontDestroyOnLoad(_instance.gameObject);
                }

                return _instance;
            }
        }

        private static LoadingScreen _instance;

        
        
        
        
        public static bool isActive => visual != null && visual.isActive;

        private static LoadingScreenVisualBase visual;
        
        
        public void Show(object sender) {
            // if (visual == null)
            //     CreateVisual();
            //
            // visual.Show();
            //
            gameObject.SetActive(true);
            OnLoadingScreenShownEvent?.Invoke(sender, this);
        }
        
        public void Hide(object sender) {
            // if (visual == null)
            //     throw new Exception("You cant hide loading screen before creating");
            //
            // visual.Hide();
            
            gameObject.SetActive(false);
            OnLoadingScreenHideStartEvent?.Invoke(sender, this);
        }
        
        public void HideInstantly(object sender) {
            // if (visual == null)
            //     throw new Exception("You cant hide loading screen before creating");
            //
            // visual.HideInstantly();
            gameObject.SetActive(false);
            OnLoadingScreenHiddenCompletelyEvent?.Invoke(sender, this);
        }
        
    }
}