using System;
using Lukomor.MVVM.Binders;
using UnityEditor;

namespace Lukomor.MVVM.Editor
{
    [CustomEditor(typeof(ObservableBinderDeprecated), true)]
    public class ObservableBinderEditor : ObservableBinderBase
    {
        private ObservableBinderDeprecated _observableBinderDeprecated;
        protected override SerializedProperty _propertyName { get; set; }

        protected override void OnEnable()
        {
            base.OnEnable();

            _observableBinderDeprecated = (ObservableBinderDeprecated)target;
            _propertyName = serializedObject.FindProperty(nameof(_propertyName));
        }

        protected override bool IsValidProperty(Type propertyType)
        {
            var requiredArgumentType = _observableBinderDeprecated.ArgumentType;
            var requiredType = typeof(IObservable<>);

            return IsValidProperty(propertyType, requiredType, requiredArgumentType);
        }
    }
}