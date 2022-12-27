using Lukomor.Domain.Signals;

namespace Lukomor.TagsGame.Grid
{
	public interface ICellMovedSignalObserver : ISignalObserver<CellMovedSignal> { }
}