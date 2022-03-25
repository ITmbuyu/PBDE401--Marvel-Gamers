using Microsoft.AspNet.Identity;
using PBDE401.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PBDE401.Utility;
using PBDE401.ViewModel;

namespace PBDE401.Controllers
{
    public class MedicineDetailController : Controller
    {
        private ApplicationDbContext db;

        public MedicineDetailController()
        {
            db = ApplicationDbContext.Create();
        }
        // GET: BookDetail
        public ActionResult Index(int id)
        {
            var userid = User.Identity.GetUserId();
            var user = db.Users.FirstOrDefault(u => u.Id == userid);

            var medicineModel = db.Medicine.Include(m => m.Category).SingleOrDefault(m => m.Id == id);

            var subscriptionPrice = medicineModel.Price;

            SubscriptionViewModel model = new SubscriptionViewModel
            {
                MedicineId = medicineModel.Id,
                MedicalNumber = medicineModel.MedicalNumber,
                Origin = medicineModel.Origin,
                Quanity = medicineModel.Quantity,
                DateAdded = medicineModel.DateAdded,
                Description = medicineModel.Description,
                Category = db.Categories.FirstOrDefault(g => g.Id.Equals(medicineModel.CategoryId)),
                CategoryId = medicineModel.CategoryId,
                ImageUrl = medicineModel.ImageUrl,
                Price = medicineModel.Price,
                ExpiryDate = medicineModel.ExpiryDate,
                Name = medicineModel.Name,
                UserId = userid,
                SubscriptionPrice = subscriptionPrice,            

            };

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
        }
    }
}