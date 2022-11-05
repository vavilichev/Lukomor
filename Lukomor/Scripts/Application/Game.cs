using System.Threading.Tasks;
using Lukomor.Application.Contexts;
using Lukomor.Application.Signals;
using Lukomor.DIContainer;
using Lukomor.Domain.Scenes;
using Lukomor.Presentation;

namespace Lukomor.Application
{
    public static class Game
    {
        public static bool GameStarted { get; private set; }
        
        private static IContext _projectContext { get; set; }
        private static bool _gameStarting { get; set; }

        public static async Task StartGameAsync(IContext projectContext)
        {
            if (!GameStarted && !_gameStarting)
            {
                _gameStarting = true;
                _projectContext = projectContext;
                
                var ui = UserInterface.CreateInstance();
                var sceneManager = SceneManager.CreateInstance(_projectContext, ui);
                var signalTower = new SignalTower();

                DI.Bind<ISignalTower>(signalTower);            
                DI.Bind(ui);
                DI.Bind(sceneManager);

                if (projectContext != null)
                {
                    await _projectContext.InitializeAsync();
                }

                await sceneManager.LoadScene(1);

                GameStarted = true;
                _gameStarting = false;
            }
        }
    }
}