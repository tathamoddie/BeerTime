using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BeerTime.WebUI.Controllers
{
    public class BeerController : Controller
    {
        private static readonly Dictionary<double, IEnumerable<string>> Mappings
            = new Dictionary<double, IEnumerable<string>>
                  {
                      { -12, new[]{ "Middle of the ocean" } },
                      { -11, new[] { "Samoa" } },
                      { -10, new[] { "Hawaii" } },
                      { -9, new[] { "Alaska" } },
                      { -8, new[] { "Los Angeles", "Hotel California" } },
                      { -7, new[] { "Arizona" } },
                      {-6, new[] {"Guadalajara"}},
                      {-5, new[] {"Indiana"}},
                      {-4.5, new[] {"Caracas"}},


                      { 10, new[] { "Sydney" } }
                  };

        [HttpGet]
        public ActionResult FindNoon()
        {
            var utcNow = DateTime.UtcNow;

            var result = FindTimeZoneInfo(utcNow);

            var hours = result.GetUtcOffset(utcNow).Hours;
            ViewBag.CityName = FindRandomLocation(hours, result.DisplayName);

            return View();
        }

        private static TimeZoneInfo FindTimeZoneInfo(DateTime utcNow)
        {
            return TimeZoneInfo
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
        }

        private static string FindRandomLocation(int hours, string defaultValue)
        {
            if (!Mappings.ContainsKey(hours))
            {
                return defaultValue;
            }

            return Mappings[hours].First();
        }
    }
}