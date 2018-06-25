using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Probel.Arbitrium.Exceptions
{
    [Serializable]
    public class IdentityException : ApplicationException
    {
        #region Constructors

        public IdentityException(IEnumerable<IdentityError> errors) : base(code)
        {
            Errors = errors;
        }

        #endregion Constructors

        #region Properties

        public IEnumerable<IdentityError> Errors { get; private set; }

        #endregion Properties
    }
}