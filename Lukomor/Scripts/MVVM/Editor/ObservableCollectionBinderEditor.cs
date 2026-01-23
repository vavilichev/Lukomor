using System;
using Lukomor.MVVM.Binders;
using Lukomor.Reactive;
using UnityEditor;

namespace Lukomor.MVVM.Editor
{
    [CustomEditor(typeof(ObservableCollectionBinderDeprecated), true)]
    public class ObservableCollectionBinderEditor : ObservableBinderBase
    {
        private ObservableCollectionBinderDeprecated _observableBinderDeprecated;
        protected override SerializedProperty _propertyName { get; set; }

        protected override void OnEnable()
        {
            base.OnEnable();

            _observableBinderDeprecated = (ObservableCollectionBinderDeprecated)target;
            _propertyName = serializedObject.FindProperty(nameof(_propertyName));
        }

        protected override bool IsValidProperty(Type propertyType)
        {
            var requiredArgumentType = _observableBinderDeprecated.ArgumentType;
            var requiredType = typeof(IReadOnlyReactiveCollection<>);

            return IsValidProperty(propertyType, requiredType, requiredArgumentType);
        }
    }
}