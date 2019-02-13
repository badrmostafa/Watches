using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Watches.Models.Classes
{
    public class Feature
    {
        public int FeatureID { get; set; }
        public string Title { get; set; }
        public string Head { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        //NavigationProperty
        public List<Product> Products { get; set; }
    }
}