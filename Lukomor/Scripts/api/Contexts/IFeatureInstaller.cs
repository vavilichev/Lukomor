using Lukomor.DI;
using Lukomor.Features;

namespace Lukomor.Domain.Contexts
{
    public interface IFeatureInstaller
    {
        IFeature Create(DIContainer container);
    }
}