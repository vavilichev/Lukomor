using System;
using System.Reactive;
using System.Reactive.Linq;
using Lukomor.MVVM;

namespace Lukomor
{
    public abstract class WindowViewModel : IViewModel
    {
        public IObservable<Unit> Closed { get; }

        private event Action<Unit> _closedEvent; 

        public WindowViewModel()
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