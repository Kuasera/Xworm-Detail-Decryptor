using System;
using System.Security.Cryptography;
using System.Text;

class Program
{
    public static void Main()
    {
        string[] encryptedValues = {
            "VV1BFJ9zDlymFZg9jDfgHxIAnpMZuDmtFF7uJuyN/I4=",
            "HL3j+Z2eHszc0CYIxThrxg==",
            "np2l93BNgdiUH8xzHZsVWA==",
            "Y8xT90nepwceBJ0yNZLISQ==",
            "KLGW99J3pAgoBDW+zcCpsA==",
            "IxRZUDEUnfl+Yvr0Qc6iug=="
        };

        foreach (var encrypted in encryptedValues)
        {
            string decrypted = Decrypt(encrypted, "QsOwMXrJgXTZTM1E");
            Console.WriteLine($"Decrypted: {decrypted}");
        }
    }

    public static string Decrypt(string input, string mutex)
    {
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        using (RijndaelManaged rijndael = new RijndaelManaged())
        {
            byte[] keyArray = new byte[32];
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(mutex));

            Array.Copy(hash, 0, keyArray, 0, 16);
            Array.Copy(hash, 0, keyArray, 15, 16);

            rijndael.Key = keyArray;
            rijndael.Mode = CipherMode.ECB;
            rijndael.Padding = PaddingMode.PKCS7;

            byte[] toDecryptArray = Convert.FromBase64String(input);

            using (ICryptoTransform decryptor = rijndael.CreateDecryptor())
            {
                byte[] resultArray = decryptor.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
                return Encoding.UTF8.GetString(resultArray);
            }
        }
    }
}

