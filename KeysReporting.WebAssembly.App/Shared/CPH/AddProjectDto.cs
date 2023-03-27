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
        [Required]
        [DataType(DataType.Date)]
        public DateTime? ReportDate { get; set; }

        [Required]
        [StringLength(255)]
        public string ProjectCode { get; set; }
    }
}
