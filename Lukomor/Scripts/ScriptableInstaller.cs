using Lukomor.DI;
using UnityEngine;

namespace Lukomor
{
    public abstract class ScriptableInstaller : ScriptableObject, IInstaller
    {
        public abstract void InstallBindings(IDIContainer container);
    }
}