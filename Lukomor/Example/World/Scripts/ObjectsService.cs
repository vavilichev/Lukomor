using Lukomor.Reactive;
using UnityEngine;

namespace Lukomor.Example.World
{
    public class ObjectsService
    {
        private readonly ReactiveCollection<ObjectViewModel> _objects = new();
        
        public IReadOnlyReactiveCollection<ObjectViewModel> Objects => _objects; 
        
        public void CreateRandomObject()
        {
            var isBouncing = Random.Range(0, 2) == 0;
            if (isBouncing)
            {
                var vm = new BouncingObjectViewModel();
                _objects.Add(vm);
            }
            else
            {
                var vm = new WanderingObjectViewModel();
                _objects.Add(vm);
            }
        }

        public void DestroyRandomObject()
        {
            var rIndex = Random.Range(0, _objects.Count);
            var vm = _objects[rIndex];
            _objects.Remove(vm);
        }
    }
}