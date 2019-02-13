using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Watches.Models.Classes
{
    public class Watch
    {
        public int WatchID { get; set; }
        public string Head { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Button { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}