using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Naukri.Moltk.DataStorage.Csv
{
    public class CsvSheet
    {
        public CsvSheet()
        {
            columns = new List<IColumn>();
        }

        public CsvSheet(params IColumn[] columns)
        {
            this.columns = new List<IColumn>(columns);
        }

        private readonly List<IColumn> columns;

        public IColumn GetColumn(string name)
        {
            return columns.Find(it => it.Name == name);
        }

        public IColumn<T> GetColumn<T>(string name)
        {
            return columns.Find(it => it.Name == name) as IColumn<T>;
        }

        public IColumn[] GetColumns()
        {
            return columns.ToArray();
        }

        public Row GetRow(int index)
        {
            var columns = GetColumns();
            return new Row(index, columns);
        }

        public Row[] GetRows()
        {
            var columns = GetColumns();

            var rowCount = FillUpCells();

            var rows = new Row[rowCount];

            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                rows[rowIndex] = new Row(rowIndex, columns);
            }

            return rows;
        }

        public void Load(string path)
        {
            using StreamReader sr = new StreamReader(path);

            // pass header line
            string line = sr.ReadLine();
            if (line == null)
            {
                return;
            }

            int rowIndex = 0;
            while ((line = sr.ReadLine()) != null)
            {
                string[] values = line.Split(',');
                for (int i = 0; i < values.Length; i++)
                {
                    var column = columns[i];
                    column.SetValue(rowIndex, values[i]);
                }
                rowIndex++;
            }
        }

        public void Save(string path)
        {
            using StreamWriter sw = new StreamWriter(path);
            // 寫入標頭
            string headerLine = string.Join(",", columns.Select(c => c.Name));
            sw.WriteLine(headerLine);

            var rowCount = FillUpCells();

            // 寫入每一行
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                string[] values = new string[columns.Count];
                for (int colIndex = 0; colIndex < columns.Count; colIndex++)
                {
                    var cellText = columns[colIndex][rowIndex].ToString();
                    values[colIndex] = cellText;
                }
                sw.WriteLine(string.Join(",", values));
            }
        }

        public string[,] To2DArray(bool withColumnName = false)
        {
            // 找出最長的行數並填補不足的部分
            var rowCount = FillUpCells();

            var resultRowCount = rowCount + (withColumnName ? 1 : 0);
            var result = new string[resultRowCount, columns.Count];

            for (int colIndex = 0; colIndex < columns.Count; colIndex++)
            {
                var column = columns[colIndex];

                if (withColumnName)
                {
                    result[0, colIndex] = column.Name;
                }
                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    var cellText = column[rowIndex].ToString();

                    var resultRowIndex = rowIndex + (withColumnName ? 1 : 0);
                    result[resultRowIndex, colIndex] = cellText;
                }
            }
            return result;
        }

        public void From2DArray(string[,] values, bool withColumnName = false)
        {
            for (int colIndex = 0; colIndex < values.GetLength(1); colIndex++)
            {
                var column = columns[colIndex];

                for (int rowIndex = (withColumnName ? 1 : 0); rowIndex < values.GetLength(0); rowIndex++)
                {
                    var value = values[rowIndex, colIndex];

                    var sheetRowIndex = rowIndex - (withColumnName ? 1 : 0);
                    column.SetValue(sheetRowIndex, value);
                }
            }
        }

        private int FillUpCells()
        {
            // 找出最長的行數並填補不足的部分
            var maxRowCount = columns.Max(col => col.RowCount);

            foreach (var column in columns)
            {
                column.FillDefaultValueUntil(maxRowCount - 1);
            }

            return maxRowCount;
        }
    }
}
