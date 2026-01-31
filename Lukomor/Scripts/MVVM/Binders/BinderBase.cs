using System;
using System.Reactive.Disposables;
using UnityEngine;

namespace Lukomor.MVVM.Binders
{
    public abstract class BinderBase : MonoBehaviour
    {
        [SerializeField] private View _sourceView;

        public View SourceView => _sourceView;

        protected readonly CompositeDisposable Subscriptions = new();

        protected virtual void OnDestroy()
        {
            Unsubscribe();
        }

        protected void Unsubscribe()
        {
            Subscriptions.Dispose();
        }

#if UNITY_EDITOR
        public abstract bool IsBroken();

        public abstract void SmartReset();

        protected bool IsBrokenBasic(string propertyName, out Type sourceViewModelType)
        {
            sourceViewModelType = null;
            
            if (SourceView == null)
            {
                return true;    // source view must be selected
            }
            
            if (string.IsNullOrEmpty(propertyName))
            {
                return true;    // command property must be selected
            }

            sourceViewModelType = Editor.ViewModelsEditorUtility.ConvertViewModelType(SourceView.ViewModelTypeFullName);
            
            if (sourceViewModelType == null)
            {
                return true;    // parent view is broken
            }

            return false;
        }
#endif
    }
}