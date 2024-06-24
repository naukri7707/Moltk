using Naukri.InspectorMaid;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Naukri.Moltk.DataStorage.Csv.GoogleAppScript
{
    public partial class GoogleAppScriptManager : MonoBehaviour, IGASActions
    {
        [
            SerializeField,
            Target,
            ColumnScope, ShowIf(nameof(showMultiApiKeyWarning)),
                HelpBox("By creating multiple Google accounts and using the same .gs script to generate multiple API Keys, you can mitigate Google App Script API access limitations to some extent. This script will switch to the next API Key with each API call, evenly distributing the token consumption across all API Keys. Note: This approach may violate Google's terms of service. We are not responsible for any issues or consequences arising from the use of this method. Please evaluate and decide whether to use it at your own discretion.", UnityEngine.UIElements.HelpBoxMessageType.Warning),
                Button("I got it.", nameof(__ReadMultiApiKeyWarning)),
            EndScope,
        ]
        private string[] apiKeys;

        private int currentKeyIndex = 0;

        [SerializeField]
        private string spreadsheetId = "";

        [SerializeField]
        private string sheetName = "Sheet 1";

        protected virtual string ApiKey
        {
            get
            {
                if (currentKeyIndex >= apiKeys.Length)
                {
                    currentKeyIndex = 0;
                }
                if (apiKeys == null || apiKeys.Length == 0)
                {
                    Debug.LogError("Missing API Key");
                    throw new Exception("Missing API Key");
                }
                var key = apiKeys[currentKeyIndex];
                currentKeyIndex = (currentKeyIndex + 1) % apiKeys.Length;
                return key;
            }
        }

        protected virtual string ApiUrl => $"https://script.google.com/macros/s/{ApiKey}/exec";

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

        public async Task<ResponseData<string[,]>> GetRange(int row, int column, int numRows = 1, int numColumns = 1)
        {
            var form = new WWWForm();
            form.AddField(FIELD_ACTION, ACTION_GET_RANGE);
            form.AddField(FIELD_SPREADSHEET_ID, spreadsheetId);
            form.AddField(FIELD_ROW, row);
            form.AddField(FIELD_COLUMN, column);
            form.AddField(FIELD_NUM_ROWS, numRows);
            form.AddField(FIELD_NUM_COLUMNS, numColumns);
            form.AddField(FIELD_SHEET_NAME, sheetName);

            using var request = await UnityWebRequest.Post(ApiUrl, form).SendWebRequestAsync();

            if (request.result != UnityWebRequest.Result.Success)
            {
                string errorMsg = $"UnityWebRequest error: {request.error}";
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
                return JsonConvert.DeserializeObject<ResponseData<string[,]>>(responseText);
            }
        }

        public async Task<ResponseData<string>> SetRange(string[,] values, int row, int column)
        {
            var form = new WWWForm();
            form.AddField(FIELD_ACTION, ACTION_SET_RANGE);
            form.AddField(FIELD_SPREADSHEET_ID, spreadsheetId);
            form.AddField(FIELD_ROW, row);
            form.AddField(FIELD_COLUMN, column);
            form.AddField(FIELD_VALUES, JsonConvert.SerializeObject(values));
            form.AddField(FIELD_SHEET_NAME, sheetName);

            using var request = await UnityWebRequest.Post(ApiUrl, form).SendWebRequestAsync();

            if (request.result != UnityWebRequest.Result.Success)
            {
                string errorMsg = $"UnityWebRequest error: {request.error}";
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
                return JsonConvert.DeserializeObject<ResponseData<string>>(responseText);
            }
        }

        public async Task<ResponseData<int>> Find(Comparer comparer)
        {
            var form = new WWWForm();
            form.AddField(FIELD_ACTION, ACTION_FIND);
            form.AddField(FIELD_SPREADSHEET_ID, spreadsheetId);
            form.AddField(FIELD_COMPARER, JsonConvert.SerializeObject(comparer));
            form.AddField(FIELD_SHEET_NAME, sheetName);

            using var request = await UnityWebRequest.Post(ApiUrl, form).SendWebRequestAsync();

            if (request.result != UnityWebRequest.Result.Success)
            {
                string errorMsg = $"UnityWebRequest error: {request.error}";
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
                return JsonConvert.DeserializeObject<ResponseData<int>>(responseText);
            }
        }

        public async Task<ResponseData<int[]>> FindAll(Comparer comparer)
        {
            var form = new WWWForm();
            form.AddField(FIELD_ACTION, ACTION_FIND_ALL);
            form.AddField(FIELD_SPREADSHEET_ID, spreadsheetId);
            form.AddField(FIELD_COMPARER, JsonConvert.SerializeObject(comparer));
            form.AddField(FIELD_SHEET_NAME, sheetName);

            using var request = await UnityWebRequest.Post(ApiUrl, form).SendWebRequestAsync();

            if (request.result != UnityWebRequest.Result.Success)
            {
                string errorMsg = $"UnityWebRequest error: {request.error}";
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
                return JsonConvert.DeserializeObject<ResponseData<int[]>>(responseText);
            }
        }

        public async Task<ResponseData> AppendRow(string[] values)
        {
            var form = new WWWForm();
            form.AddField(FIELD_ACTION, ACTION_APPEND_ROW);
            form.AddField(FIELD_SPREADSHEET_ID, spreadsheetId);
            form.AddField(FIELD_VALUES, JsonConvert.SerializeObject(values));
            form.AddField(FIELD_SHEET_NAME, sheetName);

            using var request = await UnityWebRequest.Post(ApiUrl, form).SendWebRequestAsync();

            if (request.result != UnityWebRequest.Result.Success)
            {
                string errorMsg = $"UnityWebRequest error: {request.error}";
                return new ResponseData
                {
                    status = ResponseStatus.Error,
                    message = errorMsg,
                };
            }
            else
            {
                string responseText = request.downloadHandler.text;
                return JsonConvert.DeserializeObject<ResponseData>(responseText);
            }
        }

        public async Task<ResponseData> Clear()
        {
            var form = new WWWForm();
            form.AddField(FIELD_ACTION, ACTION_CLEAR);
            form.AddField(FIELD_SPREADSHEET_ID, spreadsheetId);
            form.AddField(FIELD_SHEET_NAME, sheetName);

            using var request = await UnityWebRequest.Post(ApiUrl, form).SendWebRequestAsync();

            if (request.result != UnityWebRequest.Result.Success)
            {
                string errorMsg = $"UnityWebRequest error: {request.error}";
                return new ResponseData
                {
                    status = ResponseStatus.Error,
                    message = errorMsg,
                };
            }
            else
            {
                string responseText = request.downloadHandler.text;
                return JsonConvert.DeserializeObject<ResponseData>(responseText);
            }
        }

        public async Task<ResponseData> SetOrAppendRow(string[] values, Comparer comparer)
        {
            // First, try to find the row
            var findResponse = await Find(comparer);
            if (findResponse.status == ResponseStatus.Success && findResponse.data != -1)
            {
                // Row found, set the range
                var rowIndex = findResponse.data;
                var rowArray = new string[1, values.Length];
                for (int i = 0; i < values.Length; i++)
                {
                    rowArray[0, i] = values[i];
                }
                return await SetRange(rowArray, rowIndex, 1);
            }
            else
            {
                // Row not found, append the row
                return await AppendRow(values);
            }
        }
    }

    // editor ui
    partial class GoogleAppScriptManager
    {
        [SerializeField, Hide]
        private bool __isShowMultiApiKeyWarningReaded;

        private bool showMultiApiKeyWarning => !__isShowMultiApiKeyWarningReaded && (apiKeys != null && apiKeys.Length > 1);

        private void __ReadMultiApiKeyWarning()
        {
            __isShowMultiApiKeyWarningReaded = true;
        }
    }

    // static
    partial class GoogleAppScriptManager
    {
        private const string ACTION_GET_RANGE = "getRange";

        private const string ACTION_SET_RANGE = "setRange";

        private const string ACTION_FIND = "find";

        private const string ACTION_FIND_ALL = "findAll";

        private const string ACTION_APPEND_ROW = "appendRow";

        private const string ACTION_CLEAR = "clear";

        private const string FIELD_ACTION = "action";

        private const string FIELD_SPREADSHEET_ID = "spreadsheetId";

        private const string FIELD_ROW = "row";

        private const string FIELD_COLUMN = "column";

        private const string FIELD_NUM_ROWS = "numRows";

        private const string FIELD_NUM_COLUMNS = "numColumns";

        private const string FIELD_VALUES = "values";

        private const string FIELD_SHEET_NAME = "sheetName";

        private const string FIELD_COMPARER = "comparer";
    }
}
