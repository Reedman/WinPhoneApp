using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace NyAppHelper
{

    public class HMACSHAEncryptor
    {
        private static string  ByteToString(byte[] bytes) 
        { 
            string sbinary = "";

            for (int i = 0; i < bytes.Length; i++)
            {
                sbinary += bytes[i].ToString("X2"); // hex format
            }
            return (sbinary);
        } 


        public static string GetHASMString(string content) 
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] keyBytes = encoding.GetBytes(AppSettings.HashKey); 
            byte[] messageBytes = encoding.GetBytes(content); 

            HMACSHA256 hmacsha256 = new HMACSHA256(keyBytes); 

            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes); 
            string hmac3 = ByteToString(hashmessage); 

            return hmac3; 
        }

    }

}
