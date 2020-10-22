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
            //Convertion To Deployment Server Time - Start
            SigningTsaTime = Utility.GetLocalTimeFromUtcTime(SigningTsaTime);
            SigningLocalPcTime = Utility.GetLocalTimeFromUtcTime(SigningLocalPcTime);
            TsaSignedTimestamp_Base64_UTF8 = tsaTimeString;
            CertificateHash = Utility.GetFingureprintFromCertificate(certificate);
            SigningReason = signingReason;
            CertificatePolicyId = Utility.GetCertificatePolicyId(certificate);
            ClassType = Utility.GetClassTypeFromPolicyId(CertificatePolicyId);
            ASPProvidedId = serverIdDuringSignature;
        }

        internal string SigningReason { get; }
        internal string ASPProvidedId { get; }
        internal string CertificateHash { get; }
        internal DateTime CertificateValidFrom { get; }
        internal DateTime CertificateValidTo { get; }
        internal string CertificateIssuer { get; }
        internal string CertificateSubject { get; }
        internal string CertificatePolicyId { get; }
        internal ClassType ClassType { get; }
        internal DateTime SigningTsaTime { get; }
        internal DateTime SigningLocalPcTime { get; }
        internal string TsaSignedTimestamp_Base64_UTF8 { get; }
    }
}
