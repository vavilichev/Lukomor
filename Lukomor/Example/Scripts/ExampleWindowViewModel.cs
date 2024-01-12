using System;
using System.Reactive;
using System.Reactive.Linq;
using Lukomor.MVVM;

namespace Lukomor.Example
{
    public abstract class ExampleWindowViewModel : IViewModel
    {
        public IObservable<Unit> Closed { get; }

        private event Action<Unit> _closedEvent; 

        public ExampleWindowViewModel()
        {
            Closed = Observable.FromEvent<Unit>(
                a => _closedEvent += a,
                a => _closedEvent -= a);
        }
        
        public void Close()
        {
            OnClosed();
            
            _closedEvent?.Invoke(Unit.Default);
        }
        
        protected void OnClosed() { }
    }
}