using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XmlSigner.Library.Model;
using XMLSigner.Library;

namespace SinedXmlVelidator.Controllers
{
    //[Route("api/[controller]")]
    [Route("/")]
    [ApiController]
    [EnableCors]
    public class XmlValidatorController : ControllerBase
    {
        private readonly ILogger<XmlValidatorController> _logger;

        public XmlValidatorController(ILogger<XmlValidatorController> logger)
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
        public async Task<ActionResult<ICollection<Certificate>>> VerifyFile([FromForm]IFormFile file)    //XmlFile xmlFile
        {
            if (file?.Length > 0)
            {
                string contentString = await Adapter.ReadAsStringAsync(file);
                bool hasAnySignature = true;
                List<Certificate> certificates = GetValidatedCertificates(contentString, out hasAnySignature);
                return certificates;
                //return null;
            }
            else
            {
                return BadRequest("A file Should be Uploaded");
            }
        }

        [HttpPost("verify_string")]
        public ActionResult<ICollection<Certificate>> VerifyXmlString([FromForm] string xml)
        {
            //string xml = Request.Form["xml"];
            if (xml?.Length == 0)
            {
                return BadRequest("A file Should be Uploaded");
            }
            else
            {
                bool hasAnySignature = true;
                List<Certificate> certificates = GetValidatedCertificates(xml, out hasAnySignature);
                return certificates;
            }
        }

        private List<Certificate> GetValidatedCertificates(string xml, out bool hasAnySignature)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
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
