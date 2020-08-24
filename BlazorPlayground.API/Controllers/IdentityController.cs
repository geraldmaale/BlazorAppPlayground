using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorPlayground.API.Controllers
{
    [Route("api/identities")]
    // [Authorize()]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            //var claims = User.Claims.Select(s=> { s.Type, s.Value});
            //return Ok(claims);
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}
