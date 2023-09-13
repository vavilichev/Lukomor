using System;
using System.Data;
using UnityEditor;
using UnityEngine;

namespace Lukomor.MVVM.Editor
{
    [CustomEditor(typeof(Binder), true)]
    public class BinderEditor : UnityEditor.Editor
    {
        private Binder _binder;
        private View _parentView;
        private SerializedProperty _viewModelTypeFullName;
        private SerializedProperty _propertyName;

        private void OnEnable()
        {
            _binder = (Binder)target;
            _parentView = _binder.GetComponentInParent<View>();
        }
    }
}