using System;
using System.Threading.Tasks;
using Lukomor.Contexts;
using Lukomor.DI;
using Lukomor.Domain.Contexts;
using Lukomor.Presentation;
using Lukomor.Scenes;
using Lukomor.Signals;
using Lukomor.UI;

namespace Lukomor
{
    public static class Game
    {
        public static event Action GameStarted;
        
        public static bool IsStarted { get; private set; }

        private static bool isGameStarting { get; set; }
        private static ProjectContext ProjectContext { get; set; }
        private static DiContainer DiContainer { get; set; }
        private static ISceneManager SceneManager { get; set; }

        public static async Task StartGameAsync(ProjectContext projectContext, Action callback = null)
        {
            if (!IsStarted && !isGameStarting)
            {
                isGameStarting = true;
                ProjectContext = projectContext;
                
                InitDiContainer();
                InitSignalTower();
                InitUI(projectContext.UserInterfacePrefab, DiContainer);
                InitSceneManager();
                
                await ProjectContext.InitializeAsync(DiContainer);
                await SceneManager.LoadScene(1);

                IsStarted = true;
                isGameStarting = false;
                
                callback?.Invoke();
                GameStarted?.Invoke();
            }
        }

        private static void InitDiContainer()
        {
            DiContainer = new DiContainer();
        }

        private static void InitSignalTower()
        {
            var signalTower = new SignalTower();

            DiContainer.Bind(signalTower);
        }

        private static void InitUI(UserInterface userInterfacePrefab, DiContainer diContainer)
        {
            var ui = UserInterface.CreateInstance(userInterfacePrefab, diContainer);

            DiContainer.Bind(ui);
        }

        private static void InitSceneManager()
        {
            SceneManager = new SceneManager(DiContainer);
            
            DiContainer.Bind(SceneManager);
        }
    }
}