using System;
using Lukomor.DI;
using Lukomor.Domain.Contexts;
using Lukomor.Features;
using UnityEngine;

namespace Lukomor.Contexts
{
    public abstract class FeatureInstaller : ScriptableObject, IFeatureInstaller, IDisposable
    {
        protected DiContainer Container { get; private set; }

        public IFeature Create(DiContainer container)
        {
            Container = container;
            
            return CreateInternal();
        }
        
        public abstract void Dispose();

        protected abstract IFeature CreateInternal();
    }
}