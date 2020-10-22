using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Xml;
using XmlSigner.Library.Models;
using XMLSigner.Library;

namespace SinedXmlVelidator.Library
{
    [Flags]
    public enum ClassType
    {
        [Display(Name = "Class 0 Certificate"), Description("2.16.50.1.5.1")]
        Class0 = 0,
        [Display(Name = "Class 1 Certificate"), Description("2.16.50.1.2.1")]
        Class1,
        [Display(Name = "Class 2 Certificate"), Description("2.16.50.1.2.2")]
        Class2,
        [Display(Name = "Class 3 Certificate"), Description("2.16.50.1.2.3")]
        Class3,
        [Display(Name = "Policy ID is Not Approved"), Description("Any Other Certificate Policy ID found which did not matched in the approved list")]
        NotExists,
        [Display(Name = "Certificate Policy ID Not Found"), Description("No Policy ID Found")]
        NotFound
    }

    public class Utility
    {
        private static readonly string CERTIFICATE_POLICY_OID = "2.5.29.32";

        internal static ClassType GetClassTypeFromPolicyId(string policyId)
        {
            switch (policyId)
            {
                case "2.16.50.1.5.1": return ClassType.Class0;
                case "2.16.50.1.2.1": return ClassType.Class1;
                case "2.16.50.1.2.2": return ClassType.Class2;
                case "2.16.50.1.2.3": return ClassType.Class3;
                case null: return ClassType.NotExists;
                case "": return ClassType.NotExists;
                default: return ClassType.NotFound;
            }
        }

        internal static string GetEnumDisplayName(ClassType classType)
        {
            return classType.GetType()
                  .GetMember(classType.ToString())
                  .FirstOrDefault()
                  ?.GetCustomAttribute<DisplayAttribute>(false)
                  ?.Name
                  ?? classType.ToString();
        }

        [Obsolete]
        internal static string GetCertificatePolicyId(X509Certificate cert, int certificatePolicyPos = 0, int policyIdentifierPos = 0)
        {
            try
            {
                byte[] extPolicyBytes = cert.GetExtensionValue(CERTIFICATE_POLICY_OID).GetOctets();
                if (extPolicyBytes == null)
                {
                    return null;
                }
                DerOctetString oct = (DerOctetString)cert.GetExtensionValue(CERTIFICATE_POLICY_OID);
                Asn1Sequence seq = (Asn1Sequence)new Asn1InputStream(oct.GetOctets()).ReadObject();
                if (seq.Count <= (certificatePolicyPos))
                {
                    return null;
                }
                CertificatePolicies certificatePolicies = new CertificatePolicies(
                        PolicyInformation.GetInstance(seq[certificatePolicyPos]));
                PolicyInformation[] policyInformation = certificatePolicies.GetPolicyInformation();
                return policyInformation[0].PolicyIdentifier.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        internal static string GetFingureprintFromCertificate(X509Certificate certificate)
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

        [Obsolete]
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
            return certParser.ReadCertificate(encodedByteArray);
        }

        internal static DateTime GetLocalTimeFromUtcTime(DateTime utcTime)
        {
            TimeZoneInfo tzinfo = TimeZoneInfo.Local;
            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzinfo);
        }
    }
}