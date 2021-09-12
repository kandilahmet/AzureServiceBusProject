using AzureServiceBusProject.Application.Interfaces.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureServiceBusProject.Application.Results
{
    public class DataResult<T>:Result,IDataResult<T>
    {
        public DataResult(T Data,bool IsSuccess,string Message):base(IsSuccess,Message)
        {
            this.Data = Data;
        }

        public DataResult(T Data, bool IsSuccess) : base(IsSuccess)
        {
            this.Data = Data;
        }

        public T Data { get; }
    }
}
