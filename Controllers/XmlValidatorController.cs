using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SinedXmlVelidator.Library;
using XmlSigner.Library.Models;
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
        public async Task<ActionResult<SignedXmlModel>> VerifyFile([FromForm]IFormFile file)    //XmlFile xmlFile
        {
            if (file?.Length > 0)
            {
                _logger.LogDebug("File Name - " + file.FileName);
                string contentString = await Adapter.ReadAsStringAsync(file);
                SignedXmlModel signedXml = Utility.GetSignedXmlModel(contentString);
                if(signedXml == null)
                {
                    return BadRequest("File was modified");
                }
                return signedXml;
            }
            else
            {
                return BadRequest("A file Should be Uploaded");
            }
        }

        [HttpPost("verify_string")]
        public ActionResult<SignedXmlModel> VerifyXmlString([FromForm] string xml)
        {
            //string xml = Request.Form["xml"];
            if (xml?.Length == 0)
            {
                return BadRequest("A file Should be Uploaded");
            }
            else
            {
                SignedXmlModel signedXml = Utility.GetSignedXmlModel(xml);
                if (signedXml == null)
                {
                    return BadRequest("File was modified");
                }
                return signedXml;
            }
        }
    }
}
