using Lukomor.Reactive;
using UnityEngine;

namespace Lukomor.MVVM
{
    public class TestSubViewModel : IViewModel
    {
        public SingleReactiveProperty<string> SomeText { get; } = new("OLOLOLLO");
        public SingleReactiveProperty<int> SomeInt { get; } = new(1923);
        public SingleReactiveProperty<Sprite> SomeSprite { get; } = new();

        public void TestEmpty()
        {
            Debug.Log("Test empty method invoked");
        }

        public void TestInteger(int value)
        {
            Debug.Log($"Test integer method invoked: {value}");
        }

        public void TestFloat(float value)
        {
            Debug.Log($"Test float method invoked: {value}");
        }

        public int GetInteger()
        {
            return 0;
        }
    }
}