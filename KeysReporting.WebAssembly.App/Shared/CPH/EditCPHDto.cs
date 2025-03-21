﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeysReporting.WebAssembly.App.Shared.CPH
{
    public class EditCPHDto
    {

        [Required]
        [DataType(DataType.Date)]
        public DateTime? SearchDate { get; set; }

        [Required]
        public long? ProjectID { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int? CPH { get; set; }
    }
}
