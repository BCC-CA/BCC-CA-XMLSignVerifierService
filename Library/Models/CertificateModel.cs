using System;
using System.Security.Cryptography.X509Certificates;
using XMLSigner.Library;

namespace XmlSigner.Library.Models
{
    public class CertificateModel
    {
        public CertificateModel(X509Certificate2 certificate, string timeString)
        {
            CertificateValidFrom = certificate.NotBefore;
            CertificateValidTo = certificate.NotAfter;
            CertificateIssuer = certificate.Issuer;
            CertificateSubject = certificate.Subject;
            SigningTime = (DateTime)Adapter.Base64DecodTime(timeString);
            tsaSignedTimestamp_Base64_UTF8 = timeString;
        }

        public DateTime CertificateValidFrom { get; }
        public DateTime CertificateValidTo { get; }
        public string CertificateIssuer { get; }
        public string CertificateSubject { get; }
        public DateTime SigningTime { get; }
        public string tsaSignedTimestamp_Base64_UTF8 { get; }
    }
}
