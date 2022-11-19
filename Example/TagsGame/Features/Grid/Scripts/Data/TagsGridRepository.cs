using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lukomor.TagsGame.Grid.Data
{
	public class TagsGridRepository
	{
		private const string Key = "Grid";
		private const int GridSize = 4;
		
		private GridData _cachedGridData;

		public void Load()
		{
			string json = PlayerPrefs.GetString(Key);

			if (!string.IsNullOrEmpty(json))
			{
				_cachedGridData = JsonUtility.FromJson<GridData>(json);
			}
			else
			{
				_cachedGridData = Create(GridSize);
				
				Save();
			}
		}

		public void Save()
		{
			string json = JsonUtility.ToJson(_cachedGridData);
			PlayerPrefs.SetString(Key, json);
		}

		public GridData Create(int gridSize)
		{
			var gridData = new GridData
			{
				Cells = new List<CellData>(),
				Size = gridSize
			};
			
			List<int> numbers = new List<int>();
			int gridLength = gridSize * gridSize;
			
			for (int i = 0; i < gridLength; i++)
			{
				numbers.Add(i);
			}

			for (int i = 0; i < gridSize; i++)
			{
				for (int j = 0; j < gridSize; j++)
				{
					var rIndex = Random.Range(0, numbers.Count);
					var rNumber = numbers[rIndex];
					
					gridData.Cells.Add(new CellData
					{
						PosX = i,
						PosY = j,
						Number = rNumber
					});

					numbers.RemoveAt(rIndex);
				}
			}
			
			var emptyCell = gridData.Cells.FirstOrDefault(c => c.Number == 0);
			var lastCell = gridData.Cells[^1];
			
			var tempPosX = emptyCell.PosX;
			var tempPosY = emptyCell.PosY;
			emptyCell.PosX = lastCell.PosX;
			emptyCell.PosY = lastCell.PosY;
			lastCell.PosX = tempPosX;
			lastCell.PosY = tempPosY;
			
			_cachedGridData = gridData;
			

			return gridData;
		}

		public GridData Get()
		{
			if (_cachedGridData != null)
			{
				return _cachedGridData;
			}

			return null;
		}
	}
}