using System.Collections.Generic;
using System.Linq;
using Lukomor.TagsGame.TagsGrid.Data;
using UnityEngine;

namespace Lukomor.TagsGame.TagsGrid
{
	public class GameGrid : IGrid
	{
		public ICell[] Cells { get; }
		public int Size => _dto.Size;


		private IGridSaveData _dto;
		

		public GameGrid(IGridSaveData dto, ICell[] cells)
		{
			_dto = dto;

			Cells = cells;
		}

		public void Randomize()
		{
			List<int> numbers = new List<int>();
			int gridLength = Size * Size;
			
			for (int i = 0; i < gridLength; i++)
			{
				numbers.Add(i);
			}

			for (int i = 0; i < Size; i++)
			{
				for (int j = 0; j < Size; j++)
				{
					var rIndex = Random.Range(0, numbers.Count);
					var rNumber = numbers[rIndex];
					var cellIndex = i * Size + j;

					Cells[cellIndex].Position = new Vector2Int(i, j);
					Cells[cellIndex].Number = rNumber;

					numbers.RemoveAt(rIndex);
				}
			}
			
			var emptyCell = Cells.FirstOrDefault(c => c.Number == 0);
			var lastCell = Cells[^1];
			
			var tempPos = emptyCell.Position;
			emptyCell.Position = lastCell.Position;
			lastCell.Position = tempPos;
		}

		public override string ToString()
		{
			var line = "";

			for (int i = 0; i < Size; i++)
			{
				line += "[";
				
				for (int j = 0; j < Size; j++)
				{
					var cell = Cells.FirstOrDefault(c => c.Position.x == i && c.Position.y == j);

					line += " " + cell.Number + " ";
				}

				line += "]\n";
			}

			return line;
		}
	}
}