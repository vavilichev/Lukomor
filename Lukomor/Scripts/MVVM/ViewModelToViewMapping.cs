using System;

namespace Lukomor.MVVM
{
    [Serializable]
    public class ViewModelToViewMapping
    {
        public string ViewModelTypeFullName;
        public View PrefabView;
    }
}