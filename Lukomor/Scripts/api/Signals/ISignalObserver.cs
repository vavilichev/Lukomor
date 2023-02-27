namespace Lukomor.Signals
{
	public interface ISignalObserver
	{
		void Subscribe();
		void Unsubscribe();
	}
	
	public interface ISignalObserver<in T> : ISignalObserver where T : ISignal
	{
		void ReceiveSignal(T signal);
	}
}