using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VavilichevGD.Tools;

namespace VavilichevGD.Architecture {
    public class InteractorsBase {

        private Dictionary<Type, IInteractor> interactorsMap;
        private ISceneConfig sceneConfig;

        public InteractorsBase(ISceneConfig sceneConfig) {
            this.interactorsMap = new Dictionary<Type, IInteractor>();
            this.sceneConfig = sceneConfig;
        }
        
        
        public void CreateAllInteractors() {
            this.interactorsMap = this.sceneConfig.CreateAllInteractors();
        }

        
        
        #region INITIALIZING

        public Coroutine InitializeAllInteractors() {
            return Coroutines.StartRoutine(InitializeAllInteractorsRoutine());
        }

        private IEnumerator InitializeAllInteractorsRoutine() {
            var allInteractors = this.interactorsMap.Values.ToArray();
            foreach (IInteractor interactor in allInteractors) {
                if (!interactor.isInitialized)
                    yield return interactor.InitializeWithRoutine();
            }
        }

        #endregion

        
        
        #region START

        public void StartAllInteractors() {
            IInteractor[] allInteractors = this.interactorsMap.Values.ToArray();
            foreach (IInteractor interactor in allInteractors) 
                interactor.OnStarted();
        } 

        #endregion

        public T GetInteractor<T>() where T : IInteractor {
            var type = typeof(T);
            var founded = interactorsMap.TryGetValue(type, out IInteractor resultInteractor);
            if (founded)
                return (T) resultInteractor;

            foreach (IInteractor interactor in interactorsMap.Values) {
                if (interactor is T)
                    return (T) interactor;
            }

            throw new KeyNotFoundException($"Key: {type}");
        }

        public IEnumerable<T> GetInteractors<T>() where T : IInteractor {
            var allInteractors = this.interactorsMap.Values;
            var requiredInteractors = new HashSet<T>();
            foreach (var interactor in allInteractors) {
                if (interactor is T)
                    requiredInteractors.Add((T) interactor);
            }

            return requiredInteractors;
        }
        
    }
}