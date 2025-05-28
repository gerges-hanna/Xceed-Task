using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;

namespace Xceed_Task.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayName("Creation Date")]
        public DateTime CreationDate { get; set; }

        [ValidateNever]
        [DisplayName("Created By User Id")]
        public string CreatedByUserId { get; set; }

        [ValidateNever]
        [DisplayName("Created By User")]

        public IdentityUser CreatedByUser { get; set; }

        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }

        [DisplayName("Duration In Hours")]
        public int DurationInHours { get; set; }
        public decimal Price { get; set; }

        [DisplayName("Category Id")]
        public int CategoryId { get; set; }

        [ValidateNever]
        public Category Category { get; set; }
    }

}
