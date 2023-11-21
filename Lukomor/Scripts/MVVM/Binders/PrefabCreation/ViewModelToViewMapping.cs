using System;

namespace Lukomor.MVVM.PrefabCreation
{
    [Serializable]
    public class ViewModelToViewMapping
    {
        public string ViewModelTypeFullName;
        public View PrefabView;
    }
}