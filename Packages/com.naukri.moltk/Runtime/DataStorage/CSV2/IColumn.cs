namespace Naukri.Moltk.DataStorage.Csv2
{
    public interface IColumn
    {
        public string Name { get; }

        public int Index { get; }

        public string DefaultValue { get; }
    }

    public class Column : IColumn
    {
        public Column(ISheet sheet, string name, int index, string defaultValue = "")
        {
            this.sheet = sheet;
            Name = name;
            Index = index;
            DefaultValue = defaultValue;
        }

        private readonly ISheet sheet;

        public string Name { get; }

        public int Index { get; }

        public string DefaultValue { get; }
    }
}
