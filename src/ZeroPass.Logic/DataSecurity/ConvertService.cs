using System;
using System.Text;
using ZeroPass.Model.Service;

namespace ZeroPass.Service
{
    public class ConvertService : IConvertService
    {
        public byte[] FromHexString(string hexString)
        {
            byte[] resultByte = new byte[hexString.Length / 2];
            int[] hexValue = new int[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05,0x06, 0x07,
                0x08, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };

            for (int byteIndex = 0, strIndex = 0; strIndex < hexString.Length; strIndex += 2, byteIndex += 1)
            {
                if (!IsHexChar(hexString[strIndex + 0]) || !IsHexChar(hexString[strIndex + 1])) throw new FormatException();

                resultByte[byteIndex] =
                    (byte)(hexValue[Char.ToUpper(hexString[strIndex + 0]) - '0'] << 4 |
                    hexValue[Char.ToUpper(hexString[strIndex + 1]) - '0']);
            }
            return resultByte;
        }

        public string ToHexString(byte[] bytes)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);
            string hexAlphabet = "0123456789ABCDEF";

            foreach (byte byteItem in bytes)
            {
                result.Append(hexAlphabet[(int)(byteItem >> 4)]);
                result.Append(hexAlphabet[(int)(byteItem & 0xF)]);
            }

            return result.ToString();
        }

        static bool IsHexChar(char character)
        {
            char upperChar = Char.ToUpper(character);
            return ((upperChar >= 48 && upperChar <= 57) || (upperChar >= 65 && upperChar <= 70)) ? true : false;
        }
    }
}
