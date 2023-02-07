using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeysReporting.WebAssembly.App.Shared.CPH
{
    public class AddProjectDto
    {
        private DateTime _ReportDate;

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReportDate { get { return _ReportDate; } set { _ReportDate = value.Date; } }

        [Required]
        [StringLength(255)]
        public string ProjectCode { get; set; }
    }
}
