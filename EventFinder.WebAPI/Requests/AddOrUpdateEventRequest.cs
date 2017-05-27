using System;
using System.ComponentModel.DataAnnotations;

namespace EventFinder.WebAPI.Requests
{
    public class AddOrUpdateEventRequest
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(4000, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 100)]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}