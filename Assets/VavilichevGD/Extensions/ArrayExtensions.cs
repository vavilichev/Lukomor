using System;
using UnityEngine;

namespace VavilichevGD.Extensions
{
	public static class ArrayExtensions
	{
		private static Vector2Int MinusOne = new Vector2Int(-1, -1);
		
		public static T FirstOrDefault<T>(this T[,] array, Func<T, bool> predicate)
		{
			var rowsCount = array.GetLength(0);
			var columnsCount = array.GetLength(1);
			var result = default(T);

			for (int i = 0; i < rowsCount; i++)
			{
				for (int j = 0; j < columnsCount; j++)
				{
					if (predicate(array[i, j]))
					{
						result = array[i, j];
					}
				}
			}

			return result;
		}

		public static Vector2Int GetCoordinates<T>(this T[,] array, T element)
		{
			var rowsCount = array.GetLength(0);
			var columnsCount = array.GetLength(1);
			var result = MinusOne;

			for (int i = 0; i < rowsCount; i++)
			{
				for (int j = 0; j < columnsCount; j++)
				{
					if (element.Equals(array[i, j]))
					{
						result.x = i;
						result.y = j;
					}
				}
			}

			if (result == MinusOne)
			{
				throw new Exception($"Cannot find element ({element}) in the array {array}");
			}

			return result;
		}
	}
}