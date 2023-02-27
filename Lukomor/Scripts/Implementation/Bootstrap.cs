using Lukomor.Common.Utils.Async;
using Lukomor.Contexts;
using Lukomor.Domain.Contexts;
using UnityEngine;

namespace Lukomor
{
    [RequireComponent(typeof(ProjectContext))]
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private ProjectContext projectContext;

        private void Start()
        {
            Game.StartGameAsync(projectContext).RunAsync();
        }

#if UNITY_EDITOR
        private void Reset()
        {
            if (projectContext == null)
            {
                projectContext = GetComponent<ProjectContext>();
            }
        }
#endif
    }
}