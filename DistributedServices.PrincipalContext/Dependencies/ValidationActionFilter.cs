using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Transversal.DTOs;
using Transversal.Response;

namespace DistributedServices.Api.Dependencies
{
    public class ValidationActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;
            if (!modelState.IsValid)
            {
                List<MessageDto> errorList = new List<MessageDto>(); 
            
                foreach (var key in modelState.Keys)
                {
                    var state = modelState[key];
                    if (state.Errors.Any())
                    {
                        MessageDto message = new MessageDto();

                        message.Code = key;
                        message.Message = state.Errors.First().ErrorMessage;

                        errorList.Add(message);

                    }
                }

                var errorResponse = new ApiResponse<object>(new
                {
                    Success = false,
                    ErrorList = errorList
                });

                context.Result = new BadRequestObjectResult(errorResponse);
            }
        }
    }

}