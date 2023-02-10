using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeysReporting.WebAssembly.App.Shared.CPH
{
    public class EditTimeDto
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Agent { get; set; }
    }
}
