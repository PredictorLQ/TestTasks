using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

using Picasso.Models.JsonModel;

namespace Picasso.Models.DataBase
{
    public class SearchUrl
    {
        public SearchUrl() { }
        public SearchUrl(JsonDomainsdb JsonDomainsdb, Uri uri) {
            Url = uri.AbsoluteUri;
            UrlToUpper = uri.AbsoluteUri.ToUpper();
            Domain = uri.Host;
        }
        [Key]
        public int Id { get; set; }
        [Column("url")]
        public string Url { get; set; }
        [Column("url_to_upper")]
        public string UrlToUpper { get; set; }
        [Column("url")]
        public string Domain { get; set; }
        [Column("country")]
        public string Country { get; set; }
        [Column("isDead")]
        public string IsDead { get; set; }
        [Column("A")]
        public string A { get; set; }
        [Column("NS")]
        public string NS { get; set; }
        [Column("CNAME")]
        public string CNAME { get; set; }
        [Column("MX")]
        public string MX { get; set; }
        [Column("TXT")]
        public string TXT { get; set; }
        [Column("create_date")]
        public DateTime DateAdd { get; set; } = DateTime.UtcNow;
        [Column("update_date")]
        public DateTime DateUpdate { get; set; } = DateTime.UtcNow;
        public void Update(SearchUrl searchUrl)
        {
            Country = searchUrl.Country;
            IsDead = searchUrl.IsDead;
            A = searchUrl.A;
            NS = searchUrl.NS;
            CNAME = searchUrl.CNAME;
            MX = searchUrl.MX;
            TXT = searchUrl.TXT;
            DateUpdate = searchUrl.DateUpdate;
        }
    }
}
