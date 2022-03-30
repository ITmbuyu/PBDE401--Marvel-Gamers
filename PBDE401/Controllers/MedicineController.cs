using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PBDE401.Models;
using PBDE401.ViewModel;

namespace PBDE401.Controllers
{
    //[Authorize(Roles = SD.AdminUserRole)]
    public class MedicineController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Medicine
        public ActionResult Index()
        {
            var medicine = db.Medicine.Include(b => b.Category);
            return View(medicine.ToList());
        }


        // GET: Medicine/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicine medicine = db.Medicine.Find(id);
            if (medicine == null)
            {
                return HttpNotFound();
            }

            var model = new MedicineViewModel
            {
                Medicine = medicine,
                Categories = db.Categories.ToList()
            };
            return View(model);
        }

        // GET: Medicine/Create
        public ActionResult Create()
        {
            var categories = db.Categories.ToList();
            var model = new MedicineViewModel
            {
                Categories = categories
            };
            return View(model);
        }

        // POST: Medicine/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MedicineViewModel medicineVM, HttpPostedFileBase file)
        {
            var medicine = new Medicine
            {
                Origin = medicineVM.Medicine.Origin,
                Quantity = medicineVM.Medicine.Quantity,
                DateAdded = medicineVM.Medicine.DateAdded,
                Description = medicineVM.Medicine.Description,
                Category = medicineVM.Medicine.Category,
                CategoryId = medicineVM.Medicine.CategoryId,
                ImageUrl = "Placeholder",
                MedicalNumber = medicineVM.Medicine.MedicalNumber,
                Price = medicineVM.Medicine.Price,
                ExpiryDate = medicineVM.Medicine.ExpiryDate,
                Name = medicineVM.Medicine.Name
            };
            if (ModelState.IsValid)
            {
                db.Medicine.Add(medicine);
                db.SaveChanges();

                var uploadsDir = new DirectoryInfo(string.Format("{0}Uploads", Server.MapPath(@"\")));

                if (file != null && file.ContentLength > 0)
                {
                    string ext = file.ContentType.ToLower();

                    if (ext != "image/jpg" &&
                        ext != "image/jpeg" &&
                        ext != "image/pjpeg" &&
                        ext != "image/gif" &&
                        ext != "image/x-png" &&
                        ext != "image/png")
                    {
                        ModelState.AddModelError("", "The image was not uploaded - wrong image extension.");
                        return View("Index", medicineVM);
                    }

                    string imageName = medicine.Id + ".jpg";
                    var path = string.Format("{0}\\{1}", uploadsDir, imageName);
                    file.SaveAs(path);
                }

                Medicine med = db.Medicine.Find(medicine.Id);
                med.ImageUrl = med.Id + ".jpg";
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            medicineVM.Categories = db.Categories.ToList();
            return View(medicineVM);
        }

        // GET: Medicine/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicine medicine = db.Medicine.Find(id);
            if (medicine == null)
            {
                return HttpNotFound();
            }

            var model = new MedicineViewModel
            {
                Medicine = medicine,
                Categories = db.Categories.ToList()
            };
            return View(model);
        }

        // POST: Medicine/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(MedicineViewModel medicineVM, HttpPostedFileBase file)
        {
            var medicine = new Medicine
            {
                Id = medicineVM.Medicine.Id,
                Origin = medicineVM.Medicine.Origin,
                Quantity = medicineVM.Medicine.Quantity,
                DateAdded = medicineVM.Medicine.DateAdded,
                Description = medicineVM.Medicine.Description,
                Category = medicineVM.Medicine.Category,
                CategoryId = medicineVM.Medicine.CategoryId,
                ImageUrl = "Placeholder",
                MedicalNumber = medicineVM.Medicine.MedicalNumber,
                Price = medicineVM.Medicine.Price,
                ExpiryDate = medicineVM.Medicine.ExpiryDate,
                Name = medicineVM.Medicine.Name
            };

            if (ModelState.IsValid)
            {
                var uploadsDir = new DirectoryInfo(string.Format("{0}Uploads", Server.MapPath(@"\")));
                if (file != null && file.ContentLength > 0)
                {
                    string ext = file.ContentType.ToLower();

                    if (ext != "image/jpg" &&
                        ext != "image/jpeg" &&
                        ext != "image/pjpeg" &&
                        ext != "image/gif" &&
                        ext != "image/x-png" &&
                        ext != "image/png")
                    {
                        ModelState.AddModelError("", "The image was not uploaded - wrong image extension.");
                        return View("Index", medicineVM);
                    }

                    string imageName = medicine.Id + ".jpg";
                    var path = string.Format("{0}\\{1}", uploadsDir, imageName);
                    file.SaveAs(path);
                }

                medicine.ImageUrl = medicine.Id + ".jpg";
                db.Entry(medicine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            medicineVM.Categories = db.Categories.ToList();
            return View(medicineVM);
        }

        // GET: Medicine/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicine medicine = db.Medicine.Find(id);
            if (medicine == null)
            {
                return HttpNotFound();
            }
            var model = new MedicineViewModel
            {
                Medicine = medicine,
                Categories = db.Categories.ToList()
            };
            return View(model);
        }

        // POST: Medicine/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Medicine medicine = db.Medicine.Find(id);
            db.Medicine.Remove(medicine);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}