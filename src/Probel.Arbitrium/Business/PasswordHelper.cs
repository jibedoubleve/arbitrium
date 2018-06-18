using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Probel.Arbitrium.Models;
using System;
using System.Text;

namespace Probel.Arbitrium.Business
{
    public class PasswordHelper
    {
        #region Fields

        private const string SALT = "sLyrUlHUNl7mPeptTy9+wA==";

        #endregion Fields

        #region Methods

        public string GetHash(string password)
        {
            var salt = Encoding.ASCII.GetBytes(SALT);
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10_000,
                numBytesRequested: 256 / 8
            ));
            return hashed;
        }

        public bool IsPasswordValid(User user, string password)
        {
            var hash = GetHash(password);
            return hash == user.PasswordHash;
        }
        #endregion Methods
    }
}