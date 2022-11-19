using Lukomor.Common.DIContainer;
using Lukomor.Domain.Signals;
using Lukomor.TagsGame.Grid.Application;

namespace Lukomor.TagsGame.Grid.Domain
{
	public class ReloadTagsFeatureInteractor : IReloadTagsFeatureInteractor
	{
		private readonly DIVar<ISignalTower> _signalTower = new DIVar<ISignalTower>();
		private readonly IGetTagsGridDataInteractor _getTagsGridData;
		private readonly TagsGridFeature _feature;

		public ReloadTagsFeatureInteractor(TagsGridFeature feature, IGetTagsGridDataInteractor getTagsGridData)
		{
			_feature = feature;
			_getTagsGridData = getTagsGridData;
		}

		public void Execute()
		{
			_signalTower.Value.FireSignal(new TagsGridRebuildStartSignal());

			var grid = _getTagsGridData.Execute();
			
			grid.Randomize();
			
			_feature.Save();

			_signalTower.Value.FireSignal(new TagsGridRebuiltSignal());
		}
	}
}