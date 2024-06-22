using System.Collections.Generic;

namespace Naukri.Moltk.DataStorage.Csv
{
    public abstract class Column
    {
        public Column(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public abstract int RowCount { get; }

        internal abstract void FillDefaultValueUntil(int index);
    }

    public class Column<T> : Column
    {
        public Column(string name) : base(name)
        {
            values = new List<T>();
        }

        public Column(string name, params T[] values) : base(name)
        {
            this.values = new List<T>(values);
        }

        private readonly List<T> values;

        public override int RowCount => values.Count;

        public T this[int index]
        {
            get => GetValue(index);
            set => SetValue(index, value);
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

        internal override void FillDefaultValueUntil(int index)
        {
            while (values.Count <= index)
            {
                values.Add(default);
            }
        }
    }
}
