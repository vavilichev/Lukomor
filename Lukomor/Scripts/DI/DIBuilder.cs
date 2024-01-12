namespace Lukomor.DI
{
    public sealed class DIBuilder<T>
    {
        private readonly DIEntry<T> _entry;

        public DIBuilder(DIEntry<T> entry)
        {
            _entry = entry;
        }
        
        public T CreateInstance()
        {
            return _entry.Resolve();
        }
    }
}