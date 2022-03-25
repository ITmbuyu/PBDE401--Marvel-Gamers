using PBDE401.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PBDE401.ViewModel
{
    public class SubscriptionViewModel
    {
        public int Id { get; set; }

        //Medicine Model Properties
        public int MedicineId { get; set; }
        public string MedicalNumber { get; set; }
        public string Name { get; set; }
        public string Origin { get; set; }
        public string Description { get; set; }
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }
        [Range(0, 1000)]
        [DisplayName("Availability")]
        public int Quanity { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMM dd yyyy}")]
        public DateTime? DateAdded { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [DisplayName("Expiry Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMM dd yyyy}")]
        public DateTime ExpiryDate { get; set; }


        //Subscription Model Properties
        [DataType(DataType.Date)]
        [DisplayName("Start Date")]
        [DisplayFormat(DataFormatString = "{0:MMM dd yyyy}")]
        public DateTime? StartDate { get; set; }

        [DisplayName("Scheduled End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMM dd yyyy}")]
        public DateTime? ScheduledEndDate { get; set; }

        [DisplayName("Additional End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMM dd yyyy}")]
        public DateTime? ActualEndDate { get; set; }

        [DisplayName("Prescription Price")]
        [DataType(DataType.Currency)]
        public double SubscriptionPrice { get; set; }

        [DisplayName("Duration")]
        public string SubscriptionDuration { get; set; }

        public String Status { get; set; }

        //Users Model Properties
        public string UserId { get; set; }
        public string Email { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Surname")]
        public string Surname { get; set; }

        [DisplayName("Full Name")]
        public string FullName { get { return FirstName + " " + Surname; } }

        [DisplayName("Birth Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMM dd yyyy}")]
        public DateTime? BirthDate { get; set; }

        //public string actionName
        //{
        //    get
        //    {
        //        if (Status.ToLower().Contains(SD.RequestedLower))
        //        {
        //            return "Approve";
        //        }
        //        if (Status.ToLower().Contains(SD.ApprovedLower))
        //        {
        //            return "PickUp";
        //        }
        //        if (Status.ToLower().Contains(SD.RentedLower))
        //        {
        //            return "Return";
        //        }
        //        return null;
        //    }
        //}
    }
}