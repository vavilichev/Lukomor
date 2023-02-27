using Lukomor.Presentation;
using Lukomor.UI;
using UnityEngine;

namespace Lukomor.Contexts
{
    public sealed class ProjectContext : MonoContext
    {
        [Space]
        [SerializeField] private UserInterface userInterfacePrefab;

        public UserInterface UserInterfacePrefab => userInterfacePrefab;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}