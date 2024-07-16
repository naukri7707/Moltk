using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Naukri.Moltk.DataStorage
{
    internal partial class Cell : IDictionary<string, object>
    {
        private readonly Dictionary<string, object> data = new();

        public Cell()
        {
        }

        public Cell(string json)
        {
            LoadFromJson(json);
        }

        public void SetCell(string json, bool append = false)
        {
            var cell = LoadFromJson(json);

            if (!append)
            {
                data.Clear();
            }
            foreach (var (key, value) in cell.data)
            {
                this[key] = value;
            }
        }
    }

    // IDictionary<string, object> implementation
    partial class Cell
    {
        public ICollection<string> Keys => ((IDictionary<string, object>)data).Keys;

        public ICollection<object> Values => ((IDictionary<string, object>)data).Values;

        public int Count => ((ICollection<KeyValuePair<string, object>>)data).Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<string, object>>)data).IsReadOnly;

        public object this[string key]
        {
            get => data[key];
            set => data[key] = value;
        }

        public void Add(string key, object value)
        {
            ((IDictionary<string, object>)data).Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return ((IDictionary<string, object>)data).ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return ((IDictionary<string, object>)data).Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return ((IDictionary<string, object>)data).TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<string, object> item)
        {
            ((ICollection<KeyValuePair<string, object>>)data).Add(item);
        }

        public void Clear()
        {
            ((ICollection<KeyValuePair<string, object>>)data).Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return ((ICollection<KeyValuePair<string, object>>)data).Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, object>>)data).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return ((ICollection<KeyValuePair<string, object>>)data).Remove(item);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, object>>)data).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)data).GetEnumerator();
        }
    }

    // Static methods
    partial class Cell
    {
        public static string SaveToJson(Cell cell, bool format = false)
        {
            var formatting = format ? Formatting.Indented : Formatting.None;
            return JsonConvert.SerializeObject(cell, formatting, new JsonConverter());
        }

        public static Cell LoadFromJson(string json)
        {
            var cell = JsonConvert.DeserializeObject<Cell>(json, new JsonConverter());
            return cell;
        }
    }

    // Inner structures
    partial class Cell
    {
        public class JsonConverter : JsonConverter<Cell>
        {
            public override Cell ReadJson(JsonReader reader, Type objectType, Cell existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var cell = new Cell();
                var jObj = JObject.Load(reader);
                foreach (var (key, value) in jObj)
                {
                    cell[key] = value.Type switch
                    {
                        JTokenType.Object => serializer.Deserialize<Cell>(value.CreateReader()),
                        JTokenType.Array => value.Select(it => serializer.Deserialize(it.CreateReader())).ToList(),
                        JTokenType.Null => null,
                        _ => value.ToObject<object>(),
                    };
                }
                return cell;
            }

            public override void WriteJson(JsonWriter writer, Cell value, JsonSerializer serializer)
            {
                var jObj = JObject.FromObject(value.data, serializer);
                jObj.WriteTo(writer);
            }
        }
    }
}
