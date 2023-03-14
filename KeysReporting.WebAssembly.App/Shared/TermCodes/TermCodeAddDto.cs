using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeysReporting.WebAssembly.App.Shared.TermCodes
{
    public class TermCodeAddDto
    {
        [Required]
        [Range(0, long.MaxValue)]
        public long? AgentID { get; set; }

        [Required]
        [Range(0, long.MaxValue)]
        public long? TermCodeID { get; set; }

        [Required]
        [Range(0, long.MaxValue)]
        public long? ProjectID { get; set; }

        [Required]
        public string? Account { get; set; }

        [Required]
        public DateTime? FileDate { get; set; }

        [Range(0, int.MaxValue)]
        public decimal? TotalPtp { get; set; }

        [Range(0, int.MaxValue)]
        public decimal? DblDip { get; set; }

    }
}
