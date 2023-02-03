using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeysReporting.WebAssembly.App.Shared.Auth
{
    public  class UserLoginDto
    {
        public string? clientName { get; set; } = "KEYS";
        [Required]
        public string? userName { get; set; }

        [Required]
        public string? password { get; set; }

        public bool agent { get; set; }
    }
}
