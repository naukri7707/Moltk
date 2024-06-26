using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Naukri.Moltk.DataStorage.Csv2
{
    public interface IRowArray : IEnumerable<IRow>
    {
        public string[,] this[params string[] columnNames] { get; }

        public string[] this[string columnName] { get; }

        public string[,] this[params IColumn[] columns] { get; }

        public string[] this[IColumn column] { get; }

        public string[,] this[Range columnRange] { get; }

        public string[,] this[params int[] columnIndexes] { get; }

        public string[] this[int columnIndex] { get; }

        IRow[] GetRows();

        public class Enumerator : IEnumerator<IRow>
        {
            public Enumerator(IRowArray target)
            {
                rows = target.GetRows();
            }

            private int index = -1;

            private IRow[] rows;

            public IRow Current => rows[index];

            object IEnumerator.Current { get; }

            public bool MoveNext()
            {
                index++;
                if (index >= rows.Length)
                {
                    return false;
                }
                return true;
            }

            public void Reset()
            {
                index = -1;
            }

            public void Dispose()
            {
                rows = null;
            }
        }
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

        public string[,] this[Range columnRange] => GetRows().Select(it => it[columnRange]).To2DArray();

        public string[,] this[params int[] columnIndexes] => GetRows().Select(it => it[columnIndexes]).To2DArray();

        public string[] this[int columnIndex] => GetRows().Select(it => it[columnIndex]).ToArray();

        public IRow[] GetRows()
        {
            return rows.ToArray();
        }

        public IEnumerator<IRow> GetEnumerator()
        {
            return new IRowArray.Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
