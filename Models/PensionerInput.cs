using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pension_Management_Portal.Models
{
    public class PensionerInput
    {
        [Required]
        [DisplayName("Pensioner Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Provide Valid Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Date Of Birth")]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [RegularExpression(@"^[0-9,A-Z]{10}$",
            ErrorMessage = "Alphanumeric are allowed and length must be 10s.")]
        [StringLength(10)]
        [DisplayName("PAN Number")]
        public string Pan { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{12}$",
            ErrorMessage = "only digits are allowed and length must be 12.")]                          
        [StringLength(12)]
        [DisplayName("Aadhar Number")]
        public string AadharNumber { get; set; }
        [Required]
        [DisplayName("Pension Type")]
        public PensionType PensionType { get; set; }
    }

    public enum PensionType
    {
        Self=1,
        Family=2
    }
}
