using System.Threading.Tasks;

namespace Lukomor.MVVM.Binders
{
    public interface IViewModelToViewMapper
    {
        public void Init();
        public View GetPrefab(IViewModel viewModel);
        public Task<View> GetPrefabAsync(IViewModel viewModel);
    }
}