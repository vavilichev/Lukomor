using Lukomor.DI;

namespace Lukomor
{
    public interface IInstaller
    {
        void InstallBindings(IDIContainer localContainer);
    }
}