using Lukomor.Presentation.Views.Windows;
using UnityEngine;

namespace Lukomor.Presentation
{
    [CreateAssetMenu(fileName = "UISceneConfig", menuName = "UI/Configs/New UI Scene Config")]
    public class UISceneConfig : ScriptableObject
    {
        [SerializeField] private WindowViewModel[] _windowPrefabs;

        public WindowViewModel[] WindowPrefabs => _windowPrefabs;
        
        public bool TryGetPrefab<T>(out T requestedPrefab) where T : WindowViewModel
        {
            requestedPrefab = null;
            
            foreach (var prefab in _windowPrefabs)
            {
                if (prefab is T certainPrefab)
                {
                    requestedPrefab = certainPrefab;
                    
                    break;
                }
            }

            return requestedPrefab != null;
        }
    }
}