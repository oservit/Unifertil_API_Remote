using AutoMapper;
using Domain.Features.Authentication;
using Libs;
using Libs.Common;
using Libs.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.Features.Authentication;
using Service.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Features.Authentication
{
    public class AuthenticationAppService : IAuthenticationAppService
    {
        protected readonly IAuthenticationService _service;
        protected readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IServiceFactory _serviceFactory;
        public AuthenticationAppService(IAuthenticationService service, IServiceFactory serviceFactory,  IMapper mapper, IConfiguration configuration)
        {
            _service = service;
            _mapper = mapper;
            _configuration = configuration;
            _serviceFactory = serviceFactory;
        }


        private async Task<bool> ValidateUser(User user)
        {
            try
            {
                return await _service.ValidateUserCredentials(user);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retorna o Token após validar o usuário.
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public async Task<DataResult> GetToken(UserViewModel userModel)
        {
            try
            {
                if (userModel == null)
                    throw new Exception("Nenhum valor repassado ao controlador");

                var user = _mapper.Map<User>(userModel);

                if(!await ValidateUser(user))
                {
                    return new DataResult
                    {
                        Data = null,
                        Success = false,
                        Message = "Usuário ou Senha Inválidos!"
                    };
                }

                var token = CreateToken(user);

                return new DataResult()
                {
                    Success = true,
                    Data = token
                };
            }
            catch (Exception ex)
            {
                return ExceptionHelper.CreateErrorResult(ex);
            }
        }

        private string CreateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Crypto.Decrypt(_configuration["Jwt:Key"]));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
