using UnityEngine;

namespace VavilichevGD.Architecture {
    public interface IInteractor {
        bool isInitialized { get; }

        void OnCreate();
        Coroutine InitializeAsync();
        void Start();
        
        string GetStatusStartInitializing();
        string GetStatusCompleteInitializing();
        string GetStatusStart();
    }
}