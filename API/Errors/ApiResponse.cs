using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            // ??  means if it is null then execute what after this
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessgaeForStatusCode(statusCode);
        }

        private string GetDefaultMessgaeForStatusCode(int statusCode)
        {
           return statusCode switch
           {
               400 => "A bad request, You have made",
               401 => "Authorized, You are not",
               404 => "resource found, it was not",
               500 =>  "Errors are the path to the dark side.  Errors lead to anger.   Anger leads to hate.  Hate leads to career change.",
               _=> null
           };
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}