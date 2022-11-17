using System.Collections.Generic;
using UnityEngine;

namespace Lukomor.Presentation.Views.Windows
{
    [CreateAssetMenu(fileName = "SceneViewModelsConfig", menuName = "Configs/UI/New SceneViewModelsConfig")]
    public class SceneViewModelsConfig : ScriptableObject
    {
        [SerializeField] private List<WindowViewModel> _viewModelPrefabs;

        public WindowViewModel[] ViewModelPrefabs;
    }
}