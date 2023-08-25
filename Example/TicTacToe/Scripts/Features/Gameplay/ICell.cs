using System;
using UnityEngine;

namespace Lukomore.Example.TicTacToe.Gameplay
{
    public interface ICell
    {
        event Action<ICell> StateChanged;
        
        CellState Filling { get; }
        Vector2Int Position { get; }

        void Mark(CellState marker);
        void Clear();
    }
}