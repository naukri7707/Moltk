using System.Threading.Tasks;

namespace Naukri.Moltk.DataStorage.Csv.GoogleAppScript
{
    public interface IGASActions
    {
        Task<ResponseData<string[,]>> GetRange(int row, int column, int numRows = 1, int numColumns = 1);

        Task<ResponseData<string>> SetRange(string[,] values, int row, int column);

        Task<ResponseData<int>> Find(Comparer comparer);

        Task<ResponseData<int[]>> FindAll(Comparer comparer);

        Task<ResponseData> AppendRow(string[] values);

        Task<ResponseData> SetOrAppendRow(string[] values, Comparer comparer);

        Task<ResponseData> Clear();
    }
}
