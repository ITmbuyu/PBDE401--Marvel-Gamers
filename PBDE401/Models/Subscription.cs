using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PBDE401.Models
{
    public class Subscription
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int MedicineId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ScheduledEndDate { get; set; }
        public DateTime? ActualEndDate { get; set; }

        [Required]
        public double SubscriptionPrice { get; set; }

        [Required]
        public string SubscriptionDuration { get; set; }

        [Required]
        public StatusEnum Status { get; set; }

        public enum StatusEnum
        {
            Requested,
            Cancelled,
            Prescribed,
            PickedUp,
            Closed
        }
    }
}