using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace Watches.Models.Classes
{
    public class WatchContext:DbContext
    {
        //ConnectionString
        public WatchContext():base("WatchContext")
        { }
        //DbSet<>
        public DbSet<Watch> Watches { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Choose> Chooses { get; set; }
        public DbSet<TypeWatch> TypesWatches { get; set; }
        public DbSet<Update> Updates { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}