using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Probel.Arbitrium.Exceptions;
using Probel.Arbitrium.Models;
using System.Threading.Tasks;

namespace Probel.Arbitrium.Business
{
    public class AuthenticationHelper
    {
        #region Fields

        private readonly UserManager<User> _userManager;
        private IHttpContextAccessor _contextAccessor;
        private long? _userId = null;

        #endregion Fields

        #region Constructors

        public AuthenticationHelper(UserManager<User> userManager, IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }

        #endregion Constructors

        #region Methods

        public async Task<long> GetUserId()
        {
            if (_userId == null)
            {
                var user = _contextAccessor.HttpContext.User;
                var u = await _userManager.GetUserAsync(user);
                _userId = u?.Id ?? throw new ServerException($"User with '{user.Identity?.Name}' not found!");
            }
            return _userId.Value;
        }

        #endregion Methods
    }
}