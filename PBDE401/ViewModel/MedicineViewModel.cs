using PBDE401.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBDE401.ViewModel
{
    public class MedicineViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public Medicine Medicine { get; set; }
    }
}