using System;

namespace Lukomor.Features.Scenes
{
    public interface ILoadingScreen
    {
        void Show(Action onComplete = null);
        void Hide(Action onComplete = null);
    }
}