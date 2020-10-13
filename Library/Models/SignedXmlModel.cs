using System.Collections.Generic;

namespace XmlSigner.Library.Models
{
    public class SignedXmlModel
    {
        public string xml { get; set; }
        public ICollection<CertificateModel> signatures { get; set; }
    }
}
