using UnityEngine;
using VavilichevGD.Utils.Attributes;

namespace VavilichevGD.Architecture {
    [CreateAssetMenu(fileName = "SceneConfig", menuName = "Architecture/Scenes/New SceneConfig")]
    public sealed class SceneConfig : ScriptableObject {

        [SerializeField, SceneName] private string _sceneName;

        [SerializeField, ClassReference(typeof(Repository))]
        private string[] _repositoryReferences;
        
        [SerializeField, ClassReference(typeof(Interactor))]
        private string[] _interactorsReferences;

        [Space, SerializeField] private bool _saveDataForThisScene;
        [SerializeField] private string _saveName;

        public string sceneName => _sceneName;
        public string[] repositoriesReferences => _repositoryReferences;
        public string[] interactorsReferences => _interactorsReferences;
        public bool saveDataForThisScene => _saveDataForThisScene;
        public string saveName => _saveName;

    }
}