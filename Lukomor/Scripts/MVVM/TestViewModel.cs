using Lukomor.Reactive;
using UnityEngine;

namespace Lukomor.MVVM
{
    public class TestViewModel : IViewModel
    {
        public IReactiveProperty<int> IntProperty { get; }
        public IReactiveProperty<IViewModel> ViewModelProperty { get; }

        public void TestMethod()
        {
            Debug.Log("Test");
        }
    }
}