using Microsoft.AspNet.Identity;
using PBDE401.Models;
using PBDE401.Utility;
using PBDE401.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Net;
using System.Net.Mail;

namespace PBDE401.Controllers
{
    [Authorize]
    public class PrescriptionController : Controller
    {
        private ApplicationDbContext db;

        public PrescriptionController()
        {
            db = ApplicationDbContext.Create();
        }

        public ActionResult Create(int id, string medicalNumber, string name)
        {
            SubscriptionViewModel model = new SubscriptionViewModel
            {
                MedicineId = id,
                Name = name,
                MedicalNumber = medicalNumber
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SubscriptionViewModel subscription)
        {
            var patient = db.Users.Where(u => u.Email == subscription.Email).FirstOrDefault();
            Medicine medicineToGet = db.Medicine.Find(subscription.MedicineId);
            double prescriptionPrice = 0.0;

            if (subscription.SubscriptionDuration == "1")
            {
                prescriptionPrice = medicineToGet.Price;
            }
            else
            {
                prescriptionPrice = medicineToGet.Price * 3;
            }

            Subscription prescription = new Subscription
            {
                MedicineId = subscription.MedicineId,
                UserId = patient.Id,
                SubscriptionDuration = subscription.SubscriptionDuration,
                SubscriptionPrice = prescriptionPrice,
                Status = Subscription.StatusEnum.Prescribed
            };

            db.Subscriptions.Add(prescription);
            var medicineInDb = db.Medicine.SingleOrDefault(c => c.Id == subscription.MedicineId);

            medicineInDb.Quantity -= 1;

            db.SaveChanges();
            return RedirectToAction("Index", "Prescription");
        }


            // GET: Prescription
            public ActionResult Index(int? pageNumber, string option = null, string search = null)
        {
            string userid = User.Identity.GetUserId();

            var model = from s in db.Subscriptions
                        join m in db.Medicine on s.MedicineId equals m.Id
                        join u in db.Users on s.UserId equals u.Id
                        select new SubscriptionViewModel
                        {
                            Id = s.Id,
                            MedicineId = m.Id,
                            SubscriptionPrice = s.SubscriptionPrice,
                            Price = m.Price,
                            FirstName = u.FirstName,
                            Surname = u.Surname,
                            BirthDate = u.BirthDate,
                            ScheduledEndDate = s.ScheduledEndDate,
                            Origin = m.Origin,
                            Quanity = m.Quantity,
                            DateAdded = m.DateAdded,
                            Description = m.Description,
                            Email = u.Email,
                            StartDate = s.StartDate,
                            CategoryId = m.CategoryId,
                            Category = db.Categories.Where(g => g.Id.Equals(m.CategoryId)).FirstOrDefault(),
                            MedicalNumber = m.MedicalNumber,
                            ImageUrl = m.ImageUrl,
                            ExpiryDate = m.ExpiryDate,
                            SubscriptionDuration = s.SubscriptionDuration,
                            Status = s.Status.ToString(),
                            Name = m.Name,
                            UserId = u.Id

                        };

            if (option == "email" && search.Length > 0)
            {
                model = model.Where(u => u.Email.Contains(search));
            }
            if (option == "fullname" && search.Length > 0)
            {
                model = model.Where(u => u.FullName.Contains(search));
            }
            if (option == "status" && search.Length > 0)
            {
                model = model.Where(u => u.Status.Contains(search));
            }

            if (!User.IsInRole(SD.DoctorRole))
            {
                model = model.Where(u => u.UserId.Equals(userid));
            }

            return View(model.ToList().ToPagedList(pageNumber ?? 1, 5));
        }

        [HttpPost]
        public ActionResult RequestPrescription(SubscriptionViewModel medicine)
        {
            var userid = User.Identity.GetUserId();
            Medicine medicineToGet = db.Medicine.Find(medicine.MedicineId);
            double prescriptionPrice = 0.0;

            if (userid != null)
            {
                if (medicine.SubscriptionDuration == "1")
                {
                    prescriptionPrice = medicineToGet.Price;
                }
                else
                {
                    prescriptionPrice = medicineToGet.Price * 3;
                }

                var userInDb = db.Users.SingleOrDefault(c => c.Id == userid);

                Subscription subscription = new Subscription
                {
                    MedicineId = medicineToGet.Id,
                    UserId = userid,
                    SubscriptionDuration = medicine.SubscriptionDuration,
                    SubscriptionPrice = prescriptionPrice,
                    Status = Subscription.StatusEnum.Requested
                };

                db.Subscriptions.Add(subscription);
                var medicineInDb = db.Medicine.SingleOrDefault(c => c.Id == medicine.MedicineId);

                medicineInDb.Quantity -= 1;

                db.SaveChanges();
                return RedirectToAction("Index", "Prescription");
            }


            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subscription prescription = db.Subscriptions.Find(id);

            var model = getVMFromPrescription(prescription);
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        //Decline Method
        public ActionResult Decline(int? id)
        {
            Subscription prescription = db.Subscriptions.Find(id);
            prescription.Status = Subscription.StatusEnum.Cancelled;
            Medicine medicineInDb = db.Medicine.Find(prescription.MedicineId);
            medicineInDb.Quantity += 1;
            db.SaveChanges();

            //Email user about decision
            var userEmail = db.Users.SingleOrDefault(c => c.Id == prescription.UserId).Email;
            //email code here
            string message =
                $"Hi there, \n\n" +
                $"You have made a request with GamersMed Weed . Here are the details: \n\n" +
                $"Sorry your request for the medicine has been declined with GamersMed Weed . Here are the details: \n\n" +
                $"Your Prescription Status is: {prescription.Status} \n" +
                $"Your Request Number is: {medicineInDb} \n" +
                $"reason for declining includes Age specifications, or not enough documentation for proof of Iidentification \n\n" +
                $"Kind Regards";

            // Sendemail
            var senderEmail = new MailAddress("gamersmedweed@gmail.com", "Sharac Phone Repair Tech");
            var recieverMail = new MailAddress(userEmail, "Client");
            var password = "hello123@B4";
            var sub = $"New Request #{id}";
            var body = message;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                // EnableSsl = false,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };
            using (var mess = new MailMessage(senderEmail, recieverMail)
            {
                Subject = sub,
                Body = body
            })
            {
                smtp.Send(mess);
            }
            //Email user about decision

            return RedirectToAction("Index");
        }

        //Approve Method
        public ActionResult Approve(int? id)
        {
            Subscription prescription = db.Subscriptions.Find(id);
            prescription.Status = Subscription.StatusEnum.Prescribed;
            db.SaveChanges();

            //Email user about decision
            var userEmail = db.Users.SingleOrDefault(c => c.Id == prescription.UserId).Email;
            //email code here
            string message =
                $"Hi there, \n\n" +
                $"your request for the medicine has been aaproved with GamersMed Weed . Here are the details: \n\n" +
                $"Your Prescription Status is: {prescription.Status} \n" +
                $"Please proceed for pick up of your medication \n\n" +
                $"Kind Regards";

            // Sendemail
            var senderEmail = new MailAddress("gamersmedweed@gmail.com", "GamersMed Pharmacy");
            var recieverMail = new MailAddress(userEmail, "Client");
            var password = "hello123@B4";
            var sub = $"New Request #{id}";
            var body = message;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                // EnableSsl = false,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };
            using (var mess = new MailMessage(senderEmail, recieverMail)
            {
                Subject = sub,
                Body = body
            })
            {
                smtp.Send(mess);
            }
            //Email user about decision

            return RedirectToAction("Index");
        }

        public ActionResult PickUp(int? id)
        {
            Subscription prescription = db.Subscriptions.Find(id);
            prescription.Status = Subscription.StatusEnum.PickedUp;
            prescription.StartDate = DateTime.Now;

            if (prescription.SubscriptionDuration == "1")
            {
                prescription.ScheduledEndDate = DateTime.Now.AddMonths(1);
            }
            else
            {
                prescription.ScheduledEndDate = DateTime.Now.AddMonths(3);
            }

            db.SaveChanges();

            //Email user about decision
            var userEmail = db.Users.SingleOrDefault(c => c.Id == prescription.UserId).Email;
            //email code here
            string message =
                $"Hi there, \n\n" +
                $"You have a pickup for medication  with GamersMed Weed . Here are the details: \n\n" +
                $"Your Prescription Status is: {prescription.Status} \n" +
                $"Please come and pick up your medication \n\n" +
                $"Kind Regards";

            // Sendemail
            var senderEmail = new MailAddress("gamersmedweed@gmail.com", "GamersMed Pharmacy");
            var recieverMail = new MailAddress(userEmail, "Client");
            var password = "hello123@B4";
            var sub = $"New Request #{id}";
            var body = message;


            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                // EnableSsl = false,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };
            using (var mess = new MailMessage(senderEmail, recieverMail)
            {
                Subject = sub,
                Body = body
            })
            {
                smtp.Send(mess);
            }
            //Email user about decision

            return RedirectToAction("OnceOff", "Payment", new { id = id });
        }

        //Close Method
        public ActionResult Close(int? id)
        {
            Subscription prescription = db.Subscriptions.Find(id);
            prescription.Status = Subscription.StatusEnum.Closed;
            prescription.ActualEndDate = DateTime.Now;
            db.SaveChanges();

            //Email user about decision
            var userEmail = db.Users.SingleOrDefault(c => c.Id == prescription.UserId).Email;
            //email code here
            string message =
                $"Hi there, \n\n" +
                $"This Email is thank you for dealing with GamersMed Weed . Here are the details: \n\n" +
                $"Your Prescription Status is: {prescription.Status} \n" +
                $"Kind Regards";

            // Sendemail
            var senderEmail = new MailAddress("gamersmedweed@gmail.com", "GamersMed Pharmacy");
            var recieverMail = new MailAddress(userEmail, "Client");
            var password = "hello123@B4";
            var sub = $"New Request #{id}";
            var body = message;


            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                // EnableSsl = false,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };
            using (var mess = new MailMessage(senderEmail, recieverMail)
            {
                Subject = sub,
                Body = body
            })
            {
                smtp.Send(mess);
            }
            //Email user about decision

            return RedirectToAction("Index");
        }

        //Delete GET Method
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Subscription prescription = db.Subscriptions.Find(id);

            var model = getVMFromPrescription(prescription);

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int Id)
        {
            if (Id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                Subscription prescription = db.Subscriptions.Find(Id);
                db.Subscriptions.Remove(prescription);

                var medicineInDb = db.Medicine.Where(b => b.Id.Equals(prescription.MedicineId)).FirstOrDefault();
                var userInDb = db.Users.SingleOrDefault(c => c.Id == prescription.UserId);

                if (prescription.Status.ToString().ToLower().Equals(SD.RequestedLower))
                {
                    medicineInDb.Quantity += 1;
                }

                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        private SubscriptionViewModel getVMFromPrescription(Subscription prescription)
        {
            Medicine medicineSelected = db.Medicine.Where(b => b.Id == prescription.MedicineId).FirstOrDefault();
            var userDetails = from u in db.Users
                              where u.Id.Equals(prescription.UserId)
                              select new { u.Id, u.FirstName, u.Surname, u.BirthDate, u.Email };

            SubscriptionViewModel model = new SubscriptionViewModel
            {
                Id = prescription.Id,
                MedicineId = medicineSelected.Id,
                SubscriptionPrice = prescription.SubscriptionPrice,
                Price = medicineSelected.Price,
                FirstName = userDetails.ToList()[0].FirstName,
                Surname = userDetails.ToList()[0].Surname,
                BirthDate = userDetails.ToList()[0].BirthDate,
                ScheduledEndDate = prescription.ScheduledEndDate,
                Origin = medicineSelected.Origin,
                StartDate = prescription.StartDate,
                Quanity = medicineSelected.Quantity,
                DateAdded = medicineSelected.DateAdded,
                Description = medicineSelected.Description,
                Email = userDetails.ToList()[0].Email,
                CategoryId = medicineSelected.CategoryId,
                Category = db.Categories.FirstOrDefault(g => g.Id.Equals(medicineSelected.CategoryId)),
                MedicalNumber = medicineSelected.MedicalNumber,
                ImageUrl = medicineSelected.ImageUrl,
                ExpiryDate = medicineSelected.ExpiryDate,
                SubscriptionDuration = prescription.SubscriptionDuration,
                Status = prescription.Status.ToString(),
                Name = medicineSelected.Name,
                UserId = userDetails.ToList()[0].Id
            };

            return model;
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