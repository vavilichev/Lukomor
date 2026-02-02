using System;
using System.Reactive.Subjects;

namespace Lukomor.Example.UICollections
{
    public class WidgetTextViewModel : WidgetBaseViewModel
    {
        private static readonly string[] _texts =
        {
            "Example 1", 
            "Example 2", 
            "Example 3", 
            "Example 4", 
            "Example 5", 
        };

        private readonly BehaviorSubject<string> _text = new(string.Empty);

        public IObservable<string> Text => _text;

        public WidgetTextViewModel()
        {
            var rColorIndex = UnityEngine.Random.Range(0, _texts.Length);
            _text.OnNext(_texts[rColorIndex]);
        }
    }
}