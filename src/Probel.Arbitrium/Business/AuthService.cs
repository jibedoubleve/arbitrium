using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Probel.Arbitrium.Exceptions;
using Probel.Arbitrium.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Probel.Arbitrium.ViewModels.Admin;
using System.Reflection;

namespace Probel.Arbitrium.Business
{
    public class AuthService : IAuthService
    {
        #region Fields

        private readonly PollContext _pollContext;
        private readonly UserManager<User> _userManager;
        private IHttpContextAccessor _contextAccessor;
        private long? _userId = null;

        #endregion Fields

        #region Constructors

        public AuthService(UserManager<User> userManager, IHttpContextAccessor contextAccessor, PollContext pollContext)
        {
            _pollContext = pollContext;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }

        #endregion Constructors

        #region Methods

        public async Task<User> GetConnectedUserAsync()
        {
            var user = _contextAccessor.HttpContext.User;
            var u = await _userManager.GetUserAsync(user);
            return u ?? throw new ServerException($"User with '{user.Identity?.Name}' not found!");
        }

        public async Task<long> GetConnectedUserIdAsync()
        {
            if (_userId == null)
            {
                var user = _contextAccessor.HttpContext.User;
                var u = await _userManager.GetUserAsync(user);
                _userId = u?.Id ?? throw new ServerException($"User with '{user.Identity?.Name}' not found!");
            }
            return _userId.Value;
        }

        public async Task<IEnumerable<RoleViewModel>> GetRolesAsStringAsync(long userId)
        {
            var user = _pollContext.Users.Find(userId);
            var fieldInfos = typeof(RoleList).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            var fields = fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly);
            var roles = new List<RoleViewModel>();

            foreach (var field in fields)
            {
                var roleName = field.GetValue(null).ToString();
                var isSelected = await _userManager.IsInRoleAsync(user, roleName);
                roles.Add(new RoleViewModel { Name = roleName, IsSelected = isSelected });
            }

            return roles;
        }

        public async Task<bool> HasConnectedUser()
        {
            var user = _contextAccessor.HttpContext.User;
            var u = await _userManager.GetUserAsync(user);
            return u == null;
        }

        #endregion Methods
    }
}