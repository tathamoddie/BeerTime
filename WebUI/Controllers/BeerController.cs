using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BeerTime.WebUI.Controllers
{
    public class BeerController : Controller
    {
        static Dictionary<int, string> _mappings
            = new Dictionary<int, string>
                  {
                      { 10, "Sydney" }
                  };

        public ActionResult FindNoon()
        {
            var utcNow = DateTime.UtcNow;

            var result = TimeZoneInfo
                .GetSystemTimeZones()
                .Select(tz => new
                {
                    TimeZone = tz,
                    LocalTimeOfDay = TimeZoneInfo.ConvertTimeFromUtc(utcNow, tz).TimeOfDay
                })
                .Where(t => t.LocalTimeOfDay.Hours >= 12)
                .OrderBy(t => t.LocalTimeOfDay.Hours)
                .ThenBy(t => t.LocalTimeOfDay.Minutes)
                .ThenBy(t => t.TimeZone.StandardName)
                .Select(t => t.TimeZone)
                .First();

            var hours = result.GetUtcOffset(utcNow).Hours;
            ViewBag.CityName = _mappings.ContainsKey(hours) ? _mappings[hours] : result.DisplayName;

            return View();
        }
    }
}