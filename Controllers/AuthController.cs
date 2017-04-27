using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ESPL.KP.Entities;
using ESPL.KP.Filters;
using ESPL.KP.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;

namespace ESPL.KP.Controllers
{
    public class AuthController : Controller
    {
        private SignInManager<ESPLUser> _signInMgr;
        private UserManager<ESPLUser> _userMgr;
        private IPasswordHasher<ESPLUser> _hasher;
        private IConfigurationRoot _config;
        private RoleManager<IdentityRole> _roleMgr;

        public AuthController(
          SignInManager<ESPLUser> signInMgr,
          UserManager<ESPLUser> userMgr,
          IPasswordHasher<ESPLUser> hasher,
          ILogger<AuthController> logger,
          IConfigurationRoot config,
          RoleManager<IdentityRole> roleMgr)
        {
            _signInMgr = signInMgr;
            _userMgr = userMgr;
            _hasher = hasher;
            _config = config;
            _roleMgr = roleMgr;
        }

        [ValidateModel]
        [HttpPost("api/auth/token")]
        public async Task<IActionResult> CreateToken([FromBody] CredentialModel model)
        {
            var user = await _userMgr.FindByNameAsync(model.UserName);
            if (user != null)
            {
                if (_hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) == PasswordVerificationResult.Success)
                {
                    var userClaims = await _userMgr.GetClaimsAsync(user);
                    var userRoles = await _userMgr.GetRolesAsync(user);
                    var itemList = userRoles.ToList();
                    var toSendClaims = new List<Claim>();
                    for (int i = 0; i < itemList.Count; i++)
                    {
                        var roleFromDb = await _roleMgr.FindByNameAsync(itemList.ElementAt(i));
                        var roleClaims = await _roleMgr.GetClaimsAsync(roleFromDb);
                        toSendClaims.AddRange(roleClaims);
                    }

                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                        new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email)
                    }.Union(toSendClaims);

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                      issuer: _config["Tokens:Issuer"],
                      audience: _config["Tokens:Audience"],
                      claims: claims,
                      expires: DateTime.UtcNow.AddMinutes(15),
                      signingCredentials: creds
                      );

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
            }

            return BadRequest("Failed to generate token");
        }

        [Authorize(Policy = "Auth.CanCreate")]
        [HttpPost("api/auth/CanCreate")]
        public IActionResult CanCreate()
        {
            return Ok("You Can Create");
        }

        [Authorize(Policy = "Auth.CanRead")]
        [HttpPost("api/auth/CanRead")]

        public IActionResult CanRead()
        {
            return Ok("You Can Read");
        }

        [Authorize(Policy = "Auth.CanUpdate")]
        [HttpPost("api/auth/CanUpdate")]
        public IActionResult CanUpdate()
        {
            return Ok("You Can Update");
        }


        [Authorize(Policy = "Auth.CanDelete")]
        [HttpPost("api/auth/CanDelete")]
        public IActionResult CanDelete()
        {
            return Ok("You Can Delete");
        }

    }
}