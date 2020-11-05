using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VavilichevGD.Tools;

namespace VavilichevGD.Architecture {
    public abstract class Interactor : IInteractor {

        #region DELEGATES

        public delegate void InteractorHandler(Interactor interactor);
        public event InteractorHandler OnInteractorInitializedEvent;
        public event InteractorHandler OnInteractorStartedEvent;

        #endregion
        

        public State state { get; private set; }
        public bool isInitialized => this.state == State.Initialized;
        
        public Interactor() {
            state = State.NotInitialized;
        }

        public virtual void OnCreate() { }

        

        #region INITIALIZATION

        public Coroutine InitializeAsync() {

            if (this.isInitialized)
                throw new Exception($"Interactor {this.GetType()} is already initialized");

            if (state == State.Initializing)
                throw new Exception($"Interactor {this.GetType()} is initializing now");

            return Coroutines.StartRoutine(InitializeRoutineBase());
        }


        protected IEnumerator InitializeRoutineBase() {
            this.state = State.Initializing;
            this.Initialize();
            yield return Coroutines.StartRoutine(this.InitializeRoutine());

            this.state = State.Initialized;
            this.OnInitialized();
            this.OnInteractorInitializedEvent?.Invoke(this);
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
            this.OnInteractorStartedEvent?.Invoke(this);
        }

        protected virtual void OnStart() { }

        #endregion
        
        
        #region STATUS

        public virtual string GetStatusStartInitializing() {
            return $"INTERACTOR START INITIALIZING: {this.GetType()}";
        }

        public virtual string GetStatusCompleteInitializing() {
            return $"INTERACTOR INITIALIZING COMPLETE: {this.GetType()}";
        }

        public string GetStatusStart() {
            return $"INTERACTOR STARTED: {this.GetType()}";
        }

        #endregion

        
        protected T GetInteractor<T>() where T : Interactor {
            return Game.GetInteractor<T>();
        }

        protected T GetRepository<T>() where T : Repository {
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