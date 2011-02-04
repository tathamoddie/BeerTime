using System.Linq;
using System.Web.Mvc;

namespace BeerTime.WebUI
{
    public class DynamicActionResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            var acceptTypes = context.RequestContext.HttpContext.Request.AcceptTypes;
            if (acceptTypes != null && acceptTypes.All(x => x == "application/json"))
                new JsonResult {Data = context.Controller.ViewData.Model, JsonRequestBehavior = JsonRequestBehavior.AllowGet}.ExecuteResult(context);
            else
                new ViewResult {ViewData = context.Controller.ViewData}.ExecuteResult(context);
        }
    }
}