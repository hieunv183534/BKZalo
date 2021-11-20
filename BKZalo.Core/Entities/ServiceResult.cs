using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKZalo.Core.Entities
{
    public class ServiceResult
    {
        public ResponseModel Response { get; set; }

        public int StatusCode { get; set; }
    }
}
