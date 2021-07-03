using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VavilichevGD.Architecture.StorageSystem;
using VavilichevGD.Architecture.UI;
using VavilichevGD.Tools;

namespace VavilichevGD.Architecture {
    public sealed class Scene : IScene {
        public SceneConfig sceneConfig { get; }
        public ComponentsBase<IRepository> repositoriesBase { get; }
        public ComponentsBase<IInteractor> interactorsBase { get; }

        public UIController uiController { get; }
        //public UIController uiController { get; }

        public Scene(SceneConfig config) {
            this.sceneConfig = config;
            this.repositoriesBase = new ComponentsBase<IRepository>(config.repositoriesReferences);
            this.interactorsBase = new ComponentsBase<IInteractor>(config.interactorsReferences);
        }



        #region ONCREATE

        public void SendMessageOnCreate() {
            this.repositoriesBase.SendMessageOnCreate();
            this.repositoriesBase.SendMessageOnCreate();
        }

        #endregion

        
        #region INITIALIZE

        public Coroutine InitializeAsync() {
            return Coroutines.StartRoutine(this.InitializeAsyncRoutine());
        }

        private IEnumerator InitializeAsyncRoutine() {
            Storage.instance.Load();
            yield return this.repositoriesBase.InitializeAllComponents();
            yield return this.interactorsBase.InitializeAllComponents();
            
            this.repositoriesBase.SendMessageOnInitialize();
            this.interactorsBase.SendMessageOnInitialize();
        }

        #endregion


        #region START

        public void Start() {
            this.repositoriesBase.SendMessageOnStart();
            this.interactorsBase.SendMessageOnStart();
        }

        #endregion


        public T GetRepository<T>() where T : IRepository {
            return this.repositoriesBase.GetComponent<T>();
        }

        public IEnumerable<T> GetRepositories<T>() where T : IRepository {
            return this.repositoriesBase.GetComponents<T>();
        }

        public T GetInteractor<T>() where T : IInteractor {
            return this.interactorsBase.GetComponent<T>();
        }
        
        public IEnumerable<T> GetInteractors<T>() where T : IInteractor {
            return this.interactorsBase.GetComponents<T>();
        }

    }
}