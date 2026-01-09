using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

        public static Type ConvertViewModelType(string viewModelTypeFullName)
        {
            var type = ViewModelsDB.AllViewModelTypes.FirstOrDefault(t => t.FullName == viewModelTypeFullName);
            return type;
        }

        public static string[] GetAllValidViewModelPropertyNames(Type type)
        {
            var allProperties = type.GetProperties();
            var allValidProperties = allProperties.Where(p =>
            {
                var propertyType = p.PropertyType;
                if (!propertyType.IsPublic || !propertyType.IsGenericType)
                {
                    return false;
                }

                var isObservable = propertyType
                                   .GetInterfaces()
                                   .FirstOrDefault(i =>
                                                       i.IsGenericType &&
                                                       i.GetGenericTypeDefinition() == typeof(IObservable<>)
                                                  ) != null;

                if (!isObservable)
                {
                    return false;
                }

                var genericArguments = propertyType.GetGenericArguments();
                if (genericArguments.Length != 1)
                {
                    return false;
                }

                var genericArgument = genericArguments[0];
                var isViewModel = typeof(IViewModel).IsAssignableFrom(genericArgument);
                if (!isViewModel)
                {
                    return false;
                }

                return true;
            });

            var result = allValidProperties.Select(p => p.Name).ToArray();
            return result;
        }
    }
}