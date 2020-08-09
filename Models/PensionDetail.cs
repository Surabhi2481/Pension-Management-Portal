using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pension_Management_Portal.Models
{
    public class PensionDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Serial Number")]
        public int SerialNumber { get; set; }
        [DisplayName("Date of Birth")]
        public DateTime DateOfBirth { get; set; }
        [DisplayName("Pan Number")]
        public string Pan { get; set; }
        [DisplayName("Aadhar Number")]
        public string AadharNumber { get; set; }
        [DisplayName("Pension Type")]
        public PensionType PensionType { get; set; }
        [DisplayName("Pension Amount")]
        public int PensionAmount { get; set; }
        [DisplayName("Status Code")]
        public int Status { get; set; }

      //  public HttpResponseMessage message { get; set; }

    }
    public enum PensionType1
    {
        Self = 1,
        Family = 2
    }
    public class Result
    {
        public string message { get; set; }
        public PensionDetail result { get; set; }

    }
}
