using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace PBDE401.Models
{
    public class Medicine
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string MedicalNumber { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Origin { get; set; }

        [Required]
        public string Description { get; set; }

        
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        [Required]
        [Range(0, 1000)]
        public int Quantity { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? DateAdded { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public Category Category { get; set; }


        [Required]
        [DataType(DataType.Date)]
        public DateTime ExpiryDate { get; set; }
    }
}