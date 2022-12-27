using Lukomor.Domain.Features;
using UnityEngine;

namespace Lukomor.Domain.Contexts
{
    public abstract class FeatureInstaller : ScriptableObject, IFeatureInstaller
    {
        public abstract IFeature Create();
    }
}