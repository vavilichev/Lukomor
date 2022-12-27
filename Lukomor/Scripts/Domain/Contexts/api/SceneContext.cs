using Lukomor.Presentation;
using UnityEngine;

namespace Lukomor.Domain.Contexts
{
    public sealed class SceneContext : MonoContext
    {
        [Space]
#if UNITY_EDITOR
        [SerializeField] private UnityEditor.SceneAsset _scene;
#endif
        [SerializeField] private UISceneConfig _uiSceneConfig;
        [HideInInspector] public string SceneName;
        
        public UISceneConfig UISceneConfig => _uiSceneConfig;

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (_scene != null)
            {
                SceneName = _scene.name;
            }
#endif
        }
    }
}