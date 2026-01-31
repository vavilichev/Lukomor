using System.Collections.Generic;
using UnityEngine;

namespace Lukomor.MVVM.Binders
{
    /// <summary>
    /// This class observes the IReadonlyObservableCollection<IViewModel> and reacts to it's changing.
    /// Adding new ViewModel leads to View creation. Removing ViewModel leads to View destroying.
    /// The view selects from the mapper. The mapper is abstract and has instant prefab pulling, and async.
    /// But exactly this class uses instant variant of the prefab creation.
    /// </summary>
    public class ObservableCollectionToViewBinder : ObservableCollectionBinder<IViewModel>
    {
        [SerializeField] private ViewModelToViewBaseMapper _mapper;
        [SerializeField] private Transform _container;
        
        private readonly Dictionary<IViewModel, View> _createdViews = new();

        protected override void Start()
        {
            _mapper.Init();
            base.Start();
        }

        protected override void OnValueAdded(IViewModel viewModel)
        {
            if (_createdViews.ContainsKey(viewModel))
            {
                return;
            }

            var prefab = _mapper.GetPrefab(viewModel);
            var createdView = Instantiate(prefab, _container);
            
            _createdViews.Add(viewModel, createdView);
            createdView.Bind(viewModel);
        }

        protected override void OnValueRemoved(IViewModel viewModel)
        {
            if (_createdViews.TryGetValue(viewModel, out var view))
            {
                view.Destroy();
                _createdViews.Remove(viewModel);
            }
        }
    }
}