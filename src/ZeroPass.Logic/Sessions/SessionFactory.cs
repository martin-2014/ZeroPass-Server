using SecureRemotePassword;
using ZeroPass.Model;

namespace ZeroPass.Service
{
    public class SessionFactory : ISessionFactory
    {
        public EphemeralModel CreateEphemeralModel(string verifier)
        {
            var srpServer = new SrpServer();
            var srpEphemeral = srpServer.GenerateEphemeral(verifier);
            return new EphemeralModel { Public = srpEphemeral.Public, Secret = srpEphemeral.Secret };
        }

        public SessionModel CreateSession(
            string serverSecretEphemeral,
            string clientPublicEphemeral,
            string salt,
            string username,
            string verifier,
            string clientSessionProof)
        {
            var srpServer = new SrpServer();
            try
            {
                var srpSession = srpServer.DeriveSession(
                    serverSecretEphemeral,
                    clientPublicEphemeral,
                    salt,
                    username,
                    verifier,
                    clientSessionProof);

                return new SessionModel { Key = srpSession.Key, Proof = srpSession.Proof };
            }
            catch
            {
                return null;
            }
        }
    }
}