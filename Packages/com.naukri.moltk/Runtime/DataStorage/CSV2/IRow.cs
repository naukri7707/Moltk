using System;
using System.Linq;

namespace Naukri.Moltk.DataStorage.Csv2
{
    public interface IRow
    {
        public int Index { get; }

        public string[] this[params string[] columnNames] { get; set; }

        public string this[string columnName] { get; set; }

        public string[] this[params IColumn[] columns] { get; set; }

        public string this[IColumn column] { get; set; }

        public string[] this[Range columnRange] { get; set; }

        public string[] this[params int[] columnIndexes] { get; set; }

        public string this[int columnIndex] { get; set; }
    }

    public class MemoryRow : IRow
    {
        public MemoryRow(ISheet sheet, int index)
        {
            this.sheet = sheet;
            Index = index;
        }

        private readonly ISheet sheet;

        public int Index { get; }

        public string[] this[params string[] columnNames]
        {
            get => this[columnNames.Select(x => Array.FindIndex(sheet.Columns, y => y.Name == x)).ToArray()];
            set => this[columnNames.Select(x => Array.FindIndex(sheet.Columns, y => y.Name == x)).ToArray()] = value;
        }

        public string this[string columnName]
        {
            get => this[Array.FindIndex(sheet.Columns, x => x.Name == columnName)];
            set => this[Array.FindIndex(sheet.Columns, x => x.Name == columnName)] = value;
        }

        public string[] this[params IColumn[] columns]
        {
            get => this[columns.Select(x => x.Index).ToArray()];
            set => this[columns.Select(x => x.Index).ToArray()] = value;
        }

        public string this[IColumn column]
        {
            get => this[column.Index];
            set => this[column.Index] = value;
        }

        public string[] this[Range columnRange]
        {
            get
            {
                (int offset, int length) = columnRange.GetOffsetAndLength(sheet.Columns.Length);
                var columnIndexes = Enumerable.Range(offset, length).ToArray();
                return this[columnIndexes];
            }
            set
            {
                (int offset, int length) = columnRange.GetOffsetAndLength(sheet.Columns.Length);
                var columnIndexes = Enumerable.Range(offset, length).ToArray();
                this[columnIndexes] = value;
            }
        }

        public string[] this[params int[] columnIndexes]
        {
            get
            {
                var values = new string[columnIndexes.Length];
                for (int i = 0; i < columnIndexes.Length; i++)
                {
                    values[i] = sheet.GetRangeValues(Index, columnIndexes[i])[0, 0];
                }
                return values;
            }
            set
            {
                for (int i = 0; i < columnIndexes.Length; i++)
                {
                    var values = new string[1, 1] { { value[i] } };
                    sheet.SetRangeValues(values, Index, columnIndexes[i]);
                }
            }
        }

        public string this[int columnIndex]
        {
            get => sheet.GetRangeValues(Index, columnIndex)[0, 0];
            set => sheet.SetRangeValues(new string[1, 1] { { value } }, Index, columnIndex);
        }
    }
}
