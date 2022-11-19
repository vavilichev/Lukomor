using Lukomor.Common.Utils.Async;
using Lukomor.Domain.Contexts;
using UnityEngine;

namespace Lukomor.Application
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private ProjectContext _projectContext;

        private void Start()
        {
            Game.StartGameAsync(_projectContext).RunAsync();
        }
    }
}