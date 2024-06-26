using System;

namespace Naukri.Moltk.DataStorage.Csv2
{
    [Serializable]
    public class Filiter
    {
        public Filiter(int columnIndex, string operation, string value)
        {
            this.columnIndex = columnIndex;
            this.operation = operation;
            this.value = value;
        }

        public int columnIndex;

        public string operation;

        public string value;
    }
}
