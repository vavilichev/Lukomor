using System;
using System.Linq;
using System.Reflection;

namespace Lukomor.MVVM.Editor
{
    public static class ViewModelsEditorUtility
    {
        public static string ToShortName(string viewModelTypeFullName)
        {
            var viewModelType = ConvertViewModelType(viewModelTypeFullName);
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
            var viewModelProperties = FilterValidViewModelProperties(allProperties, typeof(IViewModel));
            var result = viewModelProperties.Select(p => p.Name).ToArray();
            return result;
        }

        public static PropertyInfo[] FilterValidViewModelProperties(PropertyInfo[] allProperties, Type type)
        {
            var validProperties = allProperties.Where(p =>
            {
                var propertyType = p.PropertyType;
                if (!propertyType.IsPublic || !propertyType.IsGenericType)
                {
                    return false;
                }

                var isDirectlyObservable = propertyType.GetGenericTypeDefinition() == typeof(IObservable<>);
                if (!isDirectlyObservable)
                {
                    var interfaces = propertyType.GetInterfaces();
                    var isInheritedByObservable =
                        interfaces.FirstOrDefault(i => i.IsGenericType &&
                                                       i.GetGenericTypeDefinition() == typeof(IObservable<>)) != null;
                    if (!isInheritedByObservable)
                    {
                        return false;
                    }
                }

                var genericArgs = propertyType.GetGenericArguments();
                if (genericArgs.Length != 1)
                {
                    return false;
                }

                var genericArgumentType = genericArgs[0];
                var result = type.IsAssignableFrom(genericArgumentType);
                return result;
            }).ToArray();

            return validProperties;
        }

        public static PropertyInfo[] FilterValidProperties(PropertyInfo[] allProperties, Type requiredType)
        {
            var validProperties = allProperties.Where(p =>
            {
                var propertyType = p.PropertyType;
                if (!propertyType.IsPublic)
                {
                    return false;
                }

                var isRequiredType = requiredType.IsAssignableFrom(propertyType);
                return isRequiredType;
            }).ToArray();

            return validProperties;
        }
    }
}