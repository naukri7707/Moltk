namespace Naukri.Moltk.DataStorage.Csv.GoogleAppScript
{
    public enum ResponseStatus
    {
        Success,

        Error
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
