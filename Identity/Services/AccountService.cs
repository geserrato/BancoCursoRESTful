using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.DTOs.Users;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Settings;
using Identity.Helpers;
using Identity.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly JWTSettings _jwtSettings;
    private readonly IDateTimeService _dateTimeService;

    public AccountService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, JWTSettings jwtSettings, IDateTimeService dateTimeService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _jwtSettings = jwtSettings;
        _dateTimeService = dateTimeService;
    }

    public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress)
    {
        ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new ApiException($"No hay una cuenta registrada con el email ${request.Email}.");
        }

        SignInResult? result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            throw new ApiException($"Las credenciales del usuario no son validas ${request.Email}.");
        }

        JwtSecurityToken jwtSecurityToken = await GenerateJwtToken(user);
        var response = new AuthenticationResponse
        {
            Id = user.Id,
            JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            Email = user.Email,
            UserName = user.UserName
        };

        IList<string>? rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        response.Roles = rolesList.ToList();
        response.IsVerified = user.EmailConfirmed;

        var refreshToken = GenerateRefreshToken(ipAddress);
        response.RefreshToken = refreshToken.Token;
        return new Response<AuthenticationResponse>(response, $"Usuario Autenticado {user.UserName}");
    }

    public async Task<Response<string>> RegisterAsync(RegisterRequest request, string origin)
    {
        ApplicationUser? userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);

        if (userWithSameUserName != null)
        {
            throw new ApiException($"El nombre de usuario {request.UserName} ya fue registrado previamente.");
        }

        var user = new ApplicationUser
        {
            Email = request.Email,
            Name = request.Name,
            Surname = request.Surname,
            UserName = request.UserName,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        ApplicationUser? userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
        if (userWithSameEmail != null)
        {
            throw new ApiException($"El email {request.Email} ya fue registrado previamente.");
        }

        IdentityResult? result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());
            return new Response<string>(user.Id, message: $"Usuario registrado exitosamente. {request.UserName}");
        }

        throw new ApiException($"{result.Errors}.");
    }

    private async Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user)
    {
        IList<Claim>? userClaims = await _userManager.GetClaimsAsync(user);
        IList<string>? roles = await _userManager.GetRolesAsync(user);
        List<Claim> roleClaims = roles.Select(role => new Claim("roles", role)).ToList();

        string ipAddress = IpHelper.GetIpAddress();

        IEnumerable<Claim> claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim("ip", ipAddress),
            }
            .Union(userClaims)
            .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: signingCredentials
        );

        return jwtSecurityToken;
    }
    
    private RefreshToken GenerateRefreshToken(string ipAddress)
    {
        return new RefreshToken
        {
            Token = RandomTokenString(),
            Expires = DateTime.Now.AddDays(7),
            Created = DateTime.Now,
            CreatedByIp = ipAddress
        };
    }
    
    private string RandomTokenString()
    {
        using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
        var randomBytes = new byte[40];
        rngCryptoServiceProvider.GetBytes(randomBytes);
        return BitConverter.ToString(randomBytes).Replace("-", "");
    }
}