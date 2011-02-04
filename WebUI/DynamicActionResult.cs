using System.Linq;
using System.Web.Mvc;

namespace BeerTime.WebUI
{
    public class DynamicActionResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context.RequestContext.HttpContext.Request.AcceptTypes.Any(x => x == "application/json"))
                new JsonResult {Data = context.Controller.ViewData}.ExecuteResult(context);
            else
            {
                new ViewResult {ViewData = context.Controller.ViewData}.ExecuteResult(context);
            }
        }
    }
}