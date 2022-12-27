using Lukomor.Domain.Features;

namespace Lukomor.Domain.Contexts
{
    public interface IFeatureInstaller
    {
        IFeature Create();
    }
}