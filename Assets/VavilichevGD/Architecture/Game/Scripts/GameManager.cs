using UnityEngine;
using VavilichevGD.Tools;

namespace VavilichevGD.Architecture {
    public class GameManager : MonoBehaviour
    {
        #region DELEGATES
        
        public delegate void GameManagerHandler();

        public static event GameManagerHandler OnApplicationPausedEvent;
        public static event GameManagerHandler OnApplicationUnpausedEvent;
        public static event GameManagerHandler OnApplicationFocusedEvent;
        public static event GameManagerHandler OnApplicationUnfocusedEvent;
        public static event GameManagerHandler OnApplicationQuitEvent;

        #endregion
        
        [SerializeField] protected bool saveOnPause;
        [SerializeField] protected bool saveOnUnfocus = true;
        [SerializeField] protected bool saveOnExit = true;

        
        #region Start

        private void Start() {
            DontDestroyOnLoad(this.gameObject);
            
            Logging.Log("GAME LAUNCHED {0}", Application.productName);
            
            this.OnGameLaunched();
        }

        protected virtual void OnGameLaunched(){ }
        
        #endregion
        
        #region Pause/Unpause

        private void OnApplicationPause(bool pauseStatus) {
            if (pauseStatus) {
                Logging.Log("GAME PAUSED");
                
                if (this.saveOnPause)
                    Game.SaveGame();
                this.OnApplicationPaused();  
                OnApplicationPausedEvent?.Invoke();
            }
            else {
                Logging.Log("GAME UNPAUSED");
                this.OnApplicationUnpaused();
                OnApplicationUnpausedEvent?.Invoke();
            }
        }

        protected virtual void OnApplicationPaused() { }
        protected virtual void OnApplicationUnpaused() { }

        #endregion


        #region Focuse/Unfocuse

        private void OnApplicationFocus(bool hasFocus) {
            if (!hasFocus) {
                Logging.Log("GAME UNFOCUSED");
                
                if (this.saveOnUnfocus)
                    Game.SaveGame();
                this.OnApplicationUnfocused();    
                OnApplicationUnfocusedEvent?.Invoke();
            }
            else {
                Logging.Log("GAME FOCUSED");
                this.OnApplicationFocused();
                OnApplicationFocusedEvent?.Invoke();
            }
        }
        
        protected virtual void OnApplicationFocused() { }
        protected virtual void OnApplicationUnfocused() { }

        #endregion


        #region Quit

        private void OnApplicationQuit() {
            Logging.Log("GAME EXITTED");
            if (this.saveOnExit)
                Game.SaveGame();
            this.OnApplicationQuitted();
            OnApplicationQuitEvent?.Invoke();
        }

        protected virtual void OnApplicationQuitted() { }

        #endregion
        
    }
}