using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Probel.Arbitrium.Exceptions
{
    [Serializable]
    public class IdentityException : ApplicationException
    {
        #region Constructors

        public IdentityException(IEnumerable<IdentityError> errors) : base()
        {
            Errors = errors;
        }

        #endregion Constructors

        #region Properties

        public IEnumerable<IdentityError> Errors { get; private set; }

        public override string Message => ExpandIdentityErrors();

        private string ExpandIdentityErrors()
        {
            var result = string.Empty;

            foreach (var error in Errors)
            {
                result += $"[{error.Code}] {error.Description}{Environment.NewLine}";
            }
            return result.TrimEnd(Environment.NewLine.ToCharArray());
        }

        #endregion Properties
    }
}