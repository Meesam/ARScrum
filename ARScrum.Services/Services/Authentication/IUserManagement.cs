using ARScrum.Model.AppModel.Authentication.SignUp;
using ARScrum.Model.AppModel.Response;
using ARScrum.Model.AppModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARScrum.Services.Services.Authentication
{
    public interface IUserManagement
    {
        Task<ApiResponse<string>> CreateUserWithTokenAsync(RegisterUser registerUser);
        Task<ApiResponse<List<string>>> AssignRoleToUserAsync(IEnumerable<string> roles, AppUser user);
    }
}
