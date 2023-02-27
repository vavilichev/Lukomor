using System;
using Lukomor.UI.Common;

namespace Lukomor.UI
{
    [Serializable]
    public struct WindowSettings
    {
        public UILayer TargetLayer;
        public bool IsPreCached;
        public bool OpenWhenCreated;
    }
}