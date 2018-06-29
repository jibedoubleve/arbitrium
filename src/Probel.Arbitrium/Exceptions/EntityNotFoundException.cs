using System;
using System.Net;

namespace Probel.Arbitrium.Exceptions
{
    [Serializable]
    public class EntityNotFoundException : ApplicationException
    {
        #region Fields

        private const HttpStatusCode code = HttpStatusCode.BadRequest;

        #endregion Fields

        #region Constructors

        public EntityNotFoundException(Type entityType, long id) : this($"Entity of type '{entityType}' with id '{id}' not found!")
        {
        }

        public EntityNotFoundException(string message) : base(message)
        {
        }

        #endregion Constructors
    }
}