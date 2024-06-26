using Codice.Client.BaseCommands.BranchExplorer;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// 載入工作表內容
        /// </summary>
        public void Load();

        /// <summary>
        /// 儲存工作表內容
        /// </summary>
        public void Save();

        Filiter CreateFiliter(IColumn column, string operation, string value);

        Filiter CreateFiliter(int columnIndex, string operation, string value);

        Filiter CreateFiliter(string columnName, string operation, string value);
    }

    public abstract class Sheet : ISheet
    {
        public abstract IColumn[] Columns { get; }

        public abstract IRow this[int index] { get; }

        public abstract IRowArray this[params int[] indexes] { get; }

        public abstract IRowArray this[Range range] { get; }

        public abstract IRowArray this[(IColumn column, string operation, string value) filiter] { get; }

        public abstract IRowArray this[(int columnIndex, string operation, string value) filiter] { get; }

        public abstract IRowArray this[(string columnName, string operation, string value) filiter] { get; }

        public abstract void AppendRow(params string[] rowContents);

        public abstract void Clear();

        public abstract Filiter CreateFiliter(IColumn column, string operation, string value);

        public abstract Filiter CreateFiliter(int columnIndex, string operation, string value);

        public abstract Filiter CreateFiliter(string columnName, string operation, string value);

        public abstract void DeleteRow(int rowPosition, int howMany = 1);

        public abstract string[,] GetRangeValues(int row, int column, int numRows = 1, int numColumns = 1);

        public abstract string[,] GetRangeValuesUntil(int startRow, int startColumn, int endRow, int endColumn);

        public abstract void SetRangeValues(string[,] values, int row, int column);

        public abstract void Load();

        public abstract void Save();

        public abstract void SetOrAppendRow((int columnIndex, string operation, string value) filiter, params string[] rowContents);

        public abstract void SetOrAppendRow((string columnName, string operation, string value) filiter, params string[] rowContents);

        public abstract void SetOrAppendRow((IColumn column, string operation, string value) filiter, params string[] rowContents);
    }

    public class MemorySheet : Sheet
    {
        private readonly List<string[]> _data = new();

        public override IColumn[] Columns { get; }

        public override IRow this[int index] => new MemoryRow(this, index);

        public override IRowArray this[params int[] indexes] => new RowArray(this, indexes.Select(x => this[x]).ToArray());

        public override IRowArray this[Range range]
        {
            get
            {
                (int offset, int length) = range.GetOffsetAndLength(_data.Count);
                var indexes = Enumerable.Range(offset, length).ToArray();
                return this[indexes];
            }
        }

        public override IRowArray this[(IColumn column, string operation, string value) filiter]
        {
            get
            {
                var (column, operation, value) = filiter;
                var rowFiliter = CreateFiliter(column, operation, value);
                var rows = Match(rowFiliter);
                return rows;
            }
        }

        public override IRowArray this[(int columnIndex, string operation, string value) filiter]
        {
            get
            {
                var (columnIndex, operation, value) = filiter;
                var rowFiliter = CreateFiliter(columnIndex, operation, value);
                var rows = Match(rowFiliter);
                return rows;
            }
        }

        public override IRowArray this[(string columnName, string operation, string value) filiter]
        {
            get
            {
                var (columnName, operation, value) = filiter;
                var rowFiliter = CreateFiliter(columnName, operation, value);
                var rows = Match(rowFiliter);
                return rows;
            }
        }

        public override void AppendRow(params string[] rowContents)
        {
            throw new NotImplementedException();
        }

        public override void Clear()
        {
            throw new NotImplementedException();
        }

        public override Filiter CreateFiliter(IColumn column, string operation, string value)
        {
            throw new NotImplementedException();
        }

        public override Filiter CreateFiliter(int columnIndex, string operation, string value)
        {
            throw new NotImplementedException();
        }

        public override Filiter CreateFiliter(string columnName, string operation, string value)
        {
            throw new NotImplementedException();
        }

        public override void DeleteRow(int rowPosition, int howMany = 1)
        {
            throw new NotImplementedException();
        }

        public override string[,] GetRangeValues(int row, int column, int numRows = 1, int numColumns = 1)
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

        public override string[,] GetRangeValuesUntil(int startRow, int startColumn, int endRow, int endColumn)
        {
            MapIndex(ref startRow);
            MapIndex(ref startColumn);
            MapIndex(ref endRow);
            MapIndex(ref endColumn);

            var numRow = endRow - startRow + 1;
            var numColumn = endColumn - startColumn + 1;
            return GetRangeValues(startRow, startColumn, numRow, numColumn);
        }

        public override void SetRangeValues(string[,] values, int row, int column)
        {
            throw new NotImplementedException();
        }

        public override void Load()
        {
            throw new NotImplementedException();
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override void SetOrAppendRow((int columnIndex, string operation, string value) filiter, params string[] rowContents)
        {
            throw new NotImplementedException();
        }

        public override void SetOrAppendRow((string columnName, string operation, string value) filiter, params string[] rowContents)
        {
            throw new NotImplementedException();
        }

        public override void SetOrAppendRow((IColumn column, string operation, string value) filiter, params string[] rowContents)
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
}
