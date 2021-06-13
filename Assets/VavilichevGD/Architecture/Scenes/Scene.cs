using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VavilichevGD.Architecture.StorageSystem;
using VavilichevGD.Architecture.UI;
using VavilichevGD.Tools;

namespace VavilichevGD.Architecture {
    public sealed class Scene : IScene {
        public ISceneConfig sceneConfig { get; }
        public RepositoriesBase repositoriesBase { get; }
        public InteractorsBase interactorsBase { get; }

        public UIController uiController { get; }
        //public UIController uiController { get; }

        public Scene(ISceneConfig config) {
            this.sceneConfig = config;
            this.repositoriesBase = new RepositoriesBase(config);
            this.interactorsBase = new InteractorsBase(config);
        }

        
        #region CREATE INSTANCES

        public void CreateInstances() {
            this.CreateAllRepositories();
            this.CreateAllInteractors();
        }
        
        private void CreateAllRepositories() {
            this.repositoriesBase.CreateAllRepositories();
        }

        private void CreateAllInteractors() {
            this.interactorsBase.CreateAllInteractors();
        }

        #endregion


        #region INITIALIZE

        public Coroutine InitializeAsync() {
            return Coroutines.StartRoutine(this.InitializeAsyncRoutine());
        }

        private IEnumerator InitializeAsyncRoutine() {
            Storage.instance.Load();
            yield return this.repositoriesBase.InitializeAllRepositories();
            yield return this.interactorsBase.InitializeAllInteractors();
        }

        #endregion


        #region START

        public void Start() {
            this.repositoriesBase.StartAllRepositories();
            this.interactorsBase.StartAllInteractors();
        }

        #endregion


        public T GetRepository<T>() where T : IRepository {
            return this.repositoriesBase.GetRepository<T>();
        }

        public IEnumerable<T> GetRepositories<T>() where T : IRepository {
            return this.repositoriesBase.GetRepositories<T>();
        }

        public T GetInteractor<T>() where T : IInteractor {
            return this.interactorsBase.GetInteractor<T>();
        }
        
        public IEnumerable<T> GetInteractors<T>() where T : IInteractor {
            return this.interactorsBase.GetInteractors<T>();
        }

    }
}