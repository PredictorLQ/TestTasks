using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

using AngleSharp.Dom;
using AngleSharp;
using System.IO;
using System.Net;
using System.Threading;
using Picasso.Models.DataBase;
using Picasso.Models.JsonModel;
using System.Text.Json;

namespace Picasso.Models.ClassModel
{
    public class Parser
    {
        private Uri uri { get; set; }
        private string NameDomen { get; set; }
        private List<string> listNewUri { get; set; } = new();
        private readonly string ApiUri = "https://api.domainsdb.info/v1/domains/search?domain=";
        public List<SearchUrl> SearchUrl { get; set; } = new();
        public List<string> ListSearchUri { get; set; } = new();
        public Parser(Uri Uri)
        {
            uri = Uri;
            NameDomen = uri.OriginalString.Replace(uri.PathAndQuery, "");
        }
        public async Task GetPage()
        {

            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(uri.AbsoluteUri);
            var elements = document.QuerySelectorAll("a[href]");
            foreach (var item in elements)
            {
                var attr = item.GetAttribute("href");
                if (!string.IsNullOrWhiteSpace(attr) && attr.Length > 5)
                {
                    attr = UpdateUri(attr);
                    if (!listNewUri.Contains(attr) && Uri.IsWellFormedUriString(attr, UriKind.RelativeOrAbsolute)) listNewUri.Add(attr);
                }
            }
        }
        private string UpdateUri(string text) => text[0] is ('/' or '\\') ? $"{NameDomen}/{text[1..]}" : text;
        public void GetInfoUris()
        {
            Task[] tasks = listNewUri.Select(u => Task.Run(() => GetInfoUri(u))).ToArray();
            Task.WaitAll();
            ListSearchUri = SearchUrl.Select(u => u.UrlToUpper).ToList();
        }
        private async Task GetInfoUri(string text)
        {
            WebRequest request = WebRequest.Create($"{ApiUri}{text}");
            WebResponse response = await request.GetResponseAsync();
            using (Stream stream = response.GetResponseStream())
            {
                using StreamReader reader = new(stream);
                try
                {
                    JsonDomainsdb JsonDomainsdb = JsonSerializer.Deserialize<JsonDomainsdb>(reader.ReadToEnd());
                    SearchUrl.Add(new(JsonDomainsdb, new Uri(text)));
                }
                catch { }
            }
            response.Close();
        }
    }
}
