using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VavilichevGD.Architecture.StorageSystem;
using VavilichevGD.Tools;

namespace VavilichevGD.Architecture {
    public abstract class Repository : IRepository{

        #region DELEGATES

        public delegate void RepositoryHandler(Repository repository);
        public event RepositoryHandler OnRepositoryInitializedEvent;
        public event RepositoryHandler OnRepositoryStartedEvent;

        #endregion
        
        
        public State state { get; private set; }
        public bool isInitialized => this.state == State.Initialized;
        public abstract string id { get; }
        public virtual int version { get; } = 1;
        
        
        public Repository() {
            state = State.NotInitialized;
        }

        public virtual void OnCreate() { }


        
        #region INITIALIZATION

        public Coroutine InitializeAsync() {

            if (this.isInitialized)
                throw new Exception($"Repository {this.GetType()} is already initialized");

            if (state == State.Initializing)
                throw new Exception($"Repository {this.GetType()} is initializing now");

            return Coroutines.StartRoutine(InitializeRoutineBase());
        }
        
        protected IEnumerator InitializeRoutineBase() {
            this.state = State.Initializing;
            this.Initialize();
            yield return Coroutines.StartRoutine(this.InitializeRoutine());

            this.state = State.Initialized;
            this.OnInitialized();
            this.OnRepositoryInitializedEvent?.Invoke(this);
        }

        protected virtual void Initialize() { }

        protected virtual IEnumerator InitializeRoutine() {
            yield break;
        }

        protected virtual void OnInitialized() { }

        #endregion

        
        
        #region START

        public void Start() {
            this.OnStart();
            this.OnRepositoryStartedEvent?.Invoke(this);
        }

        protected virtual void OnStart() { }

        #endregion


        #region SAVE

        public virtual void Save() { }

        public Coroutine SaveAsync() {
            return Coroutines.StartRoutine(this.SaveAsyncRoutine());
        }

        protected virtual IEnumerator SaveAsyncRoutine() {
            this.Save();
            yield return null;
        }

        public abstract RepoData GetRepoData();
        public abstract RepoData GetRepoDataDefault();

        public abstract void UploadRepoData(RepoData repoData);

        #endregion


        #region STATUS

        public virtual string GetStatusStartInitializing() {
            return $"REPO START INITIALIZING: {this.GetType()}";
        }

        public virtual string GetStatusCompleteInitializing() {
            return $"REPO INITIALIZING COMPLETE: {this.GetType()}";
        }

        public string GetStatusStart() {
            return $"REPO STARTED: {this.GetType()}";
        }

        #endregion


        public T GetInteractor<T>() where T : Interactor {
            return Game.GetInteractor<T>();
        }

        public T GetRepository<T>() where T : Repository {
            return Game.GetRepository<T>();
        }

        protected IEnumerable<T> GetInteractors<T>() where T : IInteractor {
            return Game.GetInteractors<T>();
        }
        
        protected IEnumerable<T> GetRepositories<T>() where T : IRepository {
            return Game.GetRepositories<T>();
        }
    }
}