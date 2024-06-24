using System;
using System.Collections.Generic;
using System.Linq;

namespace Naukri.Moltk.DataStorage.Csv
{
    public interface IColumn
    {
        public string Name { get; }

        internal int RowCount { get; }

        public object this[int index] { get; set; }

        public object GetValue(int index);

        public void SetValue(int index, string value);

        public IEnumerable<string> GetValues();

        internal void FillDefaultValueUntil(int index);
    }

    public interface IColumn<T> : IColumn
    {
        public new T this[int index] { get; set; }

        public new T GetValue(int index);

        public void SetValue(int index, T value);

        public new IEnumerable<T> GetValues();
    }

    public class Column<T> : IColumn<T>
    {
        public Column(string name) : this(name, new T[0])
        {
        }

        public Column(string name, params T[] values)
        {
            Name = name;
            this.values = new List<T>(values);
        }

        private readonly List<T> values;

        public string Name { get; }

        int IColumn.RowCount => values.Count;

        public T this[int index]
        {
            get => GetValue(index);
            set => SetValue(index, value);
        }

        object IColumn.this[int index]
        {
            get => GetValue(index);
            set => SetValue(index, (T)value);
        }

        public T GetValue(int index)
        {
            FillDefaultValueUntil(index);

            return values[index];
        }

        public void SetValue(int index, T value)
        {
            FillDefaultValueUntil(index);

            values[index] = value;
        }

        public IEnumerable<T> GetValues() => values;

        object IColumn.GetValue(int index)
        {
            return GetValue(index);
        }

        void IColumn.SetValue(int index, string value)
        {
            var parsedValue = Parse<T>.From(value);
            SetValue(index, parsedValue);
        }

        IEnumerable<string> IColumn.GetValues()
        {
            return GetValues().Select(it => it.ToString());
        }

        void IColumn.FillDefaultValueUntil(int index)
        {
            FillDefaultValueUntil(index);
        }

        private void FillDefaultValueUntil(int index)
        {
            while (values.Count <= index)
            {
                values.Add(default);
            }
        }
    }
}
