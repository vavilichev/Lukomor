using Lukomor.Domain.Signals;

namespace Lukomor.TagsGame.Grid.Application
{
	public interface ITagsGridRebuildStartSignalObserver : ISignalObserver<TagsGridRebuildStartSignal> { }
}