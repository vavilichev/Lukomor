using System.Reactive.Subjects;

namespace Lukomor.Reactive
{
    // TODO: Implement
    public static class ReactivePool
    {
        public static BehaviorSubject<T> CreateSubject<T>(T valueByDefault = default)
        {
            // TODO: Implement pool
            return new BehaviorSubject<T>(valueByDefault);
        }

        public static ReactiveCollection<T> CreateReactiveCollection<T>()
        {
            // TODO: Implement pool
            return new ReactiveCollection<T>();
        }

        public static void Release<T>(BehaviorSubject<T> subject)
        {
            // TODO: Implement pool
        }
    }
}