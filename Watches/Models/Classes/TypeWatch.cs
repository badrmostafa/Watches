using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Watches.Models.Classes
{
    public class TypeWatch
    {
        public int TypeWatchID { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public int ChooseID { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        //NavigationProperty
        public Choose Choose { get; set; }
        public List<Product> Products { get; set; }
    }
}