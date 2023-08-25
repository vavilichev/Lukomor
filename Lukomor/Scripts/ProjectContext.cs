using Lukomor.DI;

namespace Lukomor
{
    public class ProjectContext : Context
    {
        public static ProjectContext Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            
            DontDestroyOnLoad(gameObject);
            Initialize();
        }

        protected override IDIContainer CreateLocalContainer()
        {
            return new DIContainer();
        }
    }
}