using Lukomor.Scenes;
using Lukomor.UI.Views.Windows;

namespace Lukomor.TagsGame.Preview.Presentation.UI
{
    public class WindowPreviewViewModel : WindowViewModel
    {
        private ISceneManager sceneManager;

       protected override void OnConstructed()
        {
            base.OnConstructed();

            sceneManager = DiContainer.Get<ISceneManager>();
        }

        public void RequestContinue()
        {
            sceneManager.LoadScene(2);
        }
    }
}