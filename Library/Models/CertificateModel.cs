using Org.BouncyCastle.X509;
using System;
using XMLSigner.Library;

namespace XmlSigner.Library.Models
{
    public class CertificateModel
    {
        public CertificateModel(X509Certificate certificate, string timeString)
        {
            CertificateValidFrom = certificate.NotBefore;
            CertificateValidTo = certificate.NotAfter;
            CertificateIssuer = certificate.IssuerDN.ToString();
            CertificateSubject = certificate.SubjectDN.ToString();
            SigningTime = (DateTime)Adapter.Base64DecodTime(timeString);
            tsaSignedTimestamp_Base64_UTF8 = timeString;
            CertificateHash = GetFingureprintFromCertificate(certificate);
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
        public DateTime SigningTime { get; }
        public string tsaSignedTimestamp_Base64_UTF8 { get; }
    }
}
