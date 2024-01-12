using Lukomor.Reactive;

namespace Lukomor.Example
{
    public class ExampleQuestViewModel1 : ExampleQuestViewModelBase
    {
        public ReactiveProperty<string> Text { get; } = new();

        public ExampleQuestViewModel1(string text)
        {
            Text.Value = text;
        }
    }
}