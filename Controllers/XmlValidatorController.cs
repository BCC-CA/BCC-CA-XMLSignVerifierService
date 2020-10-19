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
                if(signedXml.success == false)
                {
                    return BadRequest(signedXml);
                }
                else
                {
                    return signedXml;
                }
            }
            else
            {
                SignedXmlModel signedXml = new SignedXmlModel();
                signedXml.error = "A file Should be Uploaded";
                signedXml.success = false;
                return BadRequest(signedXml);
            }
        }

        [HttpPost("verify_string")]
        public ActionResult<SignedXmlModel> VerifyXmlString([FromForm] string xml)
        {
            //string xml = Request.Form["xml"];
            if (xml?.Length == 0)
            {
                SignedXmlModel signedXml = new SignedXmlModel();
                signedXml.error = "A xml string should be uploaded";
                signedXml.success = false;
                return BadRequest(signedXml);
            }
            else
            {
                SignedXmlModel signedXml = Utility.GetSignedXmlModel(xml);
                if (signedXml.success == false)
                {
                    return BadRequest(signedXml);
                }
                else
                {
                    return signedXml;
                }
            }
        }
    }
}
