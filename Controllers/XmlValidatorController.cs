using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SinedXmlVelidator.Controllers
{
    //[Route("api/[controller]")]
    [Route("/")]
    [ApiController]
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
            return Ok(new
            {
                status = "XML Signature Verifier Is Running",
                currentTime = DateTime.UtcNow,
                os = System.Runtime.InteropServices.RuntimeInformation.OSDescription
            });
        }

        // POST api/<XmlValidatorController>
        [HttpPost]
        public string Post([FromBody] string value)
        {
            return value;
        }
    }
}
