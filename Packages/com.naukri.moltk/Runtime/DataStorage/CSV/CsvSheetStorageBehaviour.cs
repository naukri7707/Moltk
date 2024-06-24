using Naukri.Moltk.DataStorage.Csv.GoogleAppScript;
using System.Threading.Tasks;
using UnityEngine;

namespace Naukri.Moltk.DataStorage.Csv
{
    public abstract class CsvSheetStorageBehaviour : ScriptableObject
    {
        public abstract CsvSheet OnBuild();

        public virtual void OnSave(CsvSheet csvSheet, CsvSheetStorage csvSheetStorage)
        {
            var path = csvSheetStorage.FilePath;
            csvSheet.Save(path);
        }

        public virtual void OnLoad(CsvSheet csvSheet, CsvSheetStorage csvSheetStorage)
        {
            var path = csvSheetStorage.FilePath;
            csvSheet.Load(path);
        }

        public virtual async Task OnUpload(CsvSheet csvSheet, IGASActions gasAction)
        {
            var values = csvSheet.To2DArray(true);
            var response = await gasAction.SetRange(values, 1, 1);
            if (response.status == ResponseStatus.Error)
            {
                Debug.LogWarning($"Upload failed: {response.message}");
            }
            else
            {
                Debug.Log("Upload success");
            }
        }

        public virtual async Task OnDownload(CsvSheet csvSheet, IGASActions gasAction)
        {
            var response = await gasAction.GetRange(1, 1, -1, -1);

            if (response.status == ResponseStatus.Error)
            {
                Debug.LogWarning($"Download failed: {response.message}");
            }
            else
            {
                csvSheet.From2DArray(response.data, true);
                Debug.Log("Download success");
            }
        }
    }
}
