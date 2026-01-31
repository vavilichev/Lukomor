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
        /// <summary>
        /// This method uses by MVVMValidator to understand, is it necessary
        /// to draw warning exclamation mark nearby the GameObject in the hierarchy or not.
        /// Different type of binders may have different approaches to understand is it broken or not
        /// </summary>
        /// <returns></returns>
        public abstract bool IsBroken();

        /// <summary>
        /// This method uses in cases when someone "above" gets the broken links, and it's affected
        /// the current binder. Each binder type may handle this method different.
        /// </summary>
        public abstract void SmartReset();

        /// <summary>
        /// This method just checks the Source View and it's ViewModel. If Source View is absent or
        /// it's ViewModel is invalid this method returns true
        /// </summary>
        /// <param name="sourceViewModelPropertyName"></param>
        /// <param name="sourceViewModelType"></param>
        /// <returns></returns>
        protected bool IsBrokenBasic(string sourceViewModelPropertyName, out Type sourceViewModelType)
        {
            sourceViewModelType = null;
            
            if (SourceView == null)
            {
                return true;    // source view must be selected
            }
            
            if (string.IsNullOrEmpty(sourceViewModelPropertyName))
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