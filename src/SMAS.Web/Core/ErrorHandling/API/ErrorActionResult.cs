using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SMAS.Web.Core.ErrorHandling.API
{
    public class ErrorActionResult : IActionResult
    {
        public Task ExecuteResultAsync(ActionContext context)
        {
            var response = new BadRequestObjectResult(context.ModelState);
            return Task.FromResult(response);
        }
    }
}
