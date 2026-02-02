
using System;
using System.Reactive.Subjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Lukomor.Example.UICollections
{
    public class WidgetColorViewModel : WidgetBaseViewModel
    {
        private static readonly Color[] _colors =
        {
            UnityEngine.Color.white, 
            UnityEngine.Color.green, 
            UnityEngine.Color.blue, 
            UnityEngine.Color.red,
            UnityEngine.Color.brown
        };

        private readonly BehaviorSubject<Color> _color = new(UnityEngine.Color.white);

        public IObservable<Color> Color => _color;

        public WidgetColorViewModel()
        {
            var rColorIndex = Random.Range(0, _colors.Length);
            _color.OnNext(_colors[rColorIndex]);
        }
    }
}