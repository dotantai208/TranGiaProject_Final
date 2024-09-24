using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SupTranGiaTichDiem.MaHoa
{
    public class HexadecimalToString
    {
        public static string HexToString(string hex)
        {
            if (hex.Length % 2 != 0)
            {
                throw new ArgumentException("Input must have an even number of characters.");
            }

            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                string hexPair = hex.Substring(i, 2);
                bytes[i / 2] = Convert.ToByte(hexPair, 16);
            }
            return Encoding.UTF8.GetString(bytes);
        }
    }
}