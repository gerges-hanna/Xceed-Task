using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Xceed_Task.ViewModels
{
    public class ProductViewModel
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }

        [DisplayName("Start Date")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [DisplayName("Duration (hours)")]
        [Range(1, 9999)]
        public int DurationInHours { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [DisplayName("Category")]
        [Required]
        public int CategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }

        // Optional display fields (not posted)
        public DateTime? CreationDate { get; set; }
        public string CreatedByEmail { get; set; }
    }
}
