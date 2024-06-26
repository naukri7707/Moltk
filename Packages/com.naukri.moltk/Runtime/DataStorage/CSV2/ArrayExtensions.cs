using System;
using System.Collections.Generic;
using System.Linq;

public static class ArrayExtensions
{
    public static T[,] To2DArray<T>(this IEnumerable<T[]> source)
    {
        // 將 source 轉換成數組以確定行數和列數
        var data = source.ToArray();
        var rows = data.Length;
        if (rows == 0)
        {
            throw new ArgumentException("The source enumerable is empty.");
        }

        var cols = data[0].Length;
        if (data.Any(x => x.Length != cols))
        {
            throw new ArgumentException("All rows must have the same number of columns.");
        }
        var result = new T[rows, cols];

        // 將數據複製到二維陣列中
        for (int i = 0; i < rows; i++)
        {
            if (data[i].Length != cols)
                throw new ArgumentException("All rows must have the same number of columns.");

            for (int j = 0; j < cols; j++)
            {
                result[i, j] = data[i][j];
            }
        }

        return result;
    }
}
