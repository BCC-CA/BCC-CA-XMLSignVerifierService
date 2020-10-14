using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.X509;
using System;
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
            try {
                xmlDoc.LoadXml(contentString);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                signedXml.success = false;
                signedXml.error = "File is not a valid XML";
                return signedXml;
            }
            try
            {
                signedXml.xml = GetXmlStringBeforeSigning(xmlDoc, out hasAnySignature);
                if (hasAnySignature)
                {
                    List<CertificateModel> certs = XmlSign.GetAllSign(xmlDoc);
                    if (certs == null)
                    {
                        signedXml.success = false;
                        signedXml.error = "File was modified";
                        signedXml.xml = null;
                        signedXml.signatures = null;
                        return signedXml;
                    }
                    signedXml.signatures = certs;
                }
                return signedXml;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                signedXml.success = false;
                signedXml.error = "Unknown Exception, Please check log File of XML Verifire Service";
                signedXml.xml = null;
                signedXml.signatures = null;
                return signedXml;
            }
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
            byte[] encodedByteArray = Base64.Decode(certString);
            return (X509Certificate)certParser.ReadCertificate(encodedByteArray);
        }
    }
}
