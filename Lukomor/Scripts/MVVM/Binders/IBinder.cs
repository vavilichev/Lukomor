using System;

namespace Lukomor.MVVM
{
    public interface IBinder
    {
        Type ViewModelType { get; }
        void Bind(IViewModel viewModel);
    }
}