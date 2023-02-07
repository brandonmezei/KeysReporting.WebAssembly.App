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
        private string? _username;

        public string clientName { get; set; } = "KEYS";
        
        [Required]
        public string userName { get { return _username; } set { _username = value?.Trim().ToUpper(); } }

        [Required]
        public string password { get; set; }

        public bool agent { get; set; }
    }
}
