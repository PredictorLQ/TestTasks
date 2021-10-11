using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Picasso.Models.ClassModel;
using Picasso.Models.DataBase;

namespace Picasso.Models.ViewModel
{
    public class ViewPageUrl
    {
        public Navigation Navigation { get; set; }
        public List<SearchUrl> SearchUrl { get; set; } = new();
    }
}
