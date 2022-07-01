using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using SecureRemotePassword;
using ZeroPass.Model.Models;
using ZeroPass.Model.Service;
using ZeroPass.Service;
using ZeroPass.Storage.Fakes;

namespace ZeroPass.Api.Tests
{
public class TestUserClientData
    {
        readonly string Email;
        readonly string MasterKey;
        private string CommunicateKey = "";
        private SrpEphemeral SessionKey;
        private readonly ICryptoService CryptoService;

        ClientIdentifierProofModel ClientIdentifierProof = null;

        public TestUserClientData(TestUserSecret secret)
        {
            var testSecretBuilder = new TestSecretBuilder();
            IConvertService convertService = new ConvertService();
            CryptoService = new CryptoService(convertService);
            Email = secret.Email;
            MasterKey = testSecretBuilder.DeriveMasterPassword(secret.Password, secret.SecretKey);
        }

        public UserExchangePublicKeyModel GetClientPublicKey()
        {
            var client = new SrpClient();
            SessionKey = client.GenerateEphemeral();

            return new UserExchangePublicKeyModel()
            {
                Email = this.Email,
                PublicKey = SessionKey.Public
            };
        }


        public AuthenticateModel GetAuthenticateModel(UserPublicKeyModel serverPublicKey)
        {
            ClientIdentifierProof = GetClientIdentifierProof(serverPublicKey);

            return new AuthenticateModel
            {
                KeyId = serverPublicKey.ExchangeKeyId,
                Request = GetRequestData(MasterKey)
            };
        }

        ClientIdentifierProofModel GetClientIdentifierProof(UserPublicKeyModel publicKey)
        {
            var client = new SrpClient();
            var privateKey = client.DerivePrivateKey(publicKey.AdditionalData, Email, MasterKey);
            var clientSession = client.DeriveSession(SessionKey.Secret, publicKey.PublicKey, publicKey.AdditionalData, Email, privateKey);
            CommunicateKey = clientSession.Key;
            return new ClientIdentifierProofModel()
            {
                Email = Email,
                IdentifierProof = clientSession.Proof
            };
        }

        UserKeyRequestModel GetRequestData(string payload)
        {
            var request = new UserKeyRequestModel();
            request.ClientIdentifierProof = ClientIdentifierProof;
            request.Raw = CryptoService.EncryptText(payload, CommunicateKey);
            request.Signature = SignRequest(ClientIdentifierProof, request.Raw, CommunicateKey);

            return request;
        }

        static string SignRequest(ClientIdentifierProofModel clientIdentifierProof, string payload, string secretKey)
        {
            var plainText = JsonConvert.SerializeObject(clientIdentifierProof) + payload;
            var cryptoKey = Encoding.UTF8.GetBytes(secretKey);
            using var hmac = new HMACSHA256(cryptoKey);
            var signatureBuffer = hmac.ComputeHash(Encoding.UTF8.GetBytes(plainText.ToLower()));

            return Convert.ToBase64String(signatureBuffer);
        }
    }
}