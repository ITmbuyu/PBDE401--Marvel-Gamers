using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBDE401.Models
{
    public class ThumbnailModel
    {
        public int MedicineId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Link { get; set; }
    }
}