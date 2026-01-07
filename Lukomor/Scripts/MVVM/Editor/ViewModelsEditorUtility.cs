using System.Collections.Generic;
using System.Linq;

namespace Lukomor.MVVM.Editor
{
    public static class ViewModelsEditorUtility
    {
        public static void DefineAllViewModels(Dictionary<string, string> viewModelNames)
        {
            viewModelNames.Clear();

            var allViewModelsTypes = ViewModelsDB.AllViewModelTypes.Where(myType => myType.IsClass && !myType.IsAbstract);
            foreach (var viewModelsType in allViewModelsTypes)
            {
                viewModelNames[viewModelsType.Name] = viewModelsType.FullName;
            }
        }
        
        public static string ToShortName(string viewModelTypeFullName)
        {
            var allViewModelTypes = ViewModelsDB.AllViewModelTypes;
            var viewModelType = allViewModelTypes.FirstOrDefault(t => t.FullName == viewModelTypeFullName);
            
            return viewModelType == null ? MVVMConstants.NONE : viewModelType.Name;
        }
    }
}