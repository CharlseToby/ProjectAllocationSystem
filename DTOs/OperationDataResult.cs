using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAllocationSystem.DTOs
{    
    public class OperationDataResult<T> : OperationResult
    {
        public OperationDataResult(T data)
        {
            Succeeded = true;
            Data = data;
        }

        public OperationDataResult(string error)
        {
            Error = error;
            Succeeded = false;
        }

        public T Data { get; set; }
    }
}
