using Core.Entities.Concrete;
using Core.Extensions;
using Core.Utilities.Security.Encyption;
using Core.Utilities.Security.Jwt.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.Utilities.Security.Jwt
{
    public class JwtHelper : ITokenHelper
    {
        public IConfiguration Configuration { get; }
        private TokenOptions _tokenOptions;
        private DateTime _accessTokenExpiration;
        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            _tokenOptions = new TokenOptions
            {
                Audience = Configuration.GetSection("TokenOptions:Audience").Value.ToString(),
                Issuer = Configuration.GetSection("TokenOptions:Issuer").Value.ToString(),
                SecurityKey = Configuration.GetSection("TokenOptions:SecurityKey").Value.ToString(),
                AccessTokenExpiration = Convert.ToInt32(Configuration.GetSection("TokenOptions:AccessTokenExpiration").Value)
            };


        }
        public AccessToken CreateToken(Kullanici user, List<OperationClaim> rol)
        {
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, rol);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);
            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };
        }
        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, Entities.Concrete.Kullanici user,
            SigningCredentials signingCredentials, List<OperationClaim> roller)
        {
            var jwt = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: SetClaims(user, roller),
                signingCredentials: signingCredentials
                );
            return jwt;
        }
        private IEnumerable<Claim> SetClaims(Kullanici user, List<OperationClaim> rol)
        {
            var claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id.ToString());
            claims.Add(new Claim("username", user.KullaniciAdi));
            claims.AddName($"{user.Ad} {user.Soyad}");
            claims.AddRoles(rol);
            return claims;
        }


    }
}
