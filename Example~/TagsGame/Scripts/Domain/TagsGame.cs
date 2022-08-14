using System.Collections.Generic;
using System.Threading.Tasks;
using Lukomor.Application.Contexts;
using Lukomor.Application.Signals;
using Lukomor.DIContainer;
using Lukomor.Domain.Scenes;
using Lukomor.Example.Application;
using Lukomor.Presentation;

namespace Lukomor.Example
{
    public class TagsGame
    {
        public static ProjectContext ProjectContext { get; }
        public static ISceneManager SceneManager { get; }

        static TagsGame()
        {
            Dictionary<string, IContext> scenesContextMap = new Dictionary<string, IContext>
            {
                {"LukomorExample_Gameplay", new TagsGameGameplaySceneContext()}
            };
            
            ProjectContext = new TagsGameProjectContext(scenesContextMap);
            
            DI.Bind(UserInterface.CreateInstance());
            
            SceneManager = Lukomor.Domain.Scenes.SceneManager.CreateInstance(ProjectContext);
        }

        public static async Task StartGameAsync()
        {
            await ProjectContext.InitializeAsync();
            await SceneManager.LoadScene("LukomorExample_Gameplay");
        }
    }
}