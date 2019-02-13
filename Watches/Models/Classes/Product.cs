using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Watches.Models.Classes
{
    public class Product
    {
        public int ProductID { get; set; }
        public int FeatureID { get; set; }
        public int TypeWatchID { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        //NavigationProperty
        public Feature Feature { get; set; }
        public TypeWatch TypeWatch { get; set; }
    }
}