using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Picasso.Models.DataBase;

namespace Picasso.Data
{
    public class DataBase : DbContext
    {
        public DataBase(DbContextOptions<DataBase> options) : base(options) { }
        
        public DbSet<SearchUrl> SearchUrl { get; set; }
    }
}
