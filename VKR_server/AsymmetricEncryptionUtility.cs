using System.Security.Cryptography;
using System.Text;

namespace VKR_server
{
    public static class AsymmetricEncryptionUtility
    {
        public static string GenerateKey(string targetFileOpen, string targetFileClose)
        {
            RSACryptoServiceProvider Algorithm = new RSACryptoServiceProvider();

            // Сохранить секретный ключ
            string CompleteKey = Algorithm.ToXmlString(true);
            byte[] KeyBytes = Encoding.UTF8.GetBytes(CompleteKey);

            KeyBytes = ProtectedData.Protect(KeyBytes, null, DataProtectionScope.LocalMachine);

            using (FileStream fs = new FileStream(targetFileClose, FileMode.Create))
            {
                fs.Write(KeyBytes, 0, KeyBytes.Length);
            }

            using (StreamWriter outputFile = new StreamWriter(targetFileOpen))
            {
                outputFile.WriteLine(Algorithm.ToXmlString(false));
            }

            // Вернуть открытый ключ
            return Algorithm.ToXmlString(false);
        }

        private static void ReadKey(RSACryptoServiceProvider algorithm, string keyFile)
        {
            byte[] KeyBytes;

            using (FileStream fs = new FileStream(keyFile, FileMode.Open))
            {
                KeyBytes = new byte[fs.Length];
                fs.Read(KeyBytes, 0, (int)fs.Length);
            }

            KeyBytes = ProtectedData.Unprotect(KeyBytes,
                    null, DataProtectionScope.LocalMachine);

            algorithm.FromXmlString(Encoding.UTF8.GetString(KeyBytes));
        }

        public static byte[] EncryptData(string data, string publicKey)
        {
            // Создать алгоритм на основе открытого ключа
            RSACryptoServiceProvider Algorithm = new RSACryptoServiceProvider();
            Algorithm.FromXmlString(publicKey);

            // Зашифровать данные
            return Algorithm.Encrypt(
                   Encoding.UTF8.GetBytes(data), true);
        }

        public static string DecryptData(byte[] data, string keyFile)
        {
            RSACryptoServiceProvider Algorithm = new RSACryptoServiceProvider();
            ReadKey(Algorithm, keyFile);

            byte[] ClearData = Algorithm.Decrypt(data, true);
            return Convert.ToString(
                   Encoding.UTF8.GetString(ClearData));
        }

        public static byte[] stringToByteArr(string data)
        {
            byte[] bytes = {};

            data = data.TrimEnd(',').Replace(" ", "");
            
            var data1 = data.Split(",");

            foreach (var item in data1)
            {
                Array.Resize(ref bytes, bytes.Length + 1);
                var num = Convert.ToInt32(item.Replace(" ", ""));
                bytes[bytes.Length - 1] = (byte)num;
            }

            return bytes;
        }

        public static string byteArrToString(byte[] bytes)
        {
            var str = "";

            foreach(var item in bytes)
            {
                str += item.ToString() + ",";
            }

            return str;
        }
    }
}
