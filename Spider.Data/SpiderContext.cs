using Spider.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Data
{
  
        public class SpiderContext : DbContext
        {
            public SpiderContext()
                : base("SpiderDb")
            {
            }

            public DbSet<Category> Categories { get; set; }
            public DbSet<MainProduct> MainProducts { get; set; }

            public DbSet<SlaveProduct> SlaveProducts { get; set; }
            public DbSet<PicturePath> Paths { get; set; }
            public DbSet<VariationColor> Colors { get; set; }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
            }
        }
    }

