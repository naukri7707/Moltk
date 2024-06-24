using Naukri.InspectorMaid;
using Naukri.Moltk.Core;
using Naukri.Moltk.DataStorage.Csv.GoogleAppScript;
using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Naukri.Moltk.DataStorage.SavedataService;

namespace Naukri.Moltk.DataStorage.Csv
{
    public class CsvSheetStorage : MonoBehaviour
    {
        [SerializeField]
        private FileDirectory fileDirectory = FileDirectory.PersistentDataPath;

        [SerializeField]
        private string fileName = "savedata.csv";

        [SerializeField]
        private CsvSheetStorageBehaviour behaviour;

        [SerializeField]
        private bool useGoogleSheet;

        [SerializeField, ShowIf(nameof(useGoogleSheet))]
        private GoogleAppScriptManager googleAppScriptService;

        private CsvSheet _csvSheet;

        public string FilePath
        {
            get
            {
                var fileDirectoryPath = fileDirectory switch
                {
                    FileDirectory.ConsoleLogPath => Application.consoleLogPath,
                    FileDirectory.DataPath => Application.dataPath,
                    FileDirectory.PersistentDataPath => Application.persistentDataPath,
                    FileDirectory.StreamingAssetsPath => Application.streamingAssetsPath,
                    FileDirectory.TemporaryCachePath => Application.temporaryCachePath,
                    _ => throw new Exception($"Invalid {nameof(FileDirectory)}"),
                };
                return $"{fileDirectoryPath}/{fileName}";
            }
        }

        protected CsvSheet CsvSheet
        {
            get
            {
                _csvSheet ??= behaviour.OnBuild();
                return _csvSheet;
            }
        }

        [Target]
        public void Save()
        {
            behaviour.OnSave(CsvSheet, this);
        }

        [Target]
        public void Load()
        {
            behaviour.OnLoad(CsvSheet, this);
        }

        [Target]
        public async Task Upload()
        {
            if (!useGoogleSheet)
            {
                throw new Exception("useGoogleSheet must be true");
            }
            await behaviour.OnUpload(CsvSheet, googleAppScriptService);
        }

        [Target]
        public async Task Download()
        {
            if (!useGoogleSheet)
            {
                throw new Exception("useGoogleSheet must be true");
            }
            await behaviour.OnDownload(CsvSheet, googleAppScriptService);
        }

        [Target]
        private void Print()
        {
            var map = CsvSheet.To2DArray(true);
            var sb = new StringBuilder();

            sb.AppendLine();
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    sb.Append(map[i, j].ToString());
                    sb.Append(',');
                }
                sb.AppendLine();
            }
            Debug.Log(sb.ToString());
        }
    }
}
