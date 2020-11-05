using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VavilichevGD.Core.Loadging {
    public static class LoadingScreen {

        #region CONSTANTS

        private const string PREF_PATH = "[LOADING_SCREEN]";

        #endregion

        
        #region DELEGATES

        public delegate void LoadingScreenHandler();

        public static event LoadingScreenHandler OnLoadingScreenShownEvent;
        public static event LoadingScreenHandler OnLoadingScreenHideStartEvent;
        public static event LoadingScreenHandler OnLoadingScreenHiddenCompletelyEvent;

        #endregion


        public static bool isActive => visual != null && visual.isActive;

        private static LoadingScreenVisualBase visual;
        
        
        private static void CreateVisual() {
            // Change to custom visual if needed
            var pref = Resources.Load<LoadingScreenVisualDefault>(PREF_PATH);
            var createdLoadingScreen = Object.Instantiate(pref);
            Resources.UnloadUnusedAssets();

            visual = createdLoadingScreen;
            visual.OnShownEvent += OnShown;
            visual.OnHideStartEvent += OnHideStart;
            visual.OnHiddenCompletelyEvent += OnHiddenCompletely;
        }
        
        
        
        public static  void Show() {
            if (visual == null)
                CreateVisual();
            
            visual.Show();
        }
        
        public static void Hide() {
            if (visual == null)
                throw new Exception("You cant hide loading screen before creating");
            
            visual.Hide();
        }
        
        public static void HideInstantly() {
            if (visual == null)
                throw new Exception("You cant hide loading screen before creating");
            
            visual.HideInstantly();
        }
        

        
        #region Events

        private static void OnHiddenCompletely(LoadingScreenVisualBase visualbase) {
            OnLoadingScreenHiddenCompletelyEvent?.Invoke();
        }

        private static void OnHideStart(LoadingScreenVisualBase visualbase) {
            OnLoadingScreenHideStartEvent?.Invoke();
        }

        private static void OnShown(LoadingScreenVisualBase visualbase) {
            OnLoadingScreenShownEvent?.Invoke();
        }
        
        #endregion
        
    }
}