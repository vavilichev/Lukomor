using Lukomor.Domain.Signals;

namespace Lukomor.TagsGame.TagsGrid.Signals
{
	public interface ICellMovedSignalObserver : ISignalObserver<CellMovedSignal> { }
}