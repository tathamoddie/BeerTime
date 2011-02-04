using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BeerTime.WebUI.Models;

namespace BeerTime.WebUI.Controllers
{
    public class BeerController : Controller
    {
        private static readonly Dictionary<double, IEnumerable<string>> Mappings
            = new Dictionary<double, IEnumerable<string>>
                  {
                      { -12, new[]{ "the middle of the Pacific ocean" } },
                      { -11, new[] { "Samoa" } },
                      { -10, new[] { "Hawaii" } },
                      { -9, new[] { "Alaska" } },
                      { -8, new[] { "Los Angeles", "Hotel California" } },
                      { -7, new[] { "Arizona" } },
                      {-6, new[] {"Guadalajara"}},
                      {-5, new[] {"Indiana"}},
                      {-4.5, new[] {"Caracas"}},
                      {-4, new[] { "Paraguay"}},
                      {-3.5, new[] { "Newfoundland"}},
                      {-3, new[] { "Buenos Aires"}},
                      {-2, new[] { "the middle of the Atlantic ocean"}},
                      {-1, new[] { "a random island off the coast of Senegal"}},
                      {0, new[] { "London"}},
                      {1, new[] { "Amsterdam"}},
                      {2, new[] {"Helsinki"}},
                      {3, new[] {"Moscow (or a Vodka)"}},
                      {3.5, new[] {"Tehran"}},
                      {4, new[] {"Abu Dhabi"}},
                      {4.5, new[] {"Kabul, the dry state"}},
                      {5, new[] {"Islamabad"}},
                      {5.5, new[] {"Sri Jayawardenepura"}},
                      {5.75, new[] {"the shadow of Mt Everest"}},
                      {6, new[] {"Novosibirsk"}},
                      {6.5, new[] {"Yangon "}},
                      {7, new[] {"Bangkok"}},
                      {8, new[] { "Yallabatharra, WA"}},
                      {9, new[] {"Osaka"}},
                      {9.5, new[] {"Uluṟu"}},
                      { 10, new[] { "Sydney" } },
                      {11, new[]{"New Caledonia"}},
                      {12, new[] {"Fiji"}},
                      {13, new[] {"Nuku'alofa"}}
                  };

        [HttpGet]
        public ActionResult FindNoon()
        {
            var utcNow = DateTime.UtcNow;
            var result = FindTimeZoneInfo(utcNow);
            var hours = result.GetUtcOffset(utcNow).Hours;
            var locationName = FindRandomLocation(hours, result.DisplayName);

            ViewData.Model = new FindNoonViewModel
                                 {
                                     LocationName = locationName
                                 };
            return new DynamicActionResult();
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