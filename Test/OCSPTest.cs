using Org.BouncyCastle.X509;
using SinedXmlVelidator.Library;
using System.IO;

namespace SinedXmlVelidator.Test
{
    public class OCSPTest
    {
        internal static OCSPStatus Test()
        {
            X509Certificate main = Utility.GetCertificateFromString(File.ReadAllText(@"C:\Users\abrar\Desktop\BCC-CA-XMLSignVerifierService\Test\CertCode\c.pem"));
            X509Certificate issuer = Utility.GetCertificateFromString(File.ReadAllText(@"C:\Users\abrar\Desktop\BCC-CA-XMLSignVerifierService\Test\CertCode\issuer.pem"));
            OCSPStatus status = OCSP.CheckOCSP(main, issuer);
            return status;
        }
    }
}
