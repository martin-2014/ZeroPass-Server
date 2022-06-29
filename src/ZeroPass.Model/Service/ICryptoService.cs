namespace ZeroPass.Model.Service
{
    public interface ICryptoService
    {
        public string EncryptText(string plainText, string cryptoKey);

        public string DecryptText(string cipherText, string cryptoKey);

        public string EncryptWithPublicKey(string plainText, string skpiPublicKey);

        public string DecryptWithPrivateKey(string cipherText, string pkcs8PrivateKey);

        string SignData(string plainText, string pkcs8PrivateKey);

        bool VerifyData(string plainText, string signature, string skpiPublicKey);
    }
}
