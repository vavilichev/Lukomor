using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VavilichevGD.Architecture.UserInterface;
using VavilichevGD.Utils.Attributes;

namespace VavilichevGD.Architecture {
    [CreateAssetMenu(fileName = "SceneConfig", menuName = "Architecture/Scenes/New SceneConfig")]
    public sealed class SceneConfig : ScriptableObject {

        [SerializeField, SceneName] private string _sceneName;

        [Header("======= CORE ARCHITECTURE =======")]
        [SerializeField, ClassReference(typeof(Repository))]
        private string[] _repositoryReferences;
        
        [SerializeField, ClassReference(typeof(Interactor))]
        private string[] _interactorsReferences;

        
        [Header("======= UI STRUCTURE ======="), Space (20)]
        [SerializeField] 
        [GameObjectOfType(typeof(IUIElementOnLayer))]
        private List<GameObject> _uiPrefabs;
        
        [Header("======= STORAGE SETTING S======="), Space(20)]
        [SerializeField] private bool _saveDataForThisScene;
        [SerializeField] private string _saveName;

        
        
        public string sceneName => _sceneName;
        public string[] repositoriesReferences => _repositoryReferences;
        public string[] interactorsReferences => _interactorsReferences;
        public IUIElementOnLayer[] uiPrefabs => GetUIPrefabs();

        public bool saveDataForThisScene => _saveDataForThisScene;
        public string saveName => _saveName;
        
        
        
        
        public IUIElementOnLayer[] GetUIPrefabs() {
            var uiPrefabs = new List<IUIElementOnLayer>();
            foreach (var goPrefab in _uiPrefabs) {
                var uiPrefab = goPrefab.GetComponent<IUIElementOnLayer>();
                uiPrefabs.Add(uiPrefab);
            }

            return uiPrefabs.ToArray();
        }
        
        public IUIElementOnLayer GetPrefab(Type type) {
            var allPrefab = uiPrefabs;
            return allPrefab.First(pref => pref.GetType() == type);
        }

    }
}