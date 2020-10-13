using System;
using System.Collections.Generic;
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
                List<CertificateModel> certs = GetValidatedCertificates(xmlDoc);
                if (certs == null)
                {
                    return null;
                }
                signedXml.certificates = certs;
            }
            return signedXml;
        }

        private static string GetXmlStringBeforeSigning(XmlDocument xmlDoc, out bool hasAnySignature)
        {
            hasAnySignature = XmlSign.CheckIfDocumentPreviouslySigned(xmlDoc);
            XmlDocument basicXml = XmlSign.GetRealXmlDocument(xmlDoc);
            return basicXml.OuterXml;
        }

        private static List<CertificateModel> GetValidatedCertificates(XmlDocument xml)
        {
            throw new NotImplementedException();
        }
    }
}
