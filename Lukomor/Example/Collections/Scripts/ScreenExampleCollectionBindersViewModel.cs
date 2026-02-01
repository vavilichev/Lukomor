using Lukomor.MVVM;
using Lukomor.Reactive;
using UnityEngine;

namespace Lukomor.Example.Collections
{
    public class ScreenExampleCollectionBindersViewModel : ViewModel
    {
        private readonly ReactiveCollection<WidgetBaseViewModel> _widgets = new();
        
        public IReadOnlyReactiveCollection<WidgetBaseViewModel> Widgets => _widgets;
        public ICommand CmdCreateRandomWidget { get; private set; }
        public ICommand CmdDeleteRandomWidget { get; private set; }

        public ScreenExampleCollectionBindersViewModel()
        {
            CmdCreateRandomWidget = new Command(CreateRandomWidget);
            CmdDeleteRandomWidget = new Command(DeleteRandomWidget);
        }

        private void CreateRandomWidget()
        {
            var createText = Random.Range(0, 2) == 0;
            if (createText)
            {
                _widgets.Add(new WidgetTextViewModel());
            }
            else
            {
                _widgets.Add(new WidgetColorViewModel());
            }
        }
        
        private void DeleteRandomWidget()
        {
            if (_widgets.Count == 0)
            {
                return;
            }
            
            var randomWidgetIndex = Random.Range(0, _widgets.Count);
            var randomWidget = _widgets[randomWidgetIndex];
            _widgets.Remove(randomWidget);
        }
    }
}