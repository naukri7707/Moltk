using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Naukri.Moltk.DataStorage.Csv.GoogleAppScript
{
    public static class UnityWebRequestExtensions
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
}
