using Lukomor.Common.DIContainer;
using Lukomor.Domain.Contexts;
using Lukomor.Domain.Features;
using Lukomor.TagsGame.TagsGrid;
using Lukomor.TagsGame.TagsGrid.Dto;
using UnityEngine;

namespace Lukomor
{
    [CreateAssetMenu(fileName = "GridFeatureInstaller", menuName = "Game/Installers/GridFeatureInstaller")]
    public class GridFeatureInstaller : FeatureInstaller
    {
        public override IFeature Create()
        {
            var repository = new GridRepository();
            var feature = new GridFeature(repository);

            DI.Bind<IGridFeature>(feature);

            return feature;
        }

        public override void Dispose()
        {
            DI.Unbind<IGridFeature>();
        }
    }
}
