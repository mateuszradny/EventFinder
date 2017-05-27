using EventFinder.Contracts;
using EventFinder.Contracts.Services;
using EventFinder.WebAPI.Options;
using EventFinder.WebAPI.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EventFinder.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly TokenValidationOptions _tokenValidationOptions;
        private readonly IUserService _userService;

        public AccountController(IUserService userService, IOptions<TokenValidationOptions> tokenValidationOptions)
        {
            _userService = userService;
            _tokenValidationOptions = tokenValidationOptions.Value;
        }

        [AllowAnonymous]
        [HttpPost("Token")]
        public async Task<IActionResult> CreateToken([FromBody]CreateTokenRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (await _userService.VerifyPassword(request.Email, request.Password))
                {
                    User user = await _userService.GetUser(request.Email);

                    IEnumerable<Claim> claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email)
                    }.Union(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

                    JwtSecurityToken token = CreateTokenBasedOnClaims(claims);

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return BadRequest("Failed to login");
        }

        [AllowAnonymous]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _userService.Register(request.Email, request.Password);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        private JwtSecurityToken CreateTokenBasedOnClaims(IEnumerable<Claim> claims)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenValidationOptions.IssuerSigningKey));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: _tokenValidationOptions.Issuer,
                audience: _tokenValidationOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials
            );
        }
    }
}