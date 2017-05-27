using EventFinder.Contracts;
using EventFinder.Contracts.Services;
using EventFinder.Persistence;
using FlatFinder.Contracts.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EventFinder.WebAPI.Services
{
    public class UserService : IUserService
    {
        private readonly EventFinderContext _context;
        private readonly ICryptographyService _cryptographyService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(EventFinderContext context, ICryptographyService cryptographyService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _cryptographyService = cryptographyService;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<User> CurrentUser => GetCurrentUser();

        public async Task<User> GetUser(string email)
        {
            var user = await _context.Users.Include(x => x.Roles).SingleOrDefaultAsync(x => x.Email == email);
            if (user == null)
                throw new InvalidOperationException($"User with email address {email} not exists.");

            return new User
            {
                Id = user.Id,
                Email = user.Email,
                Roles = user.Roles.Select(x => x.Name)
            };
        }

        public async Task<User> Register(string email, string password)
        {
            if (!IsValidEmail(email))
                throw new InvalidOperationException("Invalid address email.");

            if (_context.Users.Any(x => x.Email == email))
                throw new InvalidOperationException($"User with email address {email} already exists.");

            byte[] salt = _cryptographyService.GetSalt();
            string hashedPassword = _cryptographyService.HashPassword(password, salt);

            var user = _context.Users.Create();
            user.Email = email;
            user.HashedPassword = hashedPassword;
            user.Salt = salt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new User
            {
                Id = user.Id,
                Email = user.Email,
                Roles = user.Roles.Select(x => x.Name)
            };
        }

        public async Task<bool> VerifyPassword(string email, string password)
        {
            if (!IsValidEmail(email))
                throw new InvalidOperationException("Invalid address email.");

            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == email);
            if (user == null)
                throw new InvalidOperationException($"User with email address {email} not exists.");

            string hashedPassword = _cryptographyService.HashPassword(password, user.Salt);
            return hashedPassword == user.HashedPassword;
        }

        private async Task<User> GetCurrentUser()
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User
                ?? throw new InvalidOperationException("No user found for current request.");

            string email = principal.FindFirst(ClaimTypes.Email).Value;
            return await GetUser(email);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}