using System.Collections.Generic;
using Lukomor.Reactive;
using UnityEngine;

namespace Lukomor.MVVM
{
    public class TestSubViewModel : IViewModel
    {
        public SingleReactiveProperty<string> SomeText { get; } = new("OLOLOLLO");
        public SingleReactiveProperty<int> SomeInt { get; } = new(1923);
        public SingleReactiveProperty<Sprite> SomeSprite { get; } = new();
    }
}