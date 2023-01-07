using System;
using Lukomor.Domain.Features;
using UnityEngine;

namespace Lukomor.Domain.Contexts
{
    public abstract class FeatureInstaller : ScriptableObject, IFeatureInstaller, IDisposable
    {
        public abstract IFeature Create();
        public abstract void Dispose();
    }
}