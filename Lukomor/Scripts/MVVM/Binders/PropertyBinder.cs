using System;
using System.Reactive.Disposables;
using Lukomor.Reactive;
using UnityEngine;

namespace Lukomor.MVVM
{
    public abstract class PropertyBinder : MonoBehaviour, IBinder
    {
        // todo: write subscriber that will cache callbacks and when this item will be destroyed subscriber will dispose subsctibtions

        [SerializeField] private string _viewModelPath;
        
        [SerializeField] protected string _propertyName;

        public abstract Type GetGenericArgumentType();

        public Type ViewModelType
        {
            get
            {
                if (_viewModelType == null)
                {
                    _viewModelType = Type.GetType(_viewModelPath);
                }

                return _viewModelType;
            }
        }

        private Type _viewModelType;

        public void Bind(IViewModel viewModel)
        {
            if (string.IsNullOrEmpty(_propertyName))
            {
                throw new Exception($"Binder property is null or empty. GameObject: {gameObject.name}");
            }
            
            var parentView = GetComponentInParent<IView>(true);
            var parentViewModelType = parentView.ViewModelType;

            if (viewModel.GetType() == parentViewModelType)
            {
                BindInternal(viewModel);
            }
        }

        protected abstract void BindInternal(IViewModel viewModel);

        protected static void TakeViewModelToChildBinders(GameObject go, IViewModel viewModel)
        {
            var allBinders = go.GetComponentsInChildren<IBinder>(true);

            foreach (var binder in allBinders)
            {
                binder.Bind(viewModel);
            }
        }
    }

    public abstract class BinderObsolete<T> : PropertyBinder
    {
        public override Type GetGenericArgumentType()
        {
            return typeof(T);
        }

        protected void BindLikeElement(string propertyName, IViewModel viewModel, Action<T> callback)
        {
            var property = viewModel.GetType().GetProperty(propertyName);
            var reactiveProperty = (IReactiveProperty<T>)property.GetValue(viewModel);
        }


        protected IDisposable BindLikeCollection(string propertyName, IViewModel viewModel, Action<T> addedCallback, Action<T> removedCallback)
        {
            var propertyInfo = viewModel.GetType().GetProperty(propertyName);
            var reactiveCollection = (IReactiveCollection<T>)propertyInfo.GetValue(viewModel);

            var addedSubscription = reactiveCollection.Added.Subscribe(addedCallback);
            var removedSubscription = reactiveCollection.Removed.Subscribe(removedCallback);
            var compositeDisposable = new CompositeDisposable();
            
            compositeDisposable.Add(addedSubscription);
            compositeDisposable.Add(removedSubscription);

            return compositeDisposable;
        }

        protected virtual void OnDestroy()
        {
            // this.DisposeAdded();
            // subscriber.Unsubscribe();
        }
    }
}