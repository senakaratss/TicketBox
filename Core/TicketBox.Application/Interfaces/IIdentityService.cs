using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.DTOs;
using TicketBox.Application.Features.Users.Results;

namespace TicketBox.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<string> RegisterAsync(RegisterUserDto dto);
        Task<bool> LoginAsync(LoginUserDto dto);
        Task LogoutAsync();
        Task<List<GetUsersQueryResult>> GetAllUsersAsync();
        Task<GetUserByIdQueryResult> GetUserByIdAsync(string userId);
        Task<GetMyProfileQueryResult> GetMyProfileAsync();
        Task<string> GetCurrentUserIdAsync();
        Task<bool> UpdateProfileAsync(UpdateProfileDto dto);
        Task<bool> ChangePasswordAsync(ChangePasswordDto dto);
        Task<UserInfoDto> GetUserInfoAsync(string userId);
        Task ConfirmEmail(string email, string code);
    }
}
