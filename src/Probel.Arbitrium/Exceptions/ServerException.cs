using System;

namespace Probel.Arbitrium.Exceptions
{
    [Serializable]
    public class ServerException : ApplicationException
    {
        #region Constructors

        public ServerException() : this("An unknown error occured in the application")
        {
        }

        public ServerException(string message) : base(message)
        {
        }

        #endregion Constructors
    }
}