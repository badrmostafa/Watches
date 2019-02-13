using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Watches.Models.Classes
{
    public class Choose
    {
        public int ChooseID { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Head { get; set; }
        public string Description { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        //NavigationProperty
        public List<TypeWatch> TypesWatches { get; set; }
    }
}