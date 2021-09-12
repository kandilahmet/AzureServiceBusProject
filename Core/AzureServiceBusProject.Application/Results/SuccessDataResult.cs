using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureServiceBusProject.Application.Results
{
    public class SuccessDataResult<T>: DataResult<T>
    {
        public SuccessDataResult(T Data,string Message):base(Data,true,Message)
        { 
        }

        public SuccessDataResult(T Data) : base(Data, true )
        {
        } 
    }
}
