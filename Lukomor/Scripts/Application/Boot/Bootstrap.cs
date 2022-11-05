using Lukomor.Application.Contexts;
using UnityEngine;
using VavilichevGD.Tools.Async;

namespace Lukomor.Application.Boot
{
    public abstract class Bootstrap : MonoBehaviour
    {
        [SerializeField] private MonoContext _projectContext;

        private void Start()
        {
            Game.StartGameAsync(_projectContext).RunAsync();
        }
    }
}