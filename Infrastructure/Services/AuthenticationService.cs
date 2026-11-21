using Application.Dtos.AccountDto;
using Application.Dtos.Response;
using Application.Helper;
using Application.Service.Contracts;
using Domain.Entities;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services
{
    public class AuthenticationService : IUserAccount
    {
        private readonly AppDbContext _context;
        private readonly IOptions<JwtSection> _config;

        public AuthenticationService(AppDbContext context, IOptions<JwtSection> config)
        {
            _context = context;
            _config = config;
        }

        public async Task<GeneralResponse> CreateAsync(Register register)
        {
            if (register == null)
                return new GeneralResponse(false, "Model is empty");

            var checkUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == register.Email);
            if (checkUser != null)
                return new GeneralResponse(false, "User already registered");

            var user = new ApplicationUser
            {
                FullName = register.FullName ?? string.Empty,
                Email = register.Email,
                UserName = register.Email
            };

            var passwordHasher = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, register.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Handle Roles
            var checkAdminRole = await _context.SystemRoles.FirstOrDefaultAsync(r => r.Name == Constants.Admin);
            if (checkAdminRole == null)
            {
                var adminRole = new SystemRole { Name = Constants.Admin };
                _context.SystemRoles.Add(adminRole);
                await _context.SaveChangesAsync();

                _context.UserRoles_App.Add(new UserRole { RoleId = adminRole.Id, UserId = user.Id });
                await _context.SaveChangesAsync();
                return new GeneralResponse(true, "Account created successfully as Admin");
            }

            var checkUserRole = await _context.SystemRoles.FirstOrDefaultAsync(r => r.Name == Constants.User);
            if (checkUserRole == null)
            {
                var userRole = new SystemRole { Name = Constants.User };
                _context.SystemRoles.Add(userRole);
                await _context.SaveChangesAsync();

                _context.UserRoles_App.Add(new UserRole { RoleId = userRole.Id, UserId = user.Id });
            }
            else
            {
                _context.UserRoles_App.Add(new UserRole { RoleId = checkUserRole.Id, UserId = user.Id });
            }

            await _context.SaveChangesAsync();
            return new GeneralResponse(true, "Account created successfully");
        }

        public async Task<LoginResponse> LoginAsync(Login login)
        {
            if (login == null)
                return new LoginResponse(false, "Model is empty");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.Email);
            if (user == null)
                return new LoginResponse(false, "Invalid email or password");

            var passwordHasher = new PasswordHasher<ApplicationUser>();
            var verifyResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash ?? string.Empty, login.Password);
            if (verifyResult == PasswordVerificationResult.Failed)
                return new LoginResponse(false, "Invalid email or password");

            // Get user role
            var userRole = await _context.UserRoles_App.FirstOrDefaultAsync(ur => ur.UserId == user.Id);
            var roleName = Constants.User;
            if (userRole != null)
            {
                var systemRole = await _context.SystemRoles.FirstOrDefaultAsync(r => r.Id == userRole.RoleId);
                if (systemRole != null)
                    roleName = systemRole.Name;
            }

            string token = GenerateToken(user, roleName);
            string refreshToken = GenerateRefreshToken();

            // Save or update RefreshToken
            var existingToken = await _context.RefreshTokenInfos.FirstOrDefaultAsync(rt => rt.UserId == user.Id);
            if (existingToken != null)
            {
                existingToken.Token = refreshToken;
            }
            else
            {
                _context.RefreshTokenInfos.Add(new RefreshTokenInfo
                {
                    UserId = user.Id,
                    Token = refreshToken
                });
            }

            await _context.SaveChangesAsync();
            return new LoginResponse(true, "Login successful", token, refreshToken);
        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshToken token)
        {
            if (token == null || string.IsNullOrWhiteSpace(token.Token))
                return new LoginResponse(false, "Token is required");

            var existingRefreshToken = await _context.RefreshTokenInfos.FirstOrDefaultAsync(rt => rt.Token == token.Token);
            if (existingRefreshToken == null)
                return new LoginResponse(false, "Invalid refresh token");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == existingRefreshToken.UserId);
            if (user == null)
                return new LoginResponse(false, "User not found");

            // Get role
            var userRole = await _context.UserRoles_App.FirstOrDefaultAsync(ur => ur.UserId == user.Id);
            var roleName = Constants.User;
            if (userRole != null)
            {
                var systemRole = await _context.SystemRoles.FirstOrDefaultAsync(r => r.Id == userRole.RoleId);
                if (systemRole != null)
                    roleName = systemRole.Name;
            }

            string newToken = GenerateToken(user, roleName);
            string newRefreshToken = GenerateRefreshToken();

            existingRefreshToken.Token = newRefreshToken;
            await _context.SaveChangesAsync();

            return new LoginResponse(true, "Token refreshed successfully", newToken, newRefreshToken);
        }

        private string GenerateToken(ApplicationUser user, string role)
        {
            var keyString = _config.Value.Key ?? "ThisIsMySecretKeyForJwtTokenGenerationkdhvorweh";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.FullName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: _config.Value.Issuer,
                audience: _config.Value.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_config.Value.ExpiryInMinutes > 0 ? _config.Value.ExpiryInMinutes : 60),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}
