using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VavilichevGD.Architecture.StorageSystem;
using VavilichevGD.Architecture.UserInterface;
using VavilichevGD.Tools;

namespace VavilichevGD.Architecture {
    public sealed class Scene : IScene {
        public SceneConfig sceneConfig { get; }
        public ComponentsBase<IRepository> repositoriesBase { get; }
        public ComponentsBase<IInteractor> interactorsBase { get; }
        
        public Storage fileStorage { get; private set; }


        public Scene(SceneConfig config) {
            sceneConfig = config;
            repositoriesBase = new ComponentsBase<IRepository>(config.repositoriesReferences);
            interactorsBase = new ComponentsBase<IInteractor>(config.interactorsReferences);
        }

        public void BuildUI() {
            UI.Build(sceneConfig.sceneName);
        }



        #region ONCREATE

        public void SendMessageOnCreate() {
            repositoriesBase.SendMessageOnCreate();
            repositoriesBase.SendMessageOnCreate();
            UI.controller.SendMessageOnCreate();
        }

        #endregion

        
        #region INITIALIZE

        public Coroutine InitializeAsync() {
            return Coroutines.StartRoutine(InitializeAsyncRoutine());
        }

        private IEnumerator InitializeAsyncRoutine() {
            // TODO: Load storage here if needed.
            if (sceneConfig.saveDataForThisScene) {
                fileStorage = new FileStorage(sceneConfig.saveName);
                fileStorage.Load();
            }

            yield return repositoriesBase.InitializeAllComponents();
            yield return interactorsBase.InitializeAllComponents();
            
            repositoriesBase.SendMessageOnInitialize();
            interactorsBase.SendMessageOnInitialize();
            UI.controller.SendMessageOnInitialize();
        }

        #endregion


        #region START

        public void Start() {
            repositoriesBase.SendMessageOnStart();
            interactorsBase.SendMessageOnStart();
            UI.controller.SendMessageOnStart();
        }

        public void Save() {
            fileStorage?.Save();
        }

        #endregion


        public T GetRepository<T>() where T : IRepository {
            return repositoriesBase.GetComponent<T>();
        }

        public IEnumerable<T> GetRepositories<T>() where T : IRepository {
            return repositoriesBase.GetComponents<T>();
        }

        public T GetInteractor<T>() where T : IInteractor {
            return interactorsBase.GetComponent<T>();
        }
        
        public IEnumerable<T> GetInteractors<T>() where T : IInteractor {
            return interactorsBase.GetComponents<T>();
        }

    }
}