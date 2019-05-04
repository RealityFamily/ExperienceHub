using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VRTeleportator.Exceptions
{
    public class ServiceException : Exception
    {
        public override string Message { get; }
        public StatusCode StatusCode { get; set; }

        public ServiceException(StatusCode statusCode, string message) : base(message)
        {
            Message = message;
            StatusCode = statusCode;
        }
    }
}
