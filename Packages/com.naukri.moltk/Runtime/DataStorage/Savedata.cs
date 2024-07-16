using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Naukri.Moltk.DataStorage
{
    public sealed partial class Savedata
    {
        public int version;

        internal Cell data;

        private bool isDestroy;
        private Savedata(int version, Cell data)
        {
            this.version = version;
            this.data = data;
        }

        public bool IsDestroy => isDestroy;

        public CellProperty this[string path]
                    => new(this, path);

        public void Destroy()
        {
            data = null;
            isDestroy = true;
        }
    }

    partial class Savedata
    {
        public static string SaveToJson(Savedata savedata, bool format = false)
        {
            var formatting = format ? Formatting.Indented : Formatting.None;
            return JsonConvert.SerializeObject(savedata, formatting, new JsonConverter(), new Cell.JsonConverter());
        }

        public static Savedata LoadFromJson(string json)
        {
            var savedata = JsonConvert.DeserializeObject<Savedata>(json, new JsonConverter(), new Cell.JsonConverter());
            return savedata;
        }

        public static void SaveToJsonFile(Savedata savedata, string filePath, bool format = false)
        {
            var json = SaveToJson(savedata, format);
            File.WriteAllText(filePath, json);
        }

        public static Savedata LoadFromJsonFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                var savedata = new Savedata(0, new Cell());
                SaveToJsonFile(savedata, filePath);
            }
            var json = File.ReadAllText(filePath);
            return LoadFromJson(json);
        }
    }

    partial class Savedata
    {
        public class JsonConverter : JsonConverter<Savedata>
        {
            public override Savedata ReadJson(JsonReader reader, Type objectType, Savedata existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var jObj = JObject.Load(reader);
                var savedata = new Savedata(
                    version: jObj[nameof(version)].ToObject<int>(),
                    data: serializer.Deserialize<Cell>(jObj[nameof(data)].CreateReader())
                    );

                return savedata;
            }

            public override void WriteJson(JsonWriter writer, Savedata value, JsonSerializer serializer)
            {
                var root = new JObject
                {
                    ["version"] = value.version,
                    ["data"] = JObject.FromObject(value.data, serializer),
                };
                root.WriteTo(writer);
            }
        }
    }
}
