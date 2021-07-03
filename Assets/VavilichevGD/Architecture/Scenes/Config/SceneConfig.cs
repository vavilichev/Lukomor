using UnityEngine;
using VavilichevGD.Architecture.Utils;

namespace VavilichevGD.Architecture {
    [CreateAssetMenu(fileName = "SceneConfig", menuName = "Architecture/Scenes/New SceneConfig")]
    public sealed class SceneConfig : ScriptableObject {

        [SerializeField, SceneName] private string _sceneName;

        [SerializeField, ClassReference(typeof(Repository))]
        private string[] _repositoryReferences;
        
        [SerializeField, ClassReference(typeof(Interactor))]
        private string[] _interactorsReferences;

        public string sceneName => this._sceneName;
        public string[] repositoriesReferences => _repositoryReferences;
        public string[] interactorsReferences => _interactorsReferences;

    }
}