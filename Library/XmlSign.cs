using SinedXmlVelidator.Library;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using XmlSigner.Library.Models;
using DataObject = System.Security.Cryptography.Xml.DataObject;

namespace XMLSigner.Library
{
    class XmlSign
    {
        internal static bool CheckIfDocumentPreviouslySigned(XmlDocument xmlDocument)
        {
            int signCount = DocumentSignCount(xmlDocument);
            if (signCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int DocumentSignCount(XmlDocument xmlDocument)
        {
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("Signature");
            return nodeList.Count;
        }

        private static XmlDocument RemoveLastSign(XmlDocument xmlDocument)
        {
            //nodes[i].ParentNode.RemoveChild(nodes[i]);
            XmlNodeList signList = xmlDocument.GetElementsByTagName("Signature");
            int indexToRemove = signList.Count - 1;
            signList[indexToRemove].ParentNode.RemoveChild(signList[indexToRemove]);
            return xmlDocument;
        }

        private static bool? VerifyLastSign(XmlDocument xmlDocument)
        {
            if (!CheckIfDocumentPreviouslySigned(xmlDocument))
            {
                return null;    //File has no sign
            }
            if (VerifySignedXmlLastSignWithoutCertificateVerification(xmlDocument))
            {
                //return VerifyMetaDataObjectSignature(xmlDocument);
                return true;
            }
            else
            {
                return false;
            }
        }

        //Should get data from XmlDocument, not file
        private static bool VerifySignedXmlLastSignWithoutCertificateVerification(XmlDocument xmlDocument)
        {
            try
            {
                // Create a new SignedXml object and pass it
                SignedXml signedXml = new SignedXml(xmlDocument);

                // Find the "Signature" node and create a new
                // XmlNodeList object.
                XmlNodeList nodeList = xmlDocument.GetElementsByTagName("Signature");

                // Load the signature node.
                signedXml.LoadXml((XmlElement)nodeList[nodeList.Count - 1]);

                AsymmetricAlgorithm key;
                bool signatureCheckStatus = signedXml.CheckSignatureReturningKey(out key);
                if (signatureCheckStatus)
                {
                    XmlElement metaElement = (XmlElement)nodeList[nodeList.Count - 1].LastChild;
                    return VerifyMetaDataObjectSignature(metaElement, key);
                }
                else
                {
                    return false;
                }
                //return signedXml.CheckSignature(key);
                //return signedXml.CheckSignature(certificate, true);
            }
            catch (Exception exception)
            {
                Console.Write("Error: " + exception);
                throw exception;
            }
        }

        private static bool VerifyMetaDataObjectSignature(XmlElement metaXmlElement, AsymmetricAlgorithm ExtractedKey)
        {
            return true;
            throw new NotImplementedException();
        }

        internal static List<CertificateModel> GetAllSign(XmlDocument xmlDocument)
        {
            if (!CheckIfDocumentPreviouslySigned(xmlDocument))
                return null;
            List<CertificateModel> signerCertificateList = new List<CertificateModel>();

            while (CheckIfDocumentPreviouslySigned(xmlDocument))
            {
                if (VerifyLastSign(xmlDocument) == false)
                {
                    return null;
                }
                else
                {
                    signerCertificateList.Add(GetLastSignerCertificateModel(xmlDocument));
                }
                //Update xmlDocument by removing last sign tag
                xmlDocument = RemoveLastSign(xmlDocument);
            }
            return signerCertificateList;
        }

        internal static XmlDocument GetRealXmlDocument(XmlDocument xmlDoc)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlDoc.OuterXml);
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("Signature");
            for (int i = nodeList.Count - 1; i > -1; i--)
            {
                try
                {
                    nodeList[i].ParentNode.RemoveChild(nodeList[i]);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return xmlDocument;
        }

        private static CertificateModel GetLastSignerCertificateModel(XmlDocument xmlDocument)
        {
            if (!CheckIfDocumentPreviouslySigned(xmlDocument))
            {
                return null;
            }
            XmlDocument document = new XmlDocument();
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("Signature");

            // Load the last signature node.
            document.LoadXml(((XmlElement)nodeList[nodeList.Count - 1]).OuterXml);
            string certString = document.GetElementsByTagName("X509Data")[0].InnerText;
            //var timeString = Adapter.Base64DecodTime(document.GetElementsByTagName("Reference")[0].InnerText);
            string timeString = document.GetElementsByTagName("Reference")[0].Attributes["Id"].Value;
            /*...Decode text in cert here (may need to use Encoding, Base64, UrlEncode, etc) ending with 'data' being a byte array...*/
            X509Certificate2 certificate = new X509Certificate2(Encoding.ASCII.GetBytes(certString));
            var cert = Utility.GetCertificateFromString(certString);
            return new CertificateModel(certificate, timeString);
        }
    }
}
