using System.Threading.Tasks;
using Lukomor.Application.Contexts;
using Lukomor.DIContainer;
using Lukomor.Domain.Scenes;
using Lukomor.Example.Application;
using Lukomor.Presentation;

namespace Lukomor.Example.Domain
{
    public class TagsGame
    {
        public static IContext Context { get; }
        public static ISceneManager SceneManager { get; }

        static TagsGame()
        {
            Context = new TagsGameContextMain();
            
            DI.Bind(UserInterface.CreateInstance());
            
            SceneManager = Lukomor.Domain.Scenes.SceneManager.CreateInstance();
        }

        public static async Task StartGameAsync()
        {
            await Context.Initialize();
            await SceneManager.LoadScene(1);
        }
    }
}