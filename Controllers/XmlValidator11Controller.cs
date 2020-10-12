using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XmlSigner.Library.Model;
using XMLSigner.Library;

namespace SinedXmlVelidator.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/")]
    [ApiController]
    public class XmlValidator11Controller : ControllerBase
    {
        private readonly ILogger<XmlValidator11Controller> _logger;

        public XmlValidator11Controller(ILogger<XmlValidator11Controller> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Get()
        {
            _logger.LogDebug("Application Home Page Accessed");
            return Ok(new {
                status = "XML Signature Verifier Is Running",
                currentTime = DateTime.UtcNow,
                os = System.Runtime.InteropServices.RuntimeInformation.OSDescription
            });
        }

        [HttpPost("verify_file")]
        public async Task<ActionResult<ICollection<Certificate>>> VerifyFile([FromForm] IFormFile file)    //XmlFile xmlFile
        {
            if (file.Length > 0)
            {
                string contentString = await Adapter.ReadAsStringAsync(file);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(contentString);
                bool hasAnySignature = true;
                List<Certificate> certificates = GetValidatedCertificates(xmlDoc, out hasAnySignature);
                return certificates;
                //return null;
            }
            else
            {
                return BadRequest("A file Should be Uploaded");
            }
        }

        [HttpPost("verify_string")]
        public ActionResult<ICollection<Certificate>> VerifyXmlString([FromQuery(Name = "xml")] string xml)    //XmlFile xmlFile
        {
            if (xml.Length>0)
            {
                return BadRequest("A file Should be Uploaded");
            }
            else
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                bool hasAnySignature = true;
                List<Certificate> certificates = GetValidatedCertificates(xmlDoc, out hasAnySignature);
                return certificates;
            }
        }

        private List<Certificate> GetValidatedCertificates(XmlDocument xmlDoc, out bool hasAnySignature)
        {
            /*
            hasAnySignature = 
            if (hasAnySignature)
            {
                return BadRequest("A file Should be Uploaded");
            }
            else
            { }
            */

            throw new NotImplementedException();
        }
    }
}
