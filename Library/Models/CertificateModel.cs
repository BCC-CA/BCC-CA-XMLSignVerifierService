using Org.BouncyCastle.X509;
using SinedXmlVelidator.Library;
using System;
using XMLSigner.Library;

namespace XmlSigner.Library.Models
{
    public class CertificateModel
    {
        [Obsolete]
        public CertificateModel(X509Certificate certificate, string tsaTimeString, string localPcTimeString, string signingReason, string serverIdDuringSignature)
        {
            CertificateValidFrom = certificate.NotBefore;
            CertificateValidTo = certificate.NotAfter;
            CertificateIssuer = certificate.IssuerDN.ToString();
            CertificateSubject = certificate.SubjectDN.ToString();
            SigningTsaTime = (DateTime)Adapter.Base64DecodTime(tsaTimeString);
            SigningLocalPcTime = Convert.ToDateTime(localPcTimeString);
            SigningTsaTime = Utility.GetLocalTimeFromUtcTime(SigningTsaTime);
            SigningLocalPcTime = Utility.GetLocalTimeFromUtcTime(SigningLocalPcTime);
            TsaSignedTimestamp_Base64_UTF8 = tsaTimeString;
            CertificateHash = Utility.GetFingureprintFromCertificate(certificate);
            SigningReason = signingReason;
            CertificatePolicyId = Utility.GetCertificatePolicyId(certificate);
            ClassType = Utility.GetEnumDisplayName(Utility.GetClassTypeFromPolicyId(CertificatePolicyId));
            ASPProvidedId = serverIdDuringSignature;
        }

        public string SigningReason { get; }
        public string ASPProvidedId { get; }
        public string CertificateHash { get; }
        public DateTime CertificateValidFrom { get; }
        public DateTime CertificateValidTo { get; }
        public string CertificateIssuer { get; }
        public string CertificateSubject { get; }
        public string CertificatePolicyId { get; }
        public string ClassType { get; }
        public DateTime SigningTsaTime { get; }
        public DateTime SigningLocalPcTime { get; }
        public string TsaSignedTimestamp_Base64_UTF8 { get; }
    }
}
