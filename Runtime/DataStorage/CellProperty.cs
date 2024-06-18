using System;
using System.Linq;

namespace Naukri.Moltk.DataStorage
{
    public readonly struct CellProperty
    {
        internal CellProperty(Savedata savedata, string path)
        {
            this.savedata = savedata;
            Path = path ?? throw new ArgumentNullException(nameof(path));
            keys = path.Length == 0 ? new string[0] : path.Split('.').ToArray();
        }

        private readonly Savedata savedata;

        private readonly string[] keys;

        public static CellProperty Null => new(null, "");

        public bool IsNull => savedata == null || savedata.IsDestroy;

        public CellProperty Parent => new(savedata, string.Join('.', keys[..^1]));

        public string Path { get; }

        public readonly CellProperty this[string relativePath]
                => new(savedata, $"{Path}.{relativePath}");

        public readonly T GetValue<T>(string key)
        {
            var cell = FindCell();
            return (T)cell[key];
        }

        public readonly void SetValue<T>(string key, T value)
        {
            var cell = FindCell();
            cell[key] = value;
        }

        public readonly void SetCell(string json, bool append = false)
        {
            var cell = FindCell();
            cell.SetCell(json, append);
        }

        public readonly void DeleteValue(string key)
        {
            var cell = FindCell();
            cell.Remove(key);
        }

        public readonly void ClearCell()
        {
            var cell = FindCell();
            cell.Clear();
        }

        public string ToJson(bool format = false)
        {
            var cell = FindCell();
            return Cell.SaveToJson(cell, format);
        }

        public void CreateIfNotExist()
        {
            var current = savedata.data;
            foreach (var key in keys)
            {
                if (current.TryGetValue(key, out var value) && value != null)
                {
                    if (value is Cell cell)
                    {
                        current = cell;
                    }
                    else
                    {
                        throw new Exception($"Key '{key}' is not a {nameof(Cell)}");
                    }
                }
                else
                {
                    var cell = new Cell();
                    current[key] = cell;
                    current = cell;
                }
            }
        }

        private readonly Cell FindCell()
        {
            if (savedata.IsDestroy)
            {
                throw new Exception($"{nameof(Savedata)} is dead.");
            }

            var current = savedata.data;
            foreach (var key in keys)
            {
                if (current.TryGetValue(key, out var value) && value != null)
                {
                    if (value is Cell cell)
                    {
                        current = cell;
                    }
                    else
                    {
                        throw new Exception($"Key '{key}' is not a {nameof(Cell)}");
                    }
                }
                else
                {
                    throw new Exception($"Key '{key}' not found");
                }
            }

            return current;
        }
    }
}
