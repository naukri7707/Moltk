namespace Naukri.Moltk.DataStorage.Csv2
{
    public interface IColumn
    {
        public string Name { get; }

        public int Index { get; }
    }

    public interface IColumn<T>
    {
        public T DefaultValue { get; }
    }

    public class Column<T> : IColumn<T>
    {
        public Column(ISheet sheet, string name, int index, T defaultValue = default)
        {
            this.sheet = sheet;
            Name = name;
            Index = index;
            DefaultValue = defaultValue;
        }

        private readonly ISheet sheet;

        public string Name { get; }

        public int Index { get; }

        public T DefaultValue { get; }
    }
}
