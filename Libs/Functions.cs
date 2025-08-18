using System.Xml.Serialization;
using System.Text.Json;

namespace Libs
{
    public class Functions
    {
        public int calcStartRow(int pageNumber, int pageSize)
        {
            var startRow = (pageNumber - 1) * pageSize;
            if (startRow < 0)
                startRow = 0;

            return startRow;
        }

    }
}
