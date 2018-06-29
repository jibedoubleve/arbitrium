﻿using Microsoft.AspNetCore.Identity;
using System;

namespace Probel.Arbitrium.Exceptions
{
    [Serializable]
    public class ConnectionException : ApplicationException
    {
        #region Constructors

        public ConnectionException(SignInResult signInResult) : base()
        {
            SignInResult = signInResult;
        }

        #endregion Constructors

        #region Properties

        public SignInResult SignInResult { get; private set; }

        public override string Message => $"Connection failed. Possible cause is '{SignInResult.ToString()}'";

        #endregion Properties
    }
}