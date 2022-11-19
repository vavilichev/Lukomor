using System;
using System.Threading.Tasks;
using Lukomor.Application.Scenes;
using Lukomor.Application.Signals;
using Lukomor.Common.DIContainer;
using Lukomor.Domain.Contexts;
using Lukomor.Domain.Signals;
using Object = UnityEngine.Object;

namespace Lukomor.Application
{
    public static class Game
    {
        public static event Action ProjectContextPreInitialized;
        public static event Action ProjectContextInitialized;
        public static event Action GameStarted;
        
        public static bool IsStarted { get; private set; }
        public static bool IsMainObjectsBound { get; private set; }
        public static bool IsProjectContextInitialized { get; private set; }
        
        private static ProjectContext _projectContext { get; set; }
        private static bool _gameStarting { get; set; }

        public static async Task StartGameAsync(ProjectContext projectContext)
        {
            if (!IsStarted && !_gameStarting)
            {
                IsMainObjectsBound = false;
                IsProjectContextInitialized = false;
                
                _gameStarting = true;
                _projectContext = projectContext;

                var ui = Object.Instantiate(projectContext.UserInterfacePrefab);
                var sceneManager = SceneManager.CreateInstance(_projectContext, ui);
                var signalTower = new SignalTower();

                DI.Bind<ISignalTower>(signalTower);            
                DI.Bind(ui);
                DI.Bind(sceneManager);

                IsMainObjectsBound = true;
                ProjectContextPreInitialized?.Invoke();

                if (projectContext != null)
                {
                    await _projectContext.InitializeAsync();
                }

                IsProjectContextInitialized = true;
                ProjectContextInitialized?.Invoke();

                await sceneManager.LoadScene(1);

                IsStarted = true;
                _gameStarting = false;
                
                GameStarted?.Invoke();
            }
        }
    }
}