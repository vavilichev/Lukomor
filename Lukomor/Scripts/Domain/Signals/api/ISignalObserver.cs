namespace Lukomor.Domain.Signals
{
	public interface ISignalObserver { }
	
	public interface ISignalObserver<in T> : ISignalObserver where T : ISignal
	{
		void ReceiveSignal(T signal);
	}
}