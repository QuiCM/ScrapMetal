using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Synapse.Models;

namespace Synapse.Controllers
{
    [Route("synapse")]
    [ApiController]
    public class SynapseHostController : Controller
    {
        [HttpPost("{name}")]
        public IActionResult CreateSynapse([Required] string name,
                                           [Required] string token,
                                           [FromBody] SynapseRegistration registration)
        {
            return Ok(registration);
        }
    }
}