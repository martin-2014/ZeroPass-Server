using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using SecureRemotePassword;
using System.Security.Cryptography;
using ZeroPass.Model.Service;
using ZeroPass.Service;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Api.Tests
{
    public partial class TestSecretBuilder
    {
        readonly IConvertService ConvertService;
        readonly ICryptoService CryptoService;

        public TestSecretBuilder()
        {
            ConvertService = new ConvertService();
            CryptoService = new CryptoService(ConvertService);
        }

        public string CreateSecretKey()
        {
            var nonceBuffer = new byte[16];
            RandomNumberGenerator.Fill(nonceBuffer);
            return ConvertService.ToHexString(nonceBuffer);
        }

        public UserKeyEntity CreateUserKeyEntity(int userId, string email, string password, string secretKey)
        {
            var masterkey = DeriveMasterPassword(password, secretKey);

            var client = new SrpClient();
            var salt = client.GenerateSalt();

            var privateKey = client.DerivePrivateKey(salt, email, masterkey);
            var verifier = client.DeriveVerifier(privateKey);

            return new UserKeyEntity
            {
                UserId = userId,
                Salt = salt,
                Verifier = verifier,
                PrivateDataKey = CryptoService.EncryptText(GetPrivateKey(), masterkey),
                PublicDataKey = GetPublicKey()
            };
        }

        public string DeriveMasterPassword(string password, string secretKey)
        {
            var secretKeyBuffer = ConvertService.FromHexString(secretKey);
            var deriveationKeyBuffer = KeyDerivation.Pbkdf2(password, secretKeyBuffer, KeyDerivationPrf.HMACSHA256, 100000, 32);

            return ConvertService.ToHexString(deriveationKeyBuffer).ToLower();
        }

        string GetPrivateKey()
        {
            //pkcs8 格式
            return "MIIEvAIBADANBgkqhkiG9w0BAQEFAASCBKYwggSiAgEAAoIBAQC9Npctw7qT63IRkyx0eF5KL+0xYYuR/5bifQmY4izbHZPtWaM2C3o/" +
                "h0WKuiLjLT6vp2zPkENasXrAGsN1aqeLKVMRWtNg/ilOiAzlLu6G2BSD9QqTSzrR4JToWTkYqOhpRXA4iBc5YjCKeDN+qHTx0PxQQTnULM" +
                "1hKWCyWkamffUZkJJ40bUKi7xCtW9/unTSWavxoGfd2xr0PqDwZqg1RuScx7jVXMwzf0mQFXJcMGpXjKAc/tSbMx4A4ge2bJbMS2uudhPT" +
                "rtIROLMslgApmz2cyPttUoO9pQjdtjO63EqKe78PgE1osasSkayhbcl2K8EQBSg3JJ+22DJoC6zfAgMBAAECggEABsu5pU/6LmA5TMP2R5" +
                "6fywFVVGiLp0GV9+ouwzodtfxQzlMMXMoPviK2eYjoQjQ4o/1wQoo3aRVRHSo46p7bMqaC0G+XLPd5S1PxRp/8utodmS8JY13t2GY9DZVQ" +
                "ICsc4dRid0f9XDcYcDi9/LwXKyzq4FpfYIKqw7pv5H1VO/CKkTmX/IuRvgP1LAPlGZ3Xp2TJVXvG3ni9GzvmmMgpiNV1bunPZn2/YkSs1kr" +
                "uz/7c1y8vmevvg+Wwrh9+GaJl9tKskdrUGL80CB7BUeoCdlOKQ/ln7G1yUSk/hwg4Tul6GCoa1Xh+32kQQu7fobYkS1J5FkvqnDH5Gc6RvsZ" +
                "TSQKBgQD83ybTgK1nQXbz9LwnevqwIGIcPqR2h+azB85HYa9Ct1JjAxDMOlwejKx6QsyANU3XUa/GUrA2/A87429z1j3fR8Iw7dpshkccQ9Ac" +
                "ZmM5qmxtGNTen70wRbZaH/T+pgVPXI3y2uaYGb1z3DhxEYnUxqW9qLvJ8kaKzhMWJytjGQKBgQC/jdTnnKIkid7fQXDr6rCDQEjMKGUzc8LSV" +
                "DGmkzhHBZicv+qyD3w96x4Ye16KjIBGMqZ5/5Ol7HH/+LRe3DHg1xGkjaplRxCCfMl/4VWE5ZjcyRZdUKt8ehm2YNRnCq0SdN9Ql8nxED5MRo6" +
                "bqMjbKuGZGUUZvSVJVjeXZ0dGtwKBgFQpIQ77oLrg+uou5gBt2cmMvZqQ7sPUb1/elTUEugQGZ5E0j/6o3cf9Idp/KPjyxLmJImtrUeK6+YcB" +
                "JzLydx09ENCgGTZNapVprHYHTbb/lZ6pyQ9fMuRCD3Lnd+7Y6qODNmtBl8W/q2JIRTC8mZcLllYNNpL746aG96tD1hjhAoGABtelJETXEeE" +
                "O4gCvbO3sMjQIpapHuyfGbMbvIxkdKIRNqEO+uW+OK5QLdipp/R75sgg7JB56ArJIfDFBGCZbbcGg8rTQwl9gepjEhdnhuSaOaEySjPDFv" +
                "2OJKZeZ3rhl6EPeej2BzCedYhIfI/ZKfnGwr0xUyI+WFmGLCNB6nk8CgYAT2XdpdYWv1hMDPyJdXa7AQ39V38egpWSE1wRLKO2rhl8XKZuHy" +
                "KkaqYPPnz6lwmremk+ZsP27rrxnXgTBAWi2j1xvTMtgaEEI5RAlM+634JEiQ3GzaAMHRuxa//LWplLBL0HLh97Z7PCnciWS6uZjV2Du0k6U/4e/4gBL02BArw==";
        }

        string GetPublicKey()
        {
            //spki
            return "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAvTaXLcO6k+tyEZMsdHheSi/tMWGLkf+W4n0JmOIs2x2T7VmjNgt6P4dFiroi4" +
                "y0+r6dsz5BDWrF6wBrDdWqniylTEVrTYP4pTogM5S7uhtgUg/UKk0s60eCU6Fk5GKjoaUVwOIgXOWIwingzfqh08dD8UEE51CzNYSlgslpGp" +
                "n31GZCSeNG1Cou8QrVvf7p00lmr8aBn3dsa9D6g8GaoNUbknMe41VzMM39JkBVyXDBqV4ygHP7UmzMeAOIHtmyWzEtrrnYT067SETizLJYAK" +
                "Zs9nMj7bVKDvaUI3bYzutxKinu/D4BNaLGrEpGsoW3JdivBEAUoNySfttgyaAus3wIDAQAB";
        }
    }
}
