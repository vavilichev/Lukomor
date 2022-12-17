using Lukomor.Domain.Signals;

namespace Lukomor.TagsGame.Grid.Application
{
	public interface ICellMovedSignalObserver : ISignalObserver<CellMovedSignal> { }
}