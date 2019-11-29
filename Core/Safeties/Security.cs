using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Cross.Util.Extensions;

namespace Core.Safeties
{
    public class Security
    {
        private readonly string _defaultKey = "bV4KaNvxu3R3mau3vpPDutawhLTA8sUV8wzH7msDWnjLTjXB7Pvhf4eUvQZNU2mdP8RLZunSprrkJKq2PuWzRk8Zq2PfnpR3YrYWSq2AaUT6meeC3tr36nTVkuudKWbDyPjhUwbwXBzkUhSPKPpSRheR49em4qJWa6YHSCjKX3K93FEMnqXhYauXwjJwbHXfPWTSdxy6ebCBPyAfqk7Uz5nrRddVjZrxWNCMZYG3PbcvPWA34eKzfFEKD3hEbxph";
        private readonly int _divisionKey = 4;
        private readonly byte[] _iv = new byte[16] {0x26, 0xdc, 0xff, 0x00, 0xad, 0xed, 0x7a, 0xee, 0xc5, 0xfe, 0x07, 0xaf, 0x4d, 0x08, 0x22, 0x3c};
        private byte[] _key;

        public Security() => _key = SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(_defaultKey));

        public string Encrypt(string data, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                CustomKey(key);
            }

            var encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;

            // Set key and IV
            var aesKey = new byte[32];
            Array.Copy(_key, 0, aesKey, 0, 32);
            encryptor.Key = aesKey;
            encryptor.IV = _iv;

            var memoryStream = new MemoryStream();

            var aesEncryptor = encryptor.CreateEncryptor();

            var cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);

            var plainBytes = Encoding.ASCII.GetBytes(data);

            cryptoStream.Write(plainBytes, 0, plainBytes.Length);

            cryptoStream.FlushFinalBlock();

            var cipherBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();

            var cipherText = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);

            return cipherText;
        }

        public string Decrypt(string data, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                CustomKey(key);
            }

            var encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;

            var aesKey = new byte[32];
            Array.Copy(_key, 0, aesKey, 0, 32);
            encryptor.Key = aesKey;
            encryptor.IV = _iv;

            var memoryStream = new MemoryStream();

            var aesDecryptor = encryptor.CreateDecryptor();

            var cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write);

            var plainText = string.Empty;

            try
            {
                var cipherBytes = Convert.FromBase64String(data);

                cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);

                cryptoStream.FlushFinalBlock();

                var plainBytes = memoryStream.ToArray();

                plainText = Encoding.ASCII.GetString(plainBytes, 0, plainBytes.Length);
            }
            finally
            {
                memoryStream.Close();
                cryptoStream.Close();
            }

            return plainText;
        }

        private void CustomKey(string key)
        {
            var blockSize = key.Length / _divisionKey;
            var splitKey = key.CutString(blockSize).ToList();
            var splitDefaultKey = _defaultKey.CutString(blockSize).ToList();
            var newKey = string.Concat(splitDefaultKey.Intertwine(splitKey).ToList());

            _key = SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(newKey));
        }
    }
}