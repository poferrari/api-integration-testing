using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProjectWeather.Api.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace ProjectWeather.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly SigningConfigurations _signingConfigurations;
        private readonly TokenConfigurations _tokenConfigurations;

        public LoginController(SigningConfigurations signingConfigurations,
            TokenConfigurations tokenConfigurations)
        {
            _signingConfigurations = signingConfigurations;
            _tokenConfigurations = tokenConfigurations;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post(LoginViewModel login)
        {
            bool credenciaisValidas = false;
            UserDto userBase = null;

            if (login != null && !string.IsNullOrWhiteSpace(login.Login) && !string.IsNullOrWhiteSpace(login.Password))
            {
                userBase = UserDto.GetUsers().FirstOrDefault(t => t.Login.Equals(login.Login) && t.Password.Equals(login.Password));
                credenciaisValidas = userBase != null;
            }

            if (credenciaisValidas)
            {
                ClaimsIdentity identity = new(
                    new GenericIdentity(login.Login, "Login"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, userBase.Id.ToString())
                    }
                );

                DateTime dataCriacao = DateTime.Now;
                DateTime dataExpiracao = dataCriacao + TimeSpan.FromSeconds(_tokenConfigurations.Seconds);

                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = _tokenConfigurations.Issuer,
                    Audience = _tokenConfigurations.Audience,
                    SigningCredentials = _signingConfigurations.SigningCredentials,
                    Subject = identity,
                    NotBefore = dataCriacao,
                    Expires = dataExpiracao
                });
                var token = handler.WriteToken(securityToken);

                return Ok(new TokenDto
                {
                    Authenticated = true,
                    Created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                    Expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                    AccessToken = token,
                    Message = "OK"
                });
            }
            else
            {
                return BadRequest(new
                {
                    authenticated = false,
                    message = "Falha ao autenticar"
                });
            }
        }
    }
}
