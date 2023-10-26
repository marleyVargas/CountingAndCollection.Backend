using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Transversal.DTOs.Account;

namespace DistributedServices.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        readonly ILogger<TokenController> _logger;

        public TokenController(IConfiguration configuration, ILogger<TokenController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        [Route("Authentication")]
        public async Task<IActionResult> Authenticate(RequestSecurityDto requestLoginDto)
        {
            var email = this._configuration["Credentials:Email"];
            var password = this._configuration["Credentials:Password"];

            if (requestLoginDto.User != email)
            {
                return Unauthorized();
            }

            if (requestLoginDto.Password != password)
            {
                return Unauthorized();
            }

            var responseSecurityDto = new ResponseSecurityDto
            {
                IsAuthenticated = true,
                Token = await GenerateToken(requestLoginDto),
            };

            return Ok(responseSecurityDto);

        }

        private async Task<string> GenerateToken(RequestSecurityDto requestLoginDto)
        {
            var dateNow = DateTime.Now;

            //Header

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._configuration["Authentication:SecretKey"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            //Claims 

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, requestLoginDto.User),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, dateNow.ToString()),
            };

            //Payload

            var payload = new JwtPayload(
                this._configuration["Authentication:Issuer"],
                this._configuration["Authentication:Audience"],
                claims,
                dateNow,
                dateNow.AddMinutes(_configuration.GetValue<int>("Authentication:Lifetime"))
            );

            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
