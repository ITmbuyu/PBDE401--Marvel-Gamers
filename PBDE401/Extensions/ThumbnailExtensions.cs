using PBDE401.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBDE401.Extensions
{
    public static class ThumbnailExtensions
    {
        public static IEnumerable<ThumbnailModel> GetMedicineThumbnail(this List<ThumbnailModel> thumbnails, ApplicationDbContext db = null, string search = null)
        {
            try
            {
                if (db == null) db = ApplicationDbContext.Create();

                thumbnails = (from m in db.Medicine
                              select new ThumbnailModel
                              {
                                  MedicineId = m.Id,
                                  Name = m.Name,
                                  Description = m.Description,
                                  ImageUrl = m.ImageUrl,
                                  Link = "/MedicineDetail/Index/" + m.Id,
                              }).ToList();

                if (search != null)
                {
                    return thumbnails.Where(t => t.Name.ToLower().Contains(search.ToLower())).OrderBy(t => t.Name);
                }
            }
            catch (Exception ex)
            {

            }
            return thumbnails.OrderBy(t => t.Name);

        }
    }
}