using Lukomor.Presentation;
using Lukomor.UI;
using UnityEngine;

namespace Lukomor.Contexts
{
    public sealed class SceneContext : MonoContext
    {
        [SerializeField] private UISceneConfig uiSceneConfig;
        
        public UISceneConfig UISceneConfig => uiSceneConfig;
    }
}