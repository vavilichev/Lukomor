using Lukomor.MVVM;
using Lukomor.Reactive;

namespace Lukomor.Example.World
{
    public class WorldRootViewModel : ViewModel
    {
        private readonly ObjectsService _objectsService;
        
        public IReadOnlyReactiveCollection<ObjectViewModel> Objects { get; }

        public WorldRootViewModel(ObjectsService objectsService)
        {
            Objects = objectsService.Objects;
        }
    }
}