using UnityEngine;
using VavilichevGD.Architecture.StorageSystem;

namespace VavilichevGD.Architecture {
    public interface IRepository {
        
        bool isInitialized { get; }
        string id { get; }
        int version { get; }
        
        void OnCreate();
        Coroutine InitializeAsync();
        void Start();
        
        RepoData GetRepoData();
        RepoData GetRepoDataDefault();
        void UploadRepoData(RepoData repoData);

        string GetStatusStartInitializing();
        string GetStatusCompleteInitializing();
        string GetStatusStart();
    }
}