using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VavilichevGD.Architecture.UserInterface;
using VavilichevGD.Tools;

namespace VavilichevGD.Architecture {
    public class SceneManager : ISceneManager{

        #region CONSTANTS

        private const string CONFIG_FOLDER = "SceneConfigs";

        #endregion

        #region DELEGATES

        public event SceneManagerHandler OnSceneLoadStartedEvent;
        public event SceneManagerHandler OnSceneLoadCompletedEvent;

        #endregion

        public Dictionary<string, SceneConfig> scenesConfigMap { get; }
        public IScene sceneActual { get; private set; }
        public bool isLoading { get; private set; }

        public SceneManager() {
            this.scenesConfigMap = new Dictionary<string, SceneConfig>();
            this.InitializeSceneConfigs();
        }

        private void InitializeSceneConfigs() {
            var allSceneConfigs = Resources.LoadAll<SceneConfig>(CONFIG_FOLDER);
            foreach (var sceneConfig in allSceneConfigs) 
                this.scenesConfigMap[sceneConfig.sceneName] = sceneConfig;
        }

        
        
        public Coroutine LoadScene(string sceneName, UnityAction<SceneConfig> sceneLoadedCallback = null) {
            return this.LoadAndInitializeScene(sceneName, sceneLoadedCallback, true);
        }
        
        public Coroutine InitializeCurrentScene(UnityAction<SceneConfig> sceneLoadedCallback = null) {
            var sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            return this.LoadAndInitializeScene(sceneName, sceneLoadedCallback, false);
        }

        
        protected Coroutine LoadAndInitializeScene(string sceneName, UnityAction<SceneConfig> sceneLoadedCallback,
            bool loadNewScene) {
            this.scenesConfigMap.TryGetValue(sceneName, out SceneConfig config);
            
            if (config == null)
                throw new NullReferenceException($"There is no scene ({sceneName}) in the scenes list. The name is wrong or you forget to add it o the list.");

            return Coroutines.StartRoutine(this.LoadSceneRoutine(config, sceneLoadedCallback, loadNewScene));
        }


        protected virtual IEnumerator LoadSceneRoutine(SceneConfig config, UnityAction<SceneConfig> sceneLoadedCallback, bool loadNewScene = true) {
            this.isLoading = true;
            this.OnSceneLoadStartedEvent?.Invoke(config);
            
            if (loadNewScene)
                yield return Coroutines.StartRoutine(this.LoadSceneAsyncRoutine(config));
            yield return Coroutines.StartRoutine(this.InitializeSceneRoutine(config, sceneLoadedCallback));
            
            this.isLoading = false;
            this.OnSceneLoadCompletedEvent?.Invoke(config);
            sceneLoadedCallback?.Invoke(config);
        }

        protected IEnumerator LoadSceneAsyncRoutine(SceneConfig config) {
            var asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(config.sceneName);
            asyncOperation.allowSceneActivation = false;

            var progressDivider = 0.9f;
            var progress = asyncOperation.progress / progressDivider;
            
            while (progress < 1f) {
                yield return null;
                progress = asyncOperation.progress / progressDivider;
            }

            asyncOperation.allowSceneActivation = true;
        }

        protected virtual IEnumerator InitializeSceneRoutine(SceneConfig config, UnityAction<SceneConfig> sceneLoadedCallback) {

            this.sceneActual = new Scene(config);
            yield return null;

            this.sceneActual.BuildUI();
            yield return null;

            this.sceneActual.SendMessageOnCreate();
            yield return null;
            
            yield return this.sceneActual.InitializeAsync();

            this.sceneActual.Start();
        }

    }
}