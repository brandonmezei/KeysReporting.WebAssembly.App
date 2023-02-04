using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeysReporting.WebAssembly.App.Shared.CPH
{
    public class SearchDto
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime? SearchDate { get; set; }
    }
}
