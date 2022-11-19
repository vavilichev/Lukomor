namespace Lukomor.Domain.Signals
{
	public interface ISignalTower
	{
		void Register<TSignal>(ISignalObserver<TSignal> observer) where TSignal : ISignal;
		void Unregister<TSignal>(ISignalObserver<TSignal> observer) where TSignal : ISignal;
		void FireSignal<TSignal>(TSignal signal) where TSignal : ISignal;
		void Clear();
	}
}