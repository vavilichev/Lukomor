using System.Linq;
using Lukomor.Presentation;
using UnityEngine;

namespace Lukomor.Domain.Contexts
{
    public abstract class ProjectContext : MonoContext
    {
        [SerializeField] private UserInterface _userInterfacePrefab;
        [SerializeField] private SceneContext[] _sceneContexts;

        public UserInterface UserInterfacePrefab => _userInterfacePrefab;

        public SceneContext GetSceneContext(string sceneName)
        {
            var result = _sceneContexts.FirstOrDefault(c => c.SceneName == sceneName);
            
            return result;
        }
    }
}