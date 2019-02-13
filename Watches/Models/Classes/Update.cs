using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Watches.Models.Classes
{
    public class Update
    {
        public int UpdateID { get; set; }
        public string Head { get; set; }
        public string Description { get; set; }
        
        [Timestamp]
        public byte[] RowVersion { get; set; }

    }
}