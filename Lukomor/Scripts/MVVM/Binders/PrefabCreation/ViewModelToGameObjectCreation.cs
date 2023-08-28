using System;
using UnityEngine;

namespace Lukomor.MVVM.PrefabCreation
{
    public class ViewModelToGameObjectCreation : ObservableBinder<IViewModel>
    {
        [SerializeField] private View _prefabView;
        
        protected override IDisposable BindInternal(IViewModel viewModel)
        {
            var type = viewModel.GetType();

            if (type.FullName == ViewModelTypeFullName)
            {
                var propertyInfo = type.GetProperty(PropertyName);
                var subViewModel = (IViewModel)propertyInfo.GetValue(viewModel);
                var createdView = Instantiate(_prefabView, transform);
                
                createdView.Bind(subViewModel);
            }

            return null;
        }
    }
}