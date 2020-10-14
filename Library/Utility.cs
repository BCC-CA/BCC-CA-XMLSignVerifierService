using Org.BouncyCastle.X509;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using XmlSigner.Library.Models;
using XMLSigner.Library;

namespace SinedXmlVelidator.Library
{
    public class Utility
    {
        internal static SignedXmlModel GetSignedXmlModel(string contentString)
        {
            SignedXmlModel signedXml = new SignedXmlModel();
            bool hasAnySignature;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(contentString);
            signedXml.xml = GetXmlStringBeforeSigning(xmlDoc, out hasAnySignature);
            if (hasAnySignature)
            {
                List<CertificateModel> certs = XmlSign.GetAllSign(xmlDoc);
                if (certs == null)
                {
                    return null;
                }
                signedXml.signatures = certs;
            }
            return signedXml;
        }

        private static string GetXmlStringBeforeSigning(XmlDocument xmlDoc, out bool hasAnySignature)
        {
            hasAnySignature = XmlSign.CheckIfDocumentPreviouslySigned(xmlDoc);
            XmlDocument basicXml = XmlSign.GetRealXmlDocument(xmlDoc);
            return basicXml.OuterXml;
        }

        internal static X509Certificate GetCertificateFromString(string certString)
        {
            X509CertificateParser certParser = new X509CertificateParser();
            var b = Encoding.ASCII.GetBytes(certString);
            return certParser.ReadCertificate(b);
        }
    }
}
