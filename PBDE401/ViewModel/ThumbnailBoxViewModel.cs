using PBDE401.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBDE401.ViewModel
{
    public class ThumbnailBoxViewModel
    {
        public IEnumerable<ThumbnailModel> Thumbnails { get; set; }
    }
}