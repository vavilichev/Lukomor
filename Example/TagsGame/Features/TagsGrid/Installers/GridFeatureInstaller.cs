using Lukomor.Contexts;
using Lukomor.Domain.Contexts;
using Lukomor.Features;
using Lukomor.TagsGame.TagsGrid;
using Lukomor.TagsGame.TagsGrid.Dto;
using UnityEngine;

namespace Lukomor
{
    [CreateAssetMenu(fileName = "GridFeatureInstaller", menuName = "Game/Installers/GridFeatureInstaller")]
    public class GridFeatureInstaller : FeatureInstaller
    {
        protected override IFeature CreateInternal()
        {
            var repository = new GridRepository();
            var feature = new GridFeature(Container, repository);

            Container.Bind<IGridFeature>(feature);

            return feature;
        }

        public override void Dispose()
        {
            Container.Unbind<IGridFeature>();
        }
    }
}
