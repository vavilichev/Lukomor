using System;
using System.Linq;
using Lukomor.MVVM.Binders;
using UnityEditor;

namespace Lukomor.MVVM.Editor
{
    [CustomEditor(typeof(Binder), true)]
    public abstract class BinderEditor : UnityEditor.Editor
    {
        protected const string NONE = "None";
        
        private Binder _binder;
        private View _parentView;
        private SerializedProperty _viewModelTypeFullName;

        protected SerializedProperty ViewModelTypeFullName => _viewModelTypeFullName;

        protected virtual void OnEnable()
        {
            _binder = (Binder)target;
            _parentView = _binder.GetComponentInParent<View>(true);

            _viewModelTypeFullName = serializedObject.FindProperty(nameof(_viewModelTypeFullName));
        }

        public sealed override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            _viewModelTypeFullName.stringValue = _parentView.ViewModelTypeFullName;

            if (string.IsNullOrWhiteSpace(_viewModelTypeFullName.stringValue))
            {
                EditorGUILayout.HelpBox("There is no view model setup on the View. Please check View setup.", MessageType.Warning);
                return;
            }
            
            DrawProperties();
        }

        protected abstract void DrawProperties();
        
        protected static bool IsValidProperty(Type propertyType, Type requiredType, Type requiredArgumentType)
        {
            var genericArgument = propertyType.GetGenericArguments().First();

            if (!requiredArgumentType.IsAssignableFrom(genericArgument))
            {
                return false;
            }
            
            var interfaceTypes = propertyType.GetInterfaces().Where(i => i.IsGenericType);

            if (requiredType.IsAssignableFrom(propertyType.GetGenericTypeDefinition()))
            {
                return true;
            }

            foreach (var interfaceType in interfaceTypes)
            {
                if (requiredType.IsAssignableFrom(interfaceType.GetGenericTypeDefinition()))
                {
                    return true;
                }
            }

            return false;
        }
    }
}