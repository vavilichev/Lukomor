using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Lukomor.MVVM.Editor
{
    public static class ViewModelsEditorUtility
    {
        public static void DefineAllViewModels(Dictionary<string, string> viewModelNames)
        {
            viewModelNames.Clear();
            viewModelNames[MVVMConstants.NONE] = null;

            var allViewModelsTypes = TypeCache.GetTypesDerivedFrom<IViewModel>()
                .Where(myType => myType.IsClass && !myType.IsAbstract);
            
            foreach (var viewModelsType in allViewModelsTypes)
            {
                viewModelNames[viewModelsType.Name] = viewModelsType.FullName;
            }
        }
        
        public static string ToShortName(string viewModelTypeFullName, TypeCache.TypeCollection allViewModelTypes)
        {
            var viewModelType = allViewModelTypes.FirstOrDefault(t => t.FullName == viewModelTypeFullName);
            
            return viewModelType == null ? MVVMConstants.NONE : viewModelType.Name;
        }
    }
}