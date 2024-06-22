using Naukri.InspectorMaid;
using Newtonsoft.Json;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Naukri.Moltk.DataStorage.Csv.GoogleAppScript
{
    public enum ResponseStatus
    {
        Success,

        Error
    }

    // Todo: JsonUtility -> Newtonsoft.Json
    // 因為 JsonUtility 不支援 string[,]
    public interface IGoogleSheetManager
    {
        Task<ResponseData<string[,]>> GetRange(int row, int column, int numRows = 1, int numColumns = 1);

        Task<ResponseData<string>> SetRange(string[,] values, int row, int column);

        Task<ResponseData<string[,]>> GetSheet();

        Task<ResponseData> SetSheet(string[,] values);

        Task<ResponseData<int>> FindRowId(Comparer comparer);

        Task<ResponseData<int[]>> FindRowIds(Comparer comparer);

        string GetSpreadSheetId();

        void SetSpreadSheetId(string id);

        string GetSheetName();

        void SetSheetName(string name);

        void SetUseActiveSpreadsheet(bool useActive);
    }

    public static class AsyncOperationExtensions
    {
        public static Task<UnityWebRequest> AsTask(this UnityWebRequestAsyncOperation asyncOperation)
        {
            var tcs = new TaskCompletionSource<UnityWebRequest>();
            asyncOperation.completed += _ => tcs.SetResult(asyncOperation.webRequest);
            return tcs.Task;
        }

        public static Task<UnityWebRequest> SendWebRequestAsync(this UnityWebRequest unityWebRequest)
        {
            return unityWebRequest.SendWebRequest().AsTask();
        }
    }

    public class GoogleSheetManager : MonoBehaviour, IGoogleSheetManager
    {
        const string API_KEY = "AKfycbxYVtSF1gQPoLOTzb3qCzBFGkMhxPPRy3dHfm3huJaHHjAgxBxlYuMu7YWZxXL7KAMn";

        private string spreadsheetId = "";

        private string sheetName;

        private bool useActiveSpreadsheet;

        private string ApiUrl => $"https://script.google.com/macros/s/{API_KEY}/exec";

        [Target]
        public async void Test()
        {
            useActiveSpreadsheet = true;
            sheetName = "sheet1";
            await SetRange(new[,] { { "A" } }, 1, 1);
        }

        public string GetSpreadSheetId()
        {
            return spreadsheetId;
        }

        public void SetSpreadSheetId(string id)
        {
            spreadsheetId = id;
        }

        public string GetSheetName()
        {
            return sheetName;
        }

        public void SetSheetName(string name)
        {
            sheetName = name;
        }

        public void SetUseActiveSpreadsheet(bool useActive)
        {
            useActiveSpreadsheet = useActive;
        }

        public async Task<ResponseData<string[,]>> GetRange(int row, int column, int numRows = 1, int numColumns = 1)
        {
            var form = new WWWForm();
            form.AddField("action", "getRange");
            form.AddField("spreadsheetId", spreadsheetId);
            form.AddField("row", row);
            form.AddField("column", column);
            form.AddField("numRows", numRows);
            form.AddField("numColumns", numColumns);
            form.AddField("sheetName", sheetName);
            form.AddField("useActiveSpreadsheet", useActiveSpreadsheet ? "true" : "false");

            using var request = await UnityWebRequest.Post(ApiUrl, form).SendWebRequestAsync();

            if (request.result != UnityWebRequest.Result.Success)
            {
                string errorMsg = $"UnityWebRequest error: {request.error}";
                Debug.LogError(errorMsg);
                return new ResponseData<string[,]>
                {
                    status = ResponseStatus.Error,
                    message = errorMsg,
                    data = null
                };
            }
            else
            {
                string responseText = request.downloadHandler.text;
                return JsonUtility.FromJson<ResponseData<string[,]>>(responseText);
            }
        }

        public async Task<ResponseData<string>> SetRange(string[,] values, int row, int column)
        {
            var form = new WWWForm();
            form.AddField("action", "setRange");
            form.AddField("spreadsheetId", spreadsheetId);
            form.AddField("row", row);
            form.AddField("column", column);
            form.AddField("values", JsonConvert.SerializeObject(values));
            form.AddField("sheetName", sheetName);
            form.AddField("useActiveSpreadsheet", useActiveSpreadsheet ? "true" : "false");

            using var request = await UnityWebRequest.Post(ApiUrl, form).SendWebRequestAsync();

            if (request.result != UnityWebRequest.Result.Success)
            {
                string errorMsg = $"UnityWebRequest error: {request.error}";
                Debug.LogError(errorMsg);
                return new ResponseData<string>
                {
                    status = ResponseStatus.Error,
                    message = errorMsg,
                    data = null
                };
            }
            else
            {
                string responseText = request.downloadHandler.text;
                return JsonUtility.FromJson<ResponseData<string>>(responseText);
            }
        }

        public async Task<ResponseData<string[,]>> GetSheet()
        {
            var form = new WWWForm();
            form.AddField("action", "getSheet");
            form.AddField("spreadsheetId", spreadsheetId);
            form.AddField("sheetName", sheetName);
            form.AddField("useActiveSpreadsheet", useActiveSpreadsheet ? "true" : "false");

            using var request = await UnityWebRequest.Post(ApiUrl, form).SendWebRequestAsync();

            if (request.result != UnityWebRequest.Result.Success)
            {
                string errorMsg = $"UnityWebRequest error: {request.error}";
                Debug.LogError(errorMsg);
                return new ResponseData<string[,]>
                {
                    status = ResponseStatus.Error,
                    message = errorMsg,
                    data = null
                };
            }
            else
            {
                string responseText = request.downloadHandler.text;
                return JsonUtility.FromJson<ResponseData<string[,]>>(responseText);
            }
        }

        public async Task<ResponseData> SetSheet(string[,] values)
        {
            var form = new WWWForm();
            form.AddField("action", "setSheet");
            form.AddField("spreadsheetId", spreadsheetId);
            form.AddField("values", JsonUtility.ToJson(values));
            form.AddField("sheetName", sheetName);
            form.AddField("useActiveSpreadsheet", useActiveSpreadsheet ? "true" : "false");

            using var request = await UnityWebRequest.Post(ApiUrl, form).SendWebRequestAsync();

            if (request.result != UnityWebRequest.Result.Success)
            {
                string errorMsg = $"UnityWebRequest error: {request.error}";
                Debug.LogError(errorMsg);
                return new ResponseData
                {
                    status = ResponseStatus.Error,
                    message = errorMsg
                };
            }
            else
            {
                string responseText = request.downloadHandler.text;
                return JsonUtility.FromJson<ResponseData>(responseText);
            }
        }

        public async Task<ResponseData<int>> FindRowId(Comparer comparer)
        {
            var form = new WWWForm();
            form.AddField("action", "findRowId");
            form.AddField("spreadsheetId", spreadsheetId);
            form.AddField("comparer", JsonUtility.ToJson(comparer));
            form.AddField("sheetName", sheetName);
            form.AddField("useActiveSpreadsheet", useActiveSpreadsheet ? "true" : "false");

            using var request = await UnityWebRequest.Post(ApiUrl, form).SendWebRequestAsync();

            if (request.result != UnityWebRequest.Result.Success)
            {
                string errorMsg = $"UnityWebRequest error: {request.error}";
                Debug.LogError(errorMsg);
                return new ResponseData<int>
                {
                    status = ResponseStatus.Error,
                    message = errorMsg,
                    data = -1
                };
            }
            else
            {
                string responseText = request.downloadHandler.text;
                return JsonUtility.FromJson<ResponseData<int>>(responseText);
            }
        }

        public async Task<ResponseData<int[]>> FindRowIds(Comparer comparer)
        {
            var form = new WWWForm();
            form.AddField("action", "findRowIds");
            form.AddField("spreadsheetId", spreadsheetId);
            form.AddField("comparer", JsonUtility.ToJson(comparer));
            form.AddField("sheetName", sheetName);
            form.AddField("useActiveSpreadsheet", useActiveSpreadsheet ? "true" : "false");

            using var request = await UnityWebRequest.Post(ApiUrl, form).SendWebRequestAsync();

            if (request.result != UnityWebRequest.Result.Success)
            {
                string errorMsg = $"UnityWebRequest error: {request.error}";
                Debug.LogError(errorMsg);
                return new ResponseData<int[]>
                {
                    status = ResponseStatus.Error,
                    message = errorMsg,
                    data = null
                };
            }
            else
            {
                string responseText = request.downloadHandler.text;
                return JsonUtility.FromJson<ResponseData<int[]>>(responseText);
            }
        }
    }

    [System.Serializable]
    public class Comparer
    {
        public int column;

        public string operation;

        public string value;
    }

    [System.Serializable]
    public class ResponseData
    {
        public ResponseStatus status;

        public string message;
    }

    [System.Serializable]
    public class ResponseData<T> : ResponseData
    {
        public T data;
    }
}
