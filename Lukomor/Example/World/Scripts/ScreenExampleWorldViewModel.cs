using Lukomor.MVVM;
using Lukomor.Reactive;

namespace Lukomor.Example.World
{
    public class ScreenExampleWorldViewModel : ViewModel
    {
    private readonly ObjectsService _objectsService;

    public IReadOnlyReactiveCollection<ObjectViewModel> Objects { get; }
    public ICommand CmdCreateObject { get; }
    public ICommand CmdDestroyObject { get; }

    public ScreenExampleWorldViewModel(ObjectsService objectsService)
    {
        _objectsService = objectsService;

        Objects = objectsService.Objects;
        CmdCreateObject = new Command(CreateObject);
        CmdDestroyObject = new Command(DestroyObject);
    }

    private void CreateObject()
    {
        _objectsService.CreateRandomObject();
    }

    private void DestroyObject()
    {
        _objectsService.DestroyRandomObject();
    }
    }
}