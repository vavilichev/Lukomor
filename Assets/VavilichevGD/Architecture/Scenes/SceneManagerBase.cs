using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VavilichevGD.Tools;

namespace VavilichevGD.Architecture {
    public abstract class SceneManagerBase : ISceneManager{

        #region DELEGATES

        public event SceneManagerHandler OnSceneLoadStartedEvent;
        public event SceneManagerHandler OnSceneLoadCompletedEvent;

        #endregion

        public Dictionary<string, ISceneConfig> scenesConfigMap { get; }
        public IScene sceneActual { get; private set; }
        public bool isLoading { get; private set; }

        public SceneManagerBase() {
            this.scenesConfigMap = new Dictionary<string, ISceneConfig>();
            // ReSharper disable once VirtualMemberCallInConstructor
            this.InitializeSceneConfigs();
        }

        protected abstract void InitializeSceneConfigs();

        
        
        public Coroutine LoadScene(string sceneName, UnityAction<ISceneConfig> sceneLoadedCallback = null) {
            return this.LoadAndInitializeScene(sceneName, sceneLoadedCallback, true);
        }
        
        public Coroutine InitializeCurrentScene(UnityAction<ISceneConfig> sceneLoadedCallback = null) {
            var sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            return this.LoadAndInitializeScene(sceneName, sceneLoadedCallback, false);
        }

        
        protected Coroutine LoadAndInitializeScene(string sceneName, UnityAction<ISceneConfig> sceneLoadedCallback,
            bool loadNewScene) {
            this.scenesConfigMap.TryGetValue(sceneName, out ISceneConfig config);
            
            if (config == null)
                throw new NullReferenceException($"There is no scene ({sceneName}) in the scenes list. The name is wrong or you forget to add it o the list.");

            return Coroutines.StartRoutine(this.LoadSceneRoutine(config, sceneLoadedCallback, loadNewScene));
        }


        protected virtual IEnumerator LoadSceneRoutine(ISceneConfig config, UnityAction<ISceneConfig> sceneLoadedCallback, bool loadNewScene = true) {
            this.isLoading = true;
            this.OnSceneLoadStartedEvent?.Invoke(config);
            
            if (loadNewScene)
                yield return Coroutines.StartRoutine(this.LoadSceneAsyncRoutine(config));
            yield return Coroutines.StartRoutine(this.InitializeSceneRoutine(config, sceneLoadedCallback));
            
            this.isLoading = false;
            this.OnSceneLoadCompletedEvent?.Invoke(config);
            sceneLoadedCallback?.Invoke(config);
        }

        protected IEnumerator LoadSceneAsyncRoutine(ISceneConfig config) {
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

        protected virtual IEnumerator InitializeSceneRoutine(ISceneConfig config, UnityAction<ISceneConfig> sceneLoadedCallback) {

            this.sceneActual = new Scene(config);
            this.sceneActual.CreateInstances();

            yield return this.sceneActual.InitializeAsync();

            this.sceneActual.Start();
        }

    }
}