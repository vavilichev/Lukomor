using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VavilichevGD.Tools;

namespace VavilichevGD.Architecture {

    public enum State {
        NotInitialized,
        Initializing,
        Initialized
    }
    
    public abstract class Game {

        #region DELEGATES

        public delegate void GameHandler();
        public static event GameHandler OnGameInitializedEvent;

        #endregion
        
        protected static Game instance;
        public static State state { get; private set; }
        public static bool isInitialized => state == State.Initialized;
        public static ISceneManager sceneManager { get; private set; }

       

        // TODO: You should write your own Game*name* script and past something like that:
//        public static void Run() {
//            // Create instance.
//            // Initialize instance.
//        }

        #region Initializing

        public Game() {
            state = State.NotInitialized;
        }

        public void Initialize() {
            Logging.Log("GAME START INITIALIZING");
            state = State.Initializing;

            sceneManager = this.CreateSceneManager();
            Logging.Log("GAME: Scene manager created: {0}", sceneManager.GetType().Name);
            
            this.LoadFirstScene(this.OnSceneLoadCompleted);
        }

        protected abstract SceneManagerBase CreateSceneManager();
        protected abstract void LoadFirstScene(UnityAction<ISceneConfig> callback);

        private void OnSceneLoadCompleted(ISceneConfig config) {
            state = State.Initialized;
            OnGameInitializedEvent?.Invoke();
            Logging.Log("GAME FULLY INITIALIZED");
        }

        #endregion
        
        
        public static T GetInteractor<T>() where T : Interactor {
            return sceneManager.sceneActual.GetInteractor<T>();
        }

        public static IEnumerable<T> GetInteractors<T>() where T : IInteractor {
            return sceneManager.sceneActual.GetInteractors<T>();
        }

        public static T GetRepository<T>() where T : Repository {
            return sceneManager.sceneActual.GetRepository<T>();
        }
        
        public static IEnumerable<T> GetRepositories<T>() where T : IRepository {
            return sceneManager.sceneActual.GetRepositories<T>();
        }

        public static void SaveGame() {
            Logging.Log("GAME SAVE INSTANTLY");
            sceneManager.sceneActual.Save();
        }

        public static Coroutine SaveGameAsync(UnityAction callback) {
            Logging.Log("GAME SAVE ASYNC");
            return sceneManager.sceneActual.SaveAsync(callback);
        }

    }
}