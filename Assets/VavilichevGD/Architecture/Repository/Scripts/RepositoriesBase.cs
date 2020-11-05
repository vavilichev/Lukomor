using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VavilichevGD.Tools;

namespace VavilichevGD.Architecture {
    public class RepositoriesBase {

        #region DELEGATES

        public delegate void RepositoriesBaseHandler(string statusText);
        public event RepositoriesBaseHandler OnRepositoriesBaseStatusChangedEvent;

        #endregion

        private Dictionary<Type, IRepository> repositoriesMap;
        private ISceneConfig sceneConfig;
        
        public RepositoriesBase(ISceneConfig sceneConfig) {
            this.repositoriesMap = new Dictionary<Type, IRepository>();
            this.sceneConfig = sceneConfig;
        }

        
        public void CreateAllRepositories() {
            this.repositoriesMap = this.sceneConfig.CreateAllRepositories();
        }
        
        

        #region INITIALIZING

        public Coroutine InitializeAllRepositories() {
            return Coroutines.StartRoutine(InitializeAllRepositoriesRoutine());
        }

        private IEnumerator InitializeAllRepositoriesRoutine() {
            IRepository[] allRepositories = repositoriesMap.Values.ToArray();
            foreach (IRepository repository in allRepositories) {
                if (!repository.isInitialized) {
                    this.OnRepositoriesBaseStatusChangedEvent?.Invoke(repository.GetStatusStartInitializing());
                    yield return repository.InitializeAsync();
                    this.OnRepositoriesBaseStatusChangedEvent?.Invoke(repository.GetStatusCompleteInitializing());
                }
            }
        }

        #endregion

        
        
        #region START

        public void StartAllRepositories() {
            IRepository[] allRepositories = repositoriesMap.Values.ToArray();
            foreach (IRepository repository in allRepositories) {
                repository.Start();
                this.OnRepositoriesBaseStatusChangedEvent?.Invoke(repository.GetStatusStart());
            }
        } 

        #endregion



        public T GetRepository<T>() where T : IRepository {
            var type = typeof(T);
            IRepository resultRepository;
            var founded = repositoriesMap.TryGetValue(type, out resultRepository);
            
            if (founded)
                return (T) resultRepository;
            
            foreach (IRepository repository in repositoriesMap.Values) {
                if (repository is T)
                    return (T) repository;
            }

            throw new KeyNotFoundException($"Key: {type}");
        }
        
        public IEnumerable<T> GetRepositories<T>() where T : IRepository {
            var allRepositories = this.repositoriesMap.Values;
            var requiredRepositories = new HashSet<T>();
            foreach (var repository in allRepositories) {
                if (repository is T)
                    requiredRepositories.Add((T) repository);
            }

            return requiredRepositories;
        }
        
    }
}