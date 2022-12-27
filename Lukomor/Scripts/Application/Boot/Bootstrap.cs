using Lukomor.Common.Utils.Async;
using Lukomor.Domain.Contexts;
using UnityEngine;

namespace Lukomor.Application
{
    [RequireComponent(typeof(ProjectContext))]
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private ProjectContext _projectContext;

        private void Start()
        {
            Game.StartGameAsync(_projectContext).RunAsync();
        }

        private void Reset()
        {
            if (_projectContext == null)
            {
                _projectContext = GetComponent<ProjectContext>();
            }
        }
    }
}