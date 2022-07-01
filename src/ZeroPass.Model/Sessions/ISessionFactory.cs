namespace ZeroPass.Model
{
    public interface ISessionFactory
    {
        EphemeralModel CreateEphemeralModel(string verifier);
        
        SessionModel CreateSession(
            string serverSecretEphemeral,
            string clientPublicEphemeral,
            string salt,
            string username,
            string verifier,
            string clientSessionProof);
    }
}