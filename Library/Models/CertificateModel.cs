using Microsoft.VisualBasic.CompilerServices;
using Org.BouncyCastle.X509;
using SinedXmlVelidator.Library;
using System;
using XMLSigner.Library;

namespace XmlSigner.Library.Models
{
    public class CertificateModel
    {
        public CertificateModel(X509Certificate certificate, string tsaTimeString, string localPcTimeString, string signingReason)
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
            //Convertion To Deployment Server Time - End
            tsaSignedTimestamp_Base64_UTF8 = tsaTimeString;
            CertificateHash = GetFingureprintFromCertificate(certificate);
            SigningReason = signingReason;
        }

        private string GetFingureprintFromCertificate(X509Certificate certificate)
        {
            byte[] derEncodedCert = certificate.GetEncoded();
            byte[] hash;
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                md5.TransformFinalBlock(derEncodedCert, 0, derEncodedCert.Length);
                hash = md5.Hash;
            }
            return Convert.ToBase64String(hash);
        }

        public DateTime CertificateValidFrom { get; }
        public DateTime CertificateValidTo { get; }
        public string CertificateIssuer { get; }
        public string CertificateSubject { get; }
        public string CertificateHash { get; }
        public DateTime SigningTsaTime { get; }
        public DateTime SigningLocalPcTime { get; }
        public string SigningReason { get; }
        public string tsaSignedTimestamp_Base64_UTF8 { get; }
    }
}
