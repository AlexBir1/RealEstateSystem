using DwellingAPI.ResponseWrapper.Implementation;
using DwellingAPI.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DwellingAPI.Filters
{
    public class ValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(!context.ModelState.IsValid)
                context.Result = new OkObjectResult(new ResponseWrapper<ContactModel>(context.ModelState.Select(x => x.Value).SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));

            base.OnActionExecuting(context);
        }
    }
}
