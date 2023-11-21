using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Lukomor.MVVM.Editor
{
    public static class ViewModelsEditorUtility
    {
        private const string NONE = "None";
        
        public static void DefineAllViewModels(Dictionary<string, string> viewModelNames)
        {
            viewModelNames.Clear();
            viewModelNames[NONE] = null;

            var allViewModelsTypes = TypeCache.GetTypesDerivedFrom<IViewModel>()
                .Where(myType => myType.IsClass && !myType.IsAbstract);
            
            foreach (var viewModelsType in allViewModelsTypes)
            {
                viewModelNames[viewModelsType.Name] = viewModelsType.FullName;
            }
        }
        
        public static string ToShortName(string viewModelTypeFullName)
        {
            var viewModelType = Type.GetType(viewModelTypeFullName);
            
            return viewModelType == null ? NONE : viewModelType.Name;
        }
    }
}