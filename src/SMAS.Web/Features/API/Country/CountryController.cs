using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrangeJetpack.Regionalization;

namespace SMAS.Web.Features.API.Country
{
    [Route("api/countries")]
    public class CountryController : Controller
    {
        // GET: api/values
        [HttpGet("")]
        public IActionResult Get()
        {
            var countries = Countries.GetAllCountries();
            return Ok(countries);
        }
        
    }
}
