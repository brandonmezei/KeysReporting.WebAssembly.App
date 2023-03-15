using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeysReporting.WebAssembly.App.Shared.VirtualResponse
{
    public  class VirtualResponseDto<T>
    {
        public List<T> Items { get; set; }
        public int TotalSize { get; set; }
    }
}
