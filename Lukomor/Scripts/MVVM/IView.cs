using System;
using UnityEngine;

namespace Lukomor.MVVM
{
    public interface IView
    {
        Type ViewModelType { get; }
        GameObject gameObject { get; }
    }
}