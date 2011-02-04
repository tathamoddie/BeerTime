using System;
using System.Linq;
using System.Web.Mvc;

namespace BeerTime.WebUI.Controllers
{
    public class BeerController : Controller
    {
        public string FindNoon()
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

            return result.DisplayName;
        }
    }
}