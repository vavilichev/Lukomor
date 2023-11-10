using System;

namespace Lukomor.MVVM.Utils
{
    public static class CompareOperationExtensions
    {
        public static bool Compare<T>(this CompareOperation operation, T valueA, T valueB) where T : IComparable
        {
            var result = operation switch
            {
                CompareOperation.LessThan => valueA.CompareTo(valueB) < 0,
                CompareOperation.LessOrEqual => valueA.CompareTo(valueB) <= 0,
                CompareOperation.Equal => valueA.CompareTo(valueB) == 0,
                CompareOperation.MoreOrEqual => valueA.CompareTo(valueB) >= 0,
                CompareOperation.More => valueA.CompareTo(valueB) > 0,
                _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, null)
            };

            return result;
        }
		
    }
}