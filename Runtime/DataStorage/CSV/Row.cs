using System;
using System.Linq;

namespace Naukri.Moltk.DataStorage.Csv
{
    public class Row
    {
        public Row(int index, Row row)
        {
            this.index = index;
            columns = row.Columns;
        }

        public Row(int index, params Column[] columns)
        {
            this.index = index;
            this.columns = columns;
        }

        private readonly int index;

        private readonly Column[] columns;

        public int Index => index;

        private Column[] Columns => columns.ToArray();

        public T GetValue<T>(string columnName)
        {
            var column = GetColumn<T>(columnName);
            return GetValue(column);
        }

        public T GetValue<T>(int columnIndex)
        {
            var column = columns[columnIndex] as Column<T>;
            return GetValue(column);
        }

        public T GetValue<T>(Column<T> column)
        {
            return column.GetValue(index);
        }

        public void SetValue<T>(string columnName, T value)
        {
            var column = GetColumn<T>(columnName);
            SetValue(column, value);
        }

        public void SetValue<T>(int columnIndex, T value)
        {
            var column = columns[columnIndex] as Column<T>;
            SetValue(column, value);
        }

        public void SetValue<T>(Column<T> column, T value)
        {
            column.SetValue(index, value);
        }

        public Row Next()
        {
            return new Row(index + 1, columns);
        }

        private Column GetColumn(string name)
        {
            return Array.Find(columns, it => it.Name == name);
        }

        private Column<T> GetColumn<T>(string name)
        {
            return GetColumn(name) as Column<T>;
        }
    }
}
