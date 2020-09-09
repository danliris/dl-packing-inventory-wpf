using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLInventoryPacking.WinApps.Services.ResponseModel
{
    public class BaseResponseBarcodeInfo<T>
    {
        public T data { get; set; }
    }
}
