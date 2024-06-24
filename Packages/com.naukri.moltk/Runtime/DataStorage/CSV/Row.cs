using System;
using System.Linq;

namespace Naukri.Moltk.DataStorage.Csv
{
    public interface IRow
    {
        public int Index { get; }

        public T GetValue<T>(string columnName);

        public T GetValue<T>(int columnIndex);

        public T GetValue<T>(IColumn<T> column);

        public void SetValue<T>(string columnName, T value);

        public void SetValue<T>(int columnIndex, T value);

        public void SetValue<T>(IColumn<T> column, T value);

        public Row Next();
    }

    public class Row : IRow
    {
        public Row(int index, Row row) : this(index, row.Columns)
        {
        }

        public Row(int index, params IColumn[] columns)
        {
            this.index = index;
            this.columns = columns;
        }

        private readonly int index;

        private readonly IColumn[] columns;

        public int Index => index;

        private IColumn[] Columns => columns.ToArray();

        public T GetValue<T>(string columnName)
        {
            var column = GetColumn<T>(columnName);
            return GetValue(column);
        }

        public T GetValue<T>(int columnIndex)
        {
            var column = columns[columnIndex] as IColumn<T>;
            return GetValue(column);
        }

        public T GetValue<T>(IColumn<T> column)
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
            var column = columns[columnIndex] as IColumn<T>;
            SetValue(column, value);
        }

        public void SetValue<T>(IColumn<T> column, T value)
        {
            column.SetValue(index, value);
        }

        public Row Next()
        {
            return new Row(index + 1, columns);
        }

        private IColumn GetColumn(string name)
        {
            return Array.Find(columns, it => it.Name == name);
        }

        private IColumn<T> GetColumn<T>(string name)
        {
            return GetColumn(name) as IColumn<T>;
        }
    }
}
