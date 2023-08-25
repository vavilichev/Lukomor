using System;
using UnityEngine;

namespace Lukomore.Example.TicTacToe.Gameplay
{
    public class Field
    {
        public event Action<ICell> CellStateChanged; 

        public int Width { get; }
        public int Height { get; }
        
        private Cell[,] _field;

        public Field(int width, int height)
        {
            Width = width;
            Height = height;

            _field = new Cell[width, height];

            for (var i = 0; i < Height; i++)
            {
                for (var j = 0; j < Width; j++)
                {
                    var cell = new Cell(new Vector2Int(i, j));
                    _field[i, j] = cell;
                    
                    cell.StateChanged += OnCellStateChanged;
                }
            }
        }

        public ICell GetCell(Vector2Int position)
        {
            return _field[position.x, position.y];
        }

        private void OnCellStateChanged(ICell cell)
        {
            CellStateChanged?.Invoke(cell);
        }

        public void Reset()
        {
            for (var i = 0; i < Height; i++)
            {
                for (var j = 0; j < Width; j++)
                {
                    _field[i, j].Clear();
                }
            }
        }
    }
}