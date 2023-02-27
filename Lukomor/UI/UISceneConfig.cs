using System;
using UnityEngine;

namespace Lukomor.UI
{
    [CreateAssetMenu(fileName = "UISceneConfig", menuName = "UI/New UI Scene Config")]
    public class UISceneConfig : ScriptableObject
    {
        [SerializeField] private WindowViewModel[] windowPrefabs;

        public WindowViewModel[] WindowPrefabs => windowPrefabs;
        
        public bool TryGetPrefab(Type windowType, out WindowViewModel requestedPrefab)
        {
            requestedPrefab = default;
            
            foreach (var prefab in windowPrefabs)
            {
                if (prefab.GetType() == windowType)
                {
                    requestedPrefab = prefab;

                    break;
                }
            }

            return requestedPrefab != null;
        }
    }
}