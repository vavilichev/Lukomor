using System;
using System.Collections.Generic;
using System.Linq;
using Lukomor.Reactive;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Lukomor.MVVM.Editor
{
    [CustomEditor(typeof(ObservableBinder), true)]
    public class ObservableBinderEditor : BinderEditor
    {
        private ObservableBinder _observableBinder;
        private SerializedProperty _propertyName;

        protected override void OnEnable()
        {
            base.OnEnable();

            _observableBinder = (ObservableBinder)target;
            _propertyName = serializedObject.FindProperty(nameof(_propertyName));
        }

        protected override void DrawProperties()
        {
            DrawPropertyNames();
        }

        private void DrawPropertyNames()
        {
            var viewModelType = Type.GetType(ViewModelTypeFullName.stringValue);
            var allProperties = viewModelType.GetProperties().Where(p => p.PropertyType.IsGenericType);
            var validProperties = allProperties.Where(p => IsValidProperty(p.PropertyType));
            var validPropertyNames = validProperties.Select(p => p.Name);
            var provider = CreateInstance<StringListSearchProvider>();
            var options = new List<string> { NONE };
            options.AddRange(validPropertyNames);
            
            provider.Init(options.ToArray(), result =>
            {
                _propertyName.stringValue = result == NONE ? null : result;

                serializedObject.ApplyModifiedProperties();
            });
            
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("PropertyName:");

            var displayName = string.IsNullOrEmpty(_propertyName.stringValue)
                ? NONE
                : _propertyName.stringValue;
            
            if (GUILayout.Button(displayName, EditorStyles.popup))
            {
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), provider);
            }
            
            EditorGUILayout.EndHorizontal();
        }

        private bool IsValidProperty(Type propertyType)
        {
            var genericArgument = propertyType.GetGenericArguments().First();
            var requiredArgument = _observableBinder.ArgumentType;

            if (genericArgument != requiredArgument)
            {
                return false;
            }
            
            var requiredObservableType = typeof(IObservable<>);
            var requiredReactiveCollectionType = typeof(IReactiveCollection<>);
            var interfaceTypes = propertyType.GetInterfaces().Where(i => i.IsGenericType);

            foreach (var interfaceType in interfaceTypes)
            {
                if (requiredObservableType.IsAssignableFrom(interfaceType.GetGenericTypeDefinition()) ||
                    requiredReactiveCollectionType.IsAssignableFrom(interfaceType.GetGenericTypeDefinition()))
                {
                    return true;
                }
            }

            return false;
        }
    }
}