using System;
using System.Collections;
using System.Collections.Generic;
using VavilichevGD.Architecture.Settings;
using VavilichevGD.Tools;

namespace VavilichevGD.Architecture {

    public abstract class Game {

        #region EVENTS

        public static event Action OnGameInitializedEvent;

        #endregion


        public static ArchitectureComponentState state { get; private set; } = ArchitectureComponentState.NotInitialized;
        public static bool isInitialized => state == ArchitectureComponentState.Initialized;
        public static ISceneManager sceneManager { get; private set; }
        public static IGameSettings gameSettings { get; private set; }



        #region GAME RUNNING

        public static void Run() {
            Coroutines.StartRoutine(RunGameRoutine());
        }

        private static IEnumerator RunGameRoutine() {
            state = ArchitectureComponentState.Initializing;

            InitGameSettings();
            yield return null;
            
            InitSceneManager();
            yield return null;

            yield return sceneManager.InitializeCurrentScene();

            state = ArchitectureComponentState.Initialized;
            OnGameInitializedEvent?.Invoke();
        }



        private static void InitGameSettings() {
            gameSettings = new GameSettings();
        }

        private static void InitSceneManager() {
            sceneManager = new SceneManager();;
        }

        #endregion
        
        

        
        
        
        
        
        
        
        
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
            sceneManager.sceneActual.fileStorage.Save();;
        }

        public static void SaveGameAsync(Action callback) {
            sceneManager.sceneActual.fileStorage.SaveAsync(callback);
        }

        public static IEnumerator SaveWithRoutine(Action callback) {
            yield return sceneManager.sceneActual.fileStorage.SaveWithRoutine();
        }


    }
}