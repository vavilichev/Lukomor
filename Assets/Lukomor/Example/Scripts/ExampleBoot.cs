using Lukomor.Example.Domain;
using UnityEngine;
using VavilichevGD.Tools.Async;

namespace Lukomor.Example
{
    public class ExampleBoot : MonoBehaviour
    {
        private void Start() {
            TagsGame.StartGameAsync().RunAsync();
        }
    }
}
