using Lukomor.DI;
using UnityEngine;

namespace Lukomor
{
    public abstract class Context : MonoBehaviour
    {
        [SerializeField] private MonoInstaller[] _monoInstallers;
        [SerializeField] private ScriptableInstaller[] _scriptableInstallers;

        public IDIContainer Container => _container ??= CreateLocalContainer();

        private IDIContainer _container;

        protected void Initialize()
        {
            foreach (var monoInstaller in _monoInstallers)
            {
                monoInstaller.InstallBindings(Container);
            }

            foreach (var scriptableInstaller in _scriptableInstallers)
            {
                scriptableInstaller.InstallBindings(Container);
            }
        }

        protected abstract IDIContainer CreateLocalContainer();

        private void OnDestroy()
        {
            Container.Dispose();
        }
    }
}