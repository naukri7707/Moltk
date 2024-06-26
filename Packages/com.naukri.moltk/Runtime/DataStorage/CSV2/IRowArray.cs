using System;
using System.Linq;

namespace Naukri.Moltk.DataStorage.Csv2
{
    public interface IRowArray
    {
        public string[,] this[params string[] columnNames] { get; }

        public string[] this[string columnName] { get; }

        public string[,] this[params IColumn[] columns] { get; }

        public string[] this[IColumn column] { get; }

        public string[,] this[Range columnRange] { get; }

        public string[,] this[params int[] columnIndexes] { get; }

        public string[] this[int columnIndex] { get; }
    }

    public class RowArray : IRowArray
    {
        public RowArray(ISheet sheet, params IRow[] rows)
        {
            this.sheet = sheet;
            this.rows = rows;
        }

        private readonly ISheet sheet;

        private readonly IRow[] rows;

        public string[,] this[params string[] columnNames] => this[columnNames.Select(x => Array.FindIndex(sheet.Columns, y => y.Name == x)).ToArray()];

        public string[] this[string columnName] => this[Array.FindIndex(sheet.Columns, x => x.Name == columnName)];

        public string[,] this[params IColumn[] columns] => this[columns.Select(x => x.Index).ToArray()];

        public string[] this[IColumn column] => this[column.Index];

        public string[,] this[Range columnRange] => rows.Select(it => it[columnRange]).To2DArray();

        public string[,] this[params int[] columnIndexes] => rows.Select(it => it[columnIndexes]).To2DArray();

        public string[] this[int columnIndex] => rows.Select(it => it[columnIndex]).ToArray();
    }
}
