using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.DTOs;
using TicketBox.Application.Features.Users.Results;
using TicketBox.Application.Interfaces;
using TicketBox.Persistence.Context;

namespace TicketBox.Persistence.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TicketContext _ticketContext;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IdentityService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IHttpContextAccessor httpContextAccessor, TicketContext ticketContext, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _ticketContext = ticketContext;
            _emailService = emailService;
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordDto dto)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            return result.Succeeded;
        }

        public async Task ConfirmEmail(string email, string code)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user.ConfirmCode != code)
            {
                throw new Exception("Wrong code");
            }
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);
        }

        public async Task<List<GetUsersQueryResult>> GetAllUsersAsync()
        {
            return await _userManager.Users.Select(x => new GetUsersQueryResult
            {
                Id = x.Id,
                Name = x.Name,
                Surname = x.Surname,
                Email = x.Email,
                Username = x.UserName,
                EmailConfirmed = x.EmailConfirmed,
                TicketCount=_ticketContext.Tickets.Count(t=>t.Booking.UserId==x.Id),
                BookingCount=_ticketContext.Bookings.Count(b=>b.UserId==x.Id)
            }).ToListAsync();
        }

        public async Task<string> GetCurrentUserIdAsync()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            return user?.Id;
        }

        public async Task<GetMyProfileQueryResult> GetMyProfileAsync()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return new GetMyProfileQueryResult
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Username = user.UserName,
                Phone = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
            };
        }

        public async Task<GetUserByIdQueryResult> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return new GetUserByIdQueryResult
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Username = user.UserName,
                EmailConfirmed = user.EmailConfirmed
            };
        }

        public async Task<UserInfoDto> GetUserInfoAsync(string userId)
        {
            var value = await _userManager.FindByIdAsync(userId);
            return new UserInfoDto
            {
                Name = value.Name,
                Surname = value.Surname,
                Email = value.Email
            };
        }

        public async Task<bool> LoginAsync(LoginUserDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);

            if (user == null)
            {
                return false;
            }

            var result = await _signInManager.PasswordSignInAsync(user, dto.Password, false, false);
            return result.Succeeded;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<string> RegisterAsync(RegisterUserDto dto)
        {
            Random rnd = new Random();
            var confirmCode = rnd.Next(1000, 10000).ToString();

            var user = new AppUser
            {
                Name = dto.Name,
                Surname = dto.Surname,
                Email = dto.Email,
                PhoneNumber = dto.Phone,
                UserName = dto.Username,
                ConfirmCode=confirmCode
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(x => x.Description));
                throw new Exception(errors);
            }

            await _emailService.SendEmailAsync(user.Email, "Email Confirmation",$"Your confirmation code is: {confirmCode}");
            return user.Id;
        }

        public async Task<bool> UpdateProfileAsync(UpdateProfileDto dto)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null) return false;

            user.Name = dto.Name;
            user.Surname = dto.Surname;
            user.Email = dto.Email;
            user.PhoneNumber = dto.Phone;
            user.UserName = dto.Username;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}
