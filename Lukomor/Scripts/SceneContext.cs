using Lukomor.DI;

namespace Lukomor
{
    public class SceneContext : Context
    {
        protected override IDIContainer CreateLocalContainer()
        {
            var rootContainer = ProjectContext.Instance.Container;
            
            return new DIContainer(rootContainer);
        }

        private void Start()
        {
            Initialize();
        }
    }
}