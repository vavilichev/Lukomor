using System;
using UnityEngine;

namespace VavilichevGD.Architecture.StorageSystem {
    [Serializable]
    public class RepoData {
        
        public string id;
        public int version;
        public string json;

        public RepoData(string id, string json, int version = 1) {
            this.id = id;
            this.version = version;
            this.json = json;
        }
        
        public RepoData(string id, IRepoEntity repoEntity, int version = 1) {
            this.id = id;
            this.version = version;
            this.json = repoEntity.ToJson();
        }

        public T GetEntity<T>() where T : IRepoEntity {
            var data = JsonUtility.FromJson<T>(this.json);
            return data;
        }

        public string ToJson() {
            return JsonUtility.ToJson(this);
        }
    }
}