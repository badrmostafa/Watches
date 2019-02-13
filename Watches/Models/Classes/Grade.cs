using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Watches.Models.Classes
{
    public class Grade
    {
        public int GradeID { get; set; }
        public string Image { get; set; }
        public string Head { get; set; }
        public string Description { get; set; }
       
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}