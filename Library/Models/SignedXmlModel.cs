using System.Collections.Generic;

namespace XmlSigner.Library.Models
{
    public class SignedXmlModel
    {
        public bool success { get; set; } = true;
        public string error { get; set; }
        public string xml { get; set; }
        public ICollection<CertificateModel> signatures { get; set; }
    }
}
