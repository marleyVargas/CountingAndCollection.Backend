using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Transversal.APIResponseHandlers.Wrappers;
using Transversal.ServiceErrorHandlers;

namespace DistributedServices.Api.Controllers
{
    public class ApiControllerBase: ControllerBase
    {
        
        [ApiExplorerSettingsAttribute(IgnoreApi = true)]
        public IActionResult HandleResult([FromBody]object data, [FromQuery]ErrorServiceProvider errors)
        {
            if(errors.HasError())
            {
                return BadRequest(new
                    ApiResponse(null,
                        errors.GetWarnings(),
                        409,
                        ResponseMessageEnum.Failure,
                        errors.GetErrors()
                    ));
            }
            else
            {
                return Ok(new ApiResponse(data, errors.GetWarnings()));
            }
        }
    }
}