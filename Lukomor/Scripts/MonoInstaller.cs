using Lukomor.DI;
using UnityEngine;

namespace Lukomor
{
    public abstract class MonoInstaller : MonoBehaviour, IInstaller
    {
        public abstract void InstallBindings(IDIContainer container);
    }
}