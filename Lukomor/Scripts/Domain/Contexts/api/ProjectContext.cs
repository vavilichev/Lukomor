using System.Linq;
using Lukomor.Presentation;
using UnityEngine;

namespace Lukomor.Domain.Contexts
{
    public sealed class ProjectContext : MonoContext
    {
        [Space]
        [SerializeField] private UserInterface _userInterfacePrefab;
        [SerializeField] private SceneContext[] _sceneContexts;

        public UserInterface UserInterfacePrefab => _userInterfacePrefab;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public SceneContext GetSceneContext(string sceneName)
        {
            var result = _sceneContexts.FirstOrDefault(c => c.SceneName == sceneName);
            
            return result;
        }
    }
}