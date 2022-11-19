using System;
using Lukomor.Presentation.Common;

namespace Lukomor.Presentation.Views.Windows
{
    [Serializable]
    public struct WindowSettings
    {
        public UILayer TargetLayer;
        public bool IsPreCached;
        public bool OpenWhenCreated;
    }
}