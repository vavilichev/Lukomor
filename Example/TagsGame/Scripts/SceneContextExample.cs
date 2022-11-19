using Lukomor.Domain.Contexts;
using Lukomor.TagsGame.Grid.Domain;

namespace Lukomor.TagsGame
{
    public class SceneContextExample : SceneContext
    {
        protected override void InstallServices() { }

        protected override void InstallFeatures()
        {
            AddFeature(new TagsGridFeature());
        }
    }
}