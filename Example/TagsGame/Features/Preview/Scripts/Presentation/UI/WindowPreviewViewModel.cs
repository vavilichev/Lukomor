using Lukomor.Common.DIContainer;
using Lukomor.Domain.Scenes;
using Lukomor.Presentation.Views.Windows;

namespace Lukomor.TagsGame.Preview.Presentation.UI
{
    public class WindowPreviewViewModel : WindowViewModel
    {
        private DIVar<ISceneManager> _sceneManager = new DIVar<ISceneManager>();
        
        public void RequestContinue()
        {
            _sceneManager.Value.LoadScene(2);
        }
    }
}