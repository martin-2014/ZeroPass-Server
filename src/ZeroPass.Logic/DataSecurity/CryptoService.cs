using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ZeroPass.Model.Service;

namespace ZeroPass.Service
{
    public class CryptoService : ICryptoService
    {
        const int NonceByteSizes = 12;
        const int TagByteSizes = 16;

        readonly IConvertService ConvertService;

        public CryptoService(IConvertService convertService)
            => ConvertService = convertService;

        public string DecryptText(string cipherText, string cryptoKey)
        {
            var additionalDataLength = NonceByteSizes + TagByteSizes;
            var cipherBuffer = Convert.FromBase64String(cipherText);
            if (cipherBuffer.Length <= additionalDataLength) return null;

            var nonceBuffer = cipherBuffer.Take(NonceByteSizes).ToArray();
            var tagBuffer = cipherBuffer.TakeLast(TagByteSizes).ToArray();

            var plainBuffer = new byte[cipherBuffer.Length - additionalDataLength];
            var payloadBuffer = cipherBuffer.Skip(NonceByteSizes).Take(plainBuffer.Length).ToArray();

            var keyBuffer = ConvertService.FromHexString(cryptoKey);
            using var crypto = new AesGcm(keyBuffer);
            crypto.Decrypt(nonceBuffer, payloadBuffer, tagBuffer, plainBuffer);
            return Encoding.UTF8.GetString(plainBuffer);
        }

        public string EncryptText(string plainText, string cryptoKey)
        {
            var plainBuffer = Encoding.UTF8.GetBytes(plainText);
            var cipherBuffer = new byte[plainBuffer.Length];

            var nonceBuffer = new byte[NonceByteSizes];
            RandomNumberGenerator.Fill(nonceBuffer);

            var tagBuffer = new byte[TagByteSizes];

            var keyBuffer = ConvertService.FromHexString(cryptoKey);
            using var crypto = new AesGcm(keyBuffer);
            crypto.Encrypt(nonceBuffer, plainBuffer, cipherBuffer, tagBuffer);

            byte[][] resultArray = { nonceBuffer, cipherBuffer, tagBuffer };
            var resultBuffer = CombineByteArray(resultArray);
            return Convert.ToBase64String(resultBuffer);
        }

        public string EncryptWithPublicKey(string plainText, string skpiPublicKey)
        {
            using var encryptRSA = RSA.Create(2048);

            var keyBuffer = Convert.FromBase64String(skpiPublicKey);
            encryptRSA.ImportSubjectPublicKeyInfo(keyBuffer, out _);

            var plainBuffer = Encoding.UTF8.GetBytes(plainText);
            var cipherBuffer = encryptRSA.Encrypt(plainBuffer, RSAEncryptionPadding.OaepSHA256);
            return Convert.ToBase64String(cipherBuffer);
        }

        public string DecryptWithPrivateKey(string cipherText, string pkcs8PrivateKey)
        {
            using var decryptRSA = RSA.Create(2048);
            var keyBuffer = Convert.FromBase64String(pkcs8PrivateKey);
            decryptRSA.ImportPkcs8PrivateKey(keyBuffer, out _);

            var cipherBuffer = Convert.FromBase64String(cipherText);
            var plainBuffer = decryptRSA.Decrypt(cipherBuffer, RSAEncryptionPadding.OaepSHA256);

            return Encoding.UTF8.GetString(plainBuffer);
        }

        public string SignData(string plainText, string pkcs8PrivateKey)
        {
            var rsa = RSA.Create(2048);
            var keyBuffer = Convert.FromBase64String(pkcs8PrivateKey);
            rsa.ImportPkcs8PrivateKey(keyBuffer, out _);

            var plainTextBuffer = Encoding.UTF8.GetBytes(plainText);
            var signatureBuffer = rsa.SignData(plainTextBuffer, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            return Convert.ToBase64String(signatureBuffer);
        }

        public bool VerifyData(string plainText, string signature, string skpiPublicKey)
        {
            var encryptRSA = RSA.Create(2048);

            var keyBuffer = Convert.FromBase64String(skpiPublicKey);
            encryptRSA.ImportSubjectPublicKeyInfo(keyBuffer, out _);

            var plainTextBuffer = Encoding.UTF8.GetBytes(plainText);
            var signatureBuffer = Convert.FromBase64String(signature);
            return encryptRSA.VerifyData(plainTextBuffer, signatureBuffer, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }

        static byte[] CombineByteArray(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }
    }
}
