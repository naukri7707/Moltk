using Codice.Client.BaseCommands.BranchExplorer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Naukri.Moltk.DataStorage.Csv2
{
    public interface ISheet
    {
        public IColumn[] Columns { get; }

        public IRowArray this[(string columnName, string operation, string value) filiter] { get; }

        public IRowArray this[(IColumn column, string operation, string value) filiter] { get; }

        public IRowArray this[(int columnIndex, string operation, string value) filiter] { get; }

        public IRowArray this[Range range] { get; }

        public IRowArray this[params int[] indexes] { get; }

        public IRow this[int index] { get; }

        /// <summary>
        /// 在工作表中目前資料區域底部附加一列。
        /// </summary>
        /// <param name="row"></param>
        public void AppendRow(params string[] rowContents);

        public void SetOrAppendRow((string columnName, string operation, string value) filiter, params string[] rowContents);

        public void SetOrAppendRow((IColumn column, string operation, string value) filiter, params string[] rowContents);

        public void SetOrAppendRow((int columnIndex, string operation, string value) filiter, params string[] rowContents);

        /// <summary>
        /// 從指定資料列位置開始刪除列數。
        /// </summary>
        /// <param name="rowPosition"></param>
        /// <param name="howMany"></param>
        public void DeleteRow(int rowPosition, int howMany = 1);

        /// <summary>
        /// 傳回最後一列包含內容的位置。
        /// </summary>
        /// <returns></returns>
        public int GetLastRow();

        /// <summary>
        /// 傳回指定座標上左上角儲存格的範圍，此範圍包含指定的資料列和欄數。
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="numRows"></param>
        /// <param name="numColumns"></param>
        public string[,] GetRangeValues(int row, int column, int numRows = 1, int numColumns = 1);

        /// <summary>
        /// 傳回指定座標上左上角儲存格的範圍，此範圍從 start 開始到 end 結束。
        /// </summary>
        /// <param name="startRow"></param>
        /// <param name="startColumn"></param>
        /// <param name="endRow"></param>
        /// <param name="endColumn"></param>
        /// <returns></returns>
        public string[,] GetRangeValuesUntil(int startRow, int startColumn, int endRow, int endColumn);

        public abstract void SetRangeValues(string[,] values, int row, int column);

        /// <summary>
        /// 清除工作表內容和格式資訊。
        /// </summary>
        /// <returns></returns>
        public void Clear();

        Filiter CreateFiliter(IColumn column, string operation, string value);

        Filiter CreateFiliter(int columnIndex, string operation, string value);

        Filiter CreateFiliter(string columnName, string operation, string value);
    }

    public partial class MemorySheet : ISheet
    {
        private MemorySheet() { }

        private readonly List<string[]> _data = new();

        private IColumn[] _columns;

        public IColumn[] Columns => _columns;

        public IRow this[int index] => new MemoryRow(this, index);

        public IRowArray this[params int[] indexes] => new RowArray(this, indexes.Select(x => this[x]).ToArray());

        public IRowArray this[Range range]
        {
            get
            {
                (int offset, int length) = range.GetOffsetAndLength(_data.Count);
                var indexes = Enumerable.Range(offset, length).ToArray();
                return this[indexes];
            }
        }

        public IRowArray this[(IColumn column, string operation, string value) filiter]
        {
            get
            {
                var (column, operation, value) = filiter;
                var rowFiliter = CreateFiliter(column, operation, value);
                var rows = Match(rowFiliter);
                return rows;
            }
        }

        public IRowArray this[(int columnIndex, string operation, string value) filiter]
        {
            get
            {
                var (columnIndex, operation, value) = filiter;
                var rowFiliter = CreateFiliter(columnIndex, operation, value);
                var rows = Match(rowFiliter);
                return rows;
            }
        }

        public IRowArray this[(string columnName, string operation, string value) filiter]
        {
            get
            {
                var (columnName, operation, value) = filiter;
                var rowFiliter = CreateFiliter(columnName, operation, value);
                var rows = Match(rowFiliter);
                return rows;
            }
        }

        public void AppendRow(params string[] rowContents)
        {
            if (rowContents.Length > Columns.Length)
            {
                throw new ArgumentException("The number of columns in the row is greater than the number of columns in the sheet.");
            }

            _data.Add(rowContents);
        }

        public void Clear()
        {
            _data.Clear();
        }

        public Filiter CreateFiliter(IColumn column, string operation, string value)
        {
            throw new NotImplementedException();
        }

        public Filiter CreateFiliter(int columnIndex, string operation, string value)
        {
            throw new NotImplementedException();
        }

        public Filiter CreateFiliter(string columnName, string operation, string value)
        {
            throw new NotImplementedException();
        }

        public void DeleteRow(int rowPosition, int howMany = 1)
        {
            throw new NotImplementedException();
        }

        public string[,] GetRangeValues(int row, int column, int numRows = 1, int numColumns = 1)
        {
            MapIndex(ref row);
            MapIndex(ref column);

            var range = new string[numRows, numColumns];

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numColumns; j++)
                {
                    range[i, j] = _data[row + i - 1][column + j - 1];
                }
            }

            return range;
        }

        public string[,] GetRangeValuesUntil(int startRow, int startColumn, int endRow, int endColumn)
        {
            MapIndex(ref startRow);
            MapIndex(ref startColumn);
            MapIndex(ref endRow);
            MapIndex(ref endColumn);

            var numRow = endRow - startRow + 1;
            var numColumn = endColumn - startColumn + 1;
            return GetRangeValues(startRow, startColumn, numRow, numColumn);
        }

        public void SetRangeValues(string[,] values, int row, int column)
        {
            throw new NotImplementedException();
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

            // Clean and set data
            Clear();
            while ((line = sr.ReadLine()) != null)
            {
                string[] values = line.Split(',');
                AppendRow(values);
            }
        }

        public void Save(string path)
        {
            using StreamWriter sw = new StreamWriter(path);

            // write header
            string headerLine = string.Join(",", _columns.Select(c => c.Name));
            sw.WriteLine(headerLine);

            // write each row
            var rowsArray = this[..]; // effectively get all rows rowArray (this[0..^0])

            foreach (var row in rowsArray.GetRows())
            {
                var values = row[..];
                string rowLine = string.Join(",", values);
                sw.WriteLine(rowLine);
            }
        }

        public void SetOrAppendRow((int columnIndex, string operation, string value) filiter, params string[] rowContents)
        {
            throw new NotImplementedException();
        }

        public void SetOrAppendRow((string columnName, string operation, string value) filiter, params string[] rowContents)
        {
            throw new NotImplementedException();
        }

        public void SetOrAppendRow((IColumn column, string operation, string value) filiter, params string[] rowContents)
        {
            throw new NotImplementedException();
        }

        public int GetLastRow()
        {
            throw new NotImplementedException();
        }

        private void MapIndex(ref int index)
        {
            if (index < 0)
            {
                index = _data.Count - index;
            }
        }

        private IRowArray Match(Filiter filiter)
        {
            var collection = new List<IRow>();
            for (var i = 0; i < _data.Count; i++)
            {
                var item = _data[i];
                var cellValue = item[filiter.columnIndex];

                var isMatched = filiter.operation switch
                {
                    "==" => cellValue == filiter.value,
                    "!=" => cellValue != filiter.value,
                    _ => throw new NotImplementedException(),
                };

                if (isMatched)
                {
                    var row = new MemoryRow(this, i);
                    collection.Add(row);
                }
            }

            return new RowArray(this, collection.ToArray());
        }
    }

    partial class MemorySheet
    {
        public static Builder CreateBuilder() => new();

        public class Builder
        {
            internal Builder() { }

            private readonly List<IColumn> _columns = new();

            private readonly MemorySheet _sheet = new();

            public Builder WithColumn(string name, string defaultValue = "")
            {
                var column = new Column(_sheet, name, _columns.Count, defaultValue);
                _columns.Add(column);
                return this;
            }

            public MemorySheet Build()
            {
                _sheet._columns = _columns.ToArray();
                return _sheet;
            }
        }
    }
}
