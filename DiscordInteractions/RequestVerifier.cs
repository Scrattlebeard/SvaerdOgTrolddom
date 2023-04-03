using System.Text;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;

namespace DiscordInteractions
{
    public class RequestVerifier
    {
        public static string PublicKey { get => Environment.GetEnvironmentVariable("PublicKey") ?? throw new Exception("Could not retrieve environment variable 'PublicKey'"); }

        public static bool VerifySignature(IHeaderDictionary headers, string message)
        {
            var signature = headers["X-Signature-Ed25519"];
            var timestamp = headers["X-Signature-Timestamp"];

            var dataToVerify = new List<byte>();
            dataToVerify.AddRange(Encoding.UTF8.GetBytes(timestamp));
            dataToVerify.AddRange(Encoding.UTF8.GetBytes(message));

            var verifier = new Ed25519Signer();
            verifier.Init(false, new Ed25519PublicKeyParameters(Convert.FromHexString(PublicKey)));
            verifier.BlockUpdate(dataToVerify.ToArray(), 0, dataToVerify.Count);
            return verifier.VerifySignature(Convert.FromHexString(signature));
        }
    }
}
