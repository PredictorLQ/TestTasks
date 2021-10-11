using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Picasso.Models;
using Picasso.Filter;
using Microsoft.EntityFrameworkCore;
using Picasso.Data;
using Picasso.Models.ViewModel;
using Picasso.Models.DataBase;
using System.Security.Policy;
using Picasso.Models.ClassModel;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections;
using System.Drawing;

namespace Picasso.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly DataBase basa;
        public HomeController(ILogger<HomeController> logger, DataBase basa)
        {
            this.basa = basa;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            int count = await basa.SearchUrl.CountAsync();
            ViewPageUrl ViewPageUrl = new()
            {
                SearchUrl = await basa.SearchUrl.Take(30).ToListAsync(),
            };
            ViewPageUrl.Navigation = new(count, 1, ViewPageUrl.SearchUrl.Count, 30, 5);
            return View(ViewPageUrl);
        }

        [Ajax]
        [HttpGet]
        public async Task<IActionResult> Search(string text)
        {
            if (!string.IsNullOrWhiteSpace(text) && Uri.IsWellFormedUriString(text, UriKind.RelativeOrAbsolute))
            {
                Uri uri = new(text);

                Parser parser = new(uri);
                await parser.GetPage();
                parser.GetInfoUris();

                List<SearchUrl> SearchUrl = await basa.SearchUrl.Where(u => parser.ListSearchUri.Contains(u.UrlToUpper)).ToListAsync();
                for (int i = 0; i < SearchUrl.Count; i++)
                {
                    int index = parser.SearchUrl.FindIndex(u => u.UrlToUpper == SearchUrl[i].UrlToUpper);
                    if (index > 0)
                    {
                        SearchUrl[i].Update(parser.SearchUrl[index]);
                        parser.SearchUrl.RemoveAt(index);
                    }
                }

                SearchUrl.AddRange(parser.SearchUrl);
                basa.SearchUrl.UpdateRange(SearchUrl);
                await basa.SaveChangesAsync();

                return await PageUrl(text);
            }
            return BadRequest();
        }
        [Ajax]
        [HttpGet]
        public async Task<IActionResult> PageUrl(string text, int page = 0, int limit = 30)
        {
            if (limit == 0) limit = 30;
            if (page < 0) page = 0;

            string searchText = text.ToUpper();

            int start = page * limit, count = await basa.SearchUrl.CountAsync(u => u.UrlToUpper == searchText);
            page++;

            ViewPageUrl ViewPageUrl = new()
            {
                SearchUrl = await basa.SearchUrl.Where(u => u.UrlToUpper == searchText).Skip(start).Take(limit).ToListAsync(),
            };
            ViewPageUrl.Navigation = new(count, page, ViewPageUrl.SearchUrl.Count, limit, 5);
            return PartialView("~/Views/Home/PageUrl.cshtml", ViewPageUrl);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
