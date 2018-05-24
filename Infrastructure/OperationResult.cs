using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.BLL.Infrastructure
{
    public class OperationResult
    {
        public string Result { get; set; }

        public OperationResult(string result)
        {
            Result = result;
        }
    }
}
