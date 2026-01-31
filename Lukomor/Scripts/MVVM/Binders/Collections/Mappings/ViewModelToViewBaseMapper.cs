using System.Threading.Tasks;
using UnityEngine;

namespace Lukomor.MVVM.Binders
{
    public abstract class ViewModelToViewBaseMapper : MonoBehaviour, IViewModelToViewMapper
    {
        public abstract void Init();
        public abstract View GetPrefab(IViewModel viewModel);
        public abstract Task<View> GetPrefabAsync(IViewModel viewModel);
    }
}