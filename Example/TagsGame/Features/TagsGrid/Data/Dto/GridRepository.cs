using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lukomor.TagsGame.TagsGrid.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace Lukomor.TagsGame.TagsGrid.Dto
{
	public class GridRepository : IGridRepository
	{
		private class Database
		{
			public int NextCellId = 0;
			public GridDto Grid;
			public Dictionary<int, CellDto> CachedCells = new Dictionary<int, CellDto>();
		}
		
		private const string saveKey = "Grid";

		private Database _database;
		private readonly int GridSize = 4;

		public Task Load()
		{
			string json = PlayerPrefs.GetString(saveKey);

			if (!string.IsNullOrEmpty(json))
			{
				_database = JsonConvert.DeserializeObject<Database>(json);
			}
			else
			{
				_database = new Database();
				_database.Grid = (GridDto) Create();

				Save();
			}
			
			return Task.CompletedTask;
		}

		public Task Save()
		{
			string json = JsonConvert.SerializeObject(_database);
			PlayerPrefs.SetString(saveKey, json);
			
			return Task.CompletedTask;
		}

		public IGridSaveData Create()
		{
			List<int> numbers = new List<int>();
			List<ICellSaveData> cells = new List<ICellSaveData>();
			int gridLength = GridSize * GridSize;
			
			for (int i = 0; i < gridLength; i++)
			{
				numbers.Add(i);
			}
			
			for (int i = 0; i < GridSize; i++)
			{
				for (int j = 0; j < GridSize; j++)
				{
					var rIndex = Random.Range(0, numbers.Count);
					var rNumber = numbers[rIndex];
					var createdCell = new CellDto(_database.NextCellId++, rNumber, new Vector2Int(i, j));
					
					cells.Add(createdCell);
					numbers.RemoveAt(rIndex);

					_database.CachedCells[createdCell.CellId] = createdCell;
				}
			}

			var cellIds = cells.Select(c => c.CellId); 
			var gridData = new GridDto(GridSize, cellIds.ToArray());
			
			_database.Grid = gridData;

			var emptyCell = cells.FirstOrDefault(c => c.Number == 0);
			var lastCell = cells[^1];
			
			var tempPos = emptyCell.Position;
			emptyCell.Position = lastCell.Position;
			lastCell.Position = tempPos;

			return gridData;
		}

		public IGridSaveData GetGrid()
		{
			return _database.Grid;

		}

		public ICellSaveData GetCell(int cellId)
		{
			return _database.CachedCells[cellId];
		}
	}
}