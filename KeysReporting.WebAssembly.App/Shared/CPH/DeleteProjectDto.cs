using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeysReporting.WebAssembly.App.Shared.CPH
{
    public class DeleteProjectDto
    {
        private DateTime _SearchDate;

        [Required]
        [DataType(DataType.Date)]
        public DateTime SearchDate { get { return _SearchDate; } set { _SearchDate = value.Date; } }

        public long? ProjectID { get; set; }
    }
}
