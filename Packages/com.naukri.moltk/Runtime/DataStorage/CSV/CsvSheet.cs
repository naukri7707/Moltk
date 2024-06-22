using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Naukri.Moltk.DataStorage.Csv
{
    public class CsvSheet
    {
        public CsvSheet()
        {
            columns = new List<Column>();
        }

        public CsvSheet(params Column[] columns)
        {
            this.columns = new List<Column>(columns);
        }

        private readonly List<Column> columns;

        public Column GetColumn(string name)
        {
            return columns.Find(it => it.Name == name);
        }

        public Column<T> GetColumn<T>(string name)
        {
            return columns.Find(it => it.Name == name) as Column<T>;
        }

        public Column[] GetColumns()
        {
            return columns.ToArray();
        }

        public Row GetRow(int index)
        {
            var columns = GetColumns();
            return new Row(index, columns);
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
                    var column = columns[i] as Column<string>;
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

            // 找出最長的行數並填補不足的部分
            var maxRowCount = columns.Max(col => col.RowCount);

            foreach (var column in columns)
            {
                column.FillDefaultValueUntil(maxRowCount);
            }

            // 寫入每一行
            for (int rowIndex = 0; rowIndex < maxRowCount; rowIndex++)
            {
                string[] values = new string[columns.Count];
                for (int colIndex = 0; colIndex < columns.Count; colIndex++)
                {
                    var columnText = columns[colIndex].ToString();
                    values[colIndex] = columnText;
                }
                sw.WriteLine(string.Join(",", values));
            }
        }
    }
}
