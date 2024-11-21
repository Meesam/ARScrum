using ARScrum.Model.AppModel;
using ARScrum.Model.AppModel.Authentication.SignUp;
using ARScrum.Model.AppModel.Response;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARScrum.Services.Services.Authentication
{
    public class UserManagement : IUserManagement
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;



        public UserManagement(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ApiResponse<List<string>>> AssignRoleToUserAsync(IEnumerable<string> roles, AppUser user)
        {
            var assignedRole = new List<string>();
            foreach (var role in roles)
            {
                if (await _roleManager.RoleExistsAsync(role))
                {
                    if (!await _userManager.IsInRoleAsync(user, role))
                    {
                        await _userManager.AddToRoleAsync(user, role);
                        assignedRole.Add(role);
                    }
                }
            }

            return new ApiResponse<List<string>> { IsSuccess = true, StatusCode = 200, Message = "Roles has been assign", Response = assignedRole };
        }

        public async Task<ApiResponse<string>> CreateUserWithTokenAsync(RegisterUser registerUser)
        {
            if (registerUser != null)
            {
                if (registerUser.Email != null)
                {
                    var userExist = await _userManager.FindByEmailAsync(registerUser.Email);
                    if (userExist != null)
                    {
                        return new ApiResponse<string> { IsSuccess = false, StatusCode = 403, Message = "User already exist" };
                    }
                }

                AppUser user = new()
                {
                    Email = registerUser.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = registerUser.UserName,
                    FirstName = registerUser.FirstName,
                    LastName = registerUser.LastName
                };

                if (await _roleManager.RoleExistsAsync(registerUser.Role))
                {

                    var result = await _userManager.CreateAsync(user, registerUser.Password);
                    if (!result.Succeeded)
                    {
                        return new ApiResponse<string> { IsSuccess = false, StatusCode = 500, Message = "User failed to create" };
                    }

                    await _userManager.AddToRoleAsync(user, registerUser.Role);
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    return new ApiResponse<string> { IsSuccess = true, StatusCode = 200, Message = "User created successfully", Response = token };
                }
                else
                {
                    var result = await _roleManager.CreateAsync(new IdentityRole(registerUser.Role));
                    if (result.Succeeded)
                    {
                        var result1 = await _userManager.CreateAsync(user, registerUser.Password);
                        if (!result1.Succeeded)
                        {
                            return new ApiResponse<string> { IsSuccess = false, StatusCode = 500, Message = "User failed to create" };
                        }

                        await _userManager.AddToRoleAsync(user, registerUser.Role);
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                        return new ApiResponse<string> { IsSuccess = true, StatusCode = 200, Message = "User created successfully", Response = token };
                    }
                    else
                    {
                        return new ApiResponse<string> { IsSuccess = false, StatusCode = 500, Message = "User failed to create" };
                    }

                }
            }
            else
            {
                return new ApiResponse<string> { IsSuccess = false, StatusCode = 403, Message = "User cannot be null" };
            }
        }
    }
}
