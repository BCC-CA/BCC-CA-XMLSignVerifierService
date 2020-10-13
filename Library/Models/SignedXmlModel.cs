using System.Collections.Generic;
using XmlSigner.Library.Models;

namespace XmlSigner.Library.Models
{
    public class SignedXmlModel
    {
        public string xml { get; set; }
        public ICollection<CertificateModel> certificates { get; set; }
    }
}
