using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VavilichevGD.Architecture.Settings;
using VavilichevGD.Architecture.UserInterface;
using VavilichevGD.Tools;

namespace VavilichevGD.Architecture {

    public enum State {
        NotInitialized,
        Initializing,
        Initialized
    }
    
    public abstract class Game {

        #region EVENTS

        public static event Action OnGameInitializedEvent;

        #endregion


        public static State state { get; private set; } = State.NotInitialized;
        public static bool isInitialized => state == State.Initialized;
        public static ISceneManager sceneManager { get; private set; }
        public static IGameSettings gameSettings { get; private set; }



        public static void Run(object sender = null) {
            Coroutines.StartRoutine(RunGameRoutine());
        }


        private static IEnumerator RunGameRoutine() {
            state = State.Initializing;

            InitGameSettings();
            yield return null;
            
            InitSceneManager();
            yield return null;

            yield return sceneManager.InitializeCurrentScene();

            state = State.Initialized;
            OnGameInitializedEvent?.Invoke();
        }



        private static void InitGameSettings() {
            gameSettings = new GameSettings();
            gameSettings.Load();
        }

        private static void InitSceneManager() {
            sceneManager = new SceneManager();;
            Logging.Log("GAME: Scene manager created: {0}", sceneManager.GetType().Name);
        }

        
        
        
        
        
        
        
        
        public static T GetInteractor<T>() where T : IInteractor {
            return sceneManager.sceneActual.GetInteractor<T>();
        }

        public static IEnumerable<T> GetInteractors<T>() where T : IInteractor {
            return sceneManager.sceneActual.GetInteractors<T>();
        }

        public static T GetRepository<T>() where T : IRepository {
            return sceneManager.sceneActual.GetRepository<T>();
        }
        
        public static IEnumerable<T> GetRepositories<T>() where T : IRepository {
            return sceneManager.sceneActual.GetRepositories<T>();
        }

        public static void SaveGame() {
            Logging.Log("GAME SAVE INSTANTLY");
            // sceneManager.sceneActual.Save();
        }

        public static Coroutine SaveGameAsync(UnityAction callback) {
            Logging.Log("GAME SAVE ASYNC");
            return null;
            // return sceneManager.sceneActual.SaveAsync(callback);
        }

    }
}