using System.Collections.Generic;
using UnityEngine;
using VavilichevGD.Architecture.UI;

namespace VavilichevGD.Architecture {
    public interface IScene {
        
        ISceneConfig sceneConfig { get; }
        RepositoriesBase repositoriesBase { get; }
        InteractorsBase interactorsBase { get; }
        UIController uiController { get; }

        
        void CreateInstances();
        Coroutine InitializeAsync();
        void Start();

        T GetRepository<T>() where T : IRepository;
        IEnumerable<T> GetRepositories<T>() where T : IRepository;
        
        T GetInteractor<T>() where T : IInteractor;
        IEnumerable<T> GetInteractors<T>() where T : IInteractor;
    }
}