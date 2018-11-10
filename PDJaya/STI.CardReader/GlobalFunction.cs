using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STI.CardReader
{
    public class GlobalFunction
    {
        public bool cekLRC(string data, string LRC)
        {
            bool Match = false;
            int i = 0;
            int discarded;
            string hexString = data;
            byte[] LrcCounter = new byte[1];
            byte[] byData = GetBytes(hexString, out discarded);

            try
            {
                for (i = 0; i < byData.Length; i++)
                {
                    LrcCounter[0] = (byte)(Convert.ToByte(LrcCounter[0]) ^ Convert.ToByte(byData[i]));
                }
                if (LRC == ToString(LrcCounter))
                {
                    Match = true;
                }
            }
            catch (Exception ex) { }

            return Match;
        }
        public string calculateLRC(string data)
        {
            string LRC = "";
            int i = 0, discarded;
            string hexString = data;
            byte[] LrcCounter = new byte[1];
            byte[] byData = GetBytes(hexString, out discarded);

            try
            {
                for (i = 0; i < byData.Length; i++)
                {
                    LrcCounter[0] = (byte)(Convert.ToByte(LrcCounter[0]) ^ Convert.ToByte(byData[i]));
                }
                LRC = ToString(LrcCounter);
            }
            catch (Exception ex)
            {
                LRC = ex.Message;
            }
            return LRC;
        }

        public byte[] GetBytes(string hexString, out int discarded)
        {
            discarded = 0;
            string newString = "";
            char c;
            // remove all none A-F, 0-9, characters
            for (int i = 0; i < hexString.Length; i++)
            {
                c = hexString[i];
                if (IsHexDigit(c))
                    newString += c;
                else
                    discarded++;
            }
            // if odd number of characters, discard last character
            if (newString.Length % 2 != 0)
            {
                discarded++;
                newString = newString.Substring(0, newString.Length - 1);
            }

            int byteLength = newString.Length / 2;
            byte[] bytes = new byte[byteLength];
            string hex;
            int j = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                hex = new String(new Char[] { newString[j], newString[j + 1] });
                bytes[i] = HexToByte(hex);
                j = j + 2;
            }
            return bytes;
        }
        public static bool IsHexDigit(Char c)
        {
            int numChar;
            int numA = Convert.ToInt32('A');
            int num1 = Convert.ToInt32('0');
            c = Char.ToUpper(c);
            numChar = Convert.ToInt32(c);
            if (numChar >= numA && numChar < (numA + 6))
                return true;
            if (numChar >= num1 && numChar < (num1 + 10))
                return true;
            return false;
        }
        private static byte HexToByte(string hex)
        {
            if (hex.Length > 2 || hex.Length <= 0)
                throw new ArgumentException("hex must be 1 or 2 characters in length");
            byte newByte = byte.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            return newByte;
        }
        public string ToString(byte[] bytes)
        {
            string hexString = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                hexString += bytes[i].ToString("X2");
            }
            return hexString;
        }


    }
}
