using System;
using System.Runtime.Serialization;

namespace Probel.Arbitrium.Core.Exception
{
    public class HttpException : ApplicationException
    {
        #region Constructors

        private HttpException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        private HttpException(string message) : base(message)
        {
        }

        #endregion Constructors

        public static HttpException NotFound => new HttpException("Error 404 - Not Found");
        public static HttpException Unauthorized => new HttpException("Error 401 - Unauthorized");
        public static HttpException BadRequest => new HttpException("Error 400 - Bad Request");
        public static HttpException InternalServerError =>new HttpException("Error 500 - Internal Server Error");
    }
}