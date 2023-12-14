using Lukomor.Reactive;

namespace Lukomor
{
    public class ExampleQuestViewModel2 : ExampleQuestViewModelBase
    {
        public ReactiveProperty<string> Text1 { get; } = new();
        public ReactiveProperty<string> Text2 { get; } = new();

        public ExampleQuestViewModel2(string text1, string text2)
        {
            Text1.Value = text1;
            Text2.Value = text2;
        }
    }
}