using Probel.Arbitrium.Business;
using Probel.Arbitrium.Models;
using Xunit;

namespace Probel.Arbitrium.Tests
{
    public class Business_Password
    {
        #region Methods

        [Fact]
        public void Returns_same_hash_for_same_password()
        {
            var password = "azertyuiop";
            var pwd = new PasswordHelper();

            var hash1 = pwd.GetHash(password);
            var hash2 = pwd.GetHash(password);
            var hash3 = pwd.GetHash(password);
            var hash4 = pwd.GetHash(password);

            Assert.Equal(hash1, hash2);
            Assert.Equal(hash1, hash3);
            Assert.Equal(hash1, hash4);
        }

        [Fact]
        public void Returns_true_when_password_is_valid()
        {
            var password = "azertyuiop";
            var pwd = new PasswordHelper();

            var hash = pwd.GetHash(password);
            var user = new User() { PasswordHash = hash };

            var isValid = pwd.IsPasswordValid(user, password);
            Assert.True(isValid);
        }

        [Fact]
        public void Returns_false_when_password_is_invalid()
        {
            var password = "azertyuiop";
            var pwd = new PasswordHelper();

            var hash = pwd.GetHash(password);
            var user = new User() { PasswordHash = hash };

            var isValid = pwd.IsPasswordValid(user, "wrong_password");
            Assert.False(isValid);
        }
        #endregion Methods
    }
}