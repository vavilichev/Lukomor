using System;
using UnityEngine;

namespace Lukomore.Example.TicTacToe.Gameplay
{
    public class Cell : ICell
    {
        public event Action<ICell> StateChanged;

        public CellState Filling { get; private set; }
        public Vector2Int Position { get; }

        public Cell(Vector2Int position)
        {
            Position = position;
            
            SetState(CellState.Empty);
        }

        public void Mark(CellState marker)
        {
            if (Filling != CellState.Empty)
            {
                return;
            }

            SetState(marker);
        }

        public void Clear()
        {
            SetState(CellState.Empty);
        }

        private void SetState(CellState newState)
        {
            Filling = newState;

            StateChanged?.Invoke(this);
        }
    }
}