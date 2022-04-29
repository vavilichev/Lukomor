using System.Collections.Generic;
using UnityEngine;

namespace Lukomor.Example.Domain.TagsGrid
{
	public class TagsGrid
	{
		private const int NullNumber = 0;

		public TagsCell[,] CellsData { get; }
		public Vector2Int Size { get; }

		public TagsGrid(int gridSize)
		{
			Size = new Vector2Int(gridSize, gridSize);
			CellsData = new TagsCell[gridSize, gridSize];
		}

		public void Randomize()
		{
			var numbers = new List<int>();
			var gridSize = Size.x;
			var numbersListCapacity = gridSize * gridSize - 1;

			for (int i = 0; i < numbersListCapacity; i++)
			{
				numbers.Add(i + 1);
			}

			for (int i = 0; i < gridSize; i++)
			{
				for (int j = 0; j < gridSize; j++)
				{
					var rValue = 0;

					if (numbers.Count > 0)
					{
						var rIndex = Random.Range(0, numbers.Count);
						rValue = numbers[rIndex];

						numbers.RemoveAt(rIndex);
					}

					CellsData[i, j] = new TagsCell(rValue);
				}
			}

			CellsData[gridSize - 1, gridSize - 1] = new TagsCell(NullNumber);
		}
	}
}