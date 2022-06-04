using AutoMapper;
using logica.Models;
using logica.Repositories.Implements;
using logica.Services.Implements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PruebaWebApi.Helper;
using PruebaWebApi.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PruebaWebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private IMapper mapper;
        private readonly IConfiguration config;
        //private readonly ActivitiesService activitiesService= new ActivitiesService(new ActivitiesRepository(new PruebasSatelitesContext()));

        public TokenController(IConfiguration configuration)
        {
            this.config = configuration;
            this.mapper = Startup.MapperConfiguration.CreateMapper();
        }
 
       

      
        /// <summary>
        /// genera token para el api(usuario Prueba,el password es 1234,sal es 1234) igual estan quedamos en el modelo para agilizar.
        /// </summary>
        /// <remarks>
        /// genera token para el api.
        /// </remarks>
        /// <param name="id">Id (GUID) del objeto.</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        [HttpPost]
        [Route("token")]
        public async Task<IActionResult>Post (Token token)//ActivitiesDTO activitiesDTO
        {
            try
            {

               // Token token = new Token();
                if (token.Usuario != "Prueba")
                {
                    return NotFound(ErrorHelper.Response(404, "Usuario no encontrado."));
                }
                if (token.Clave != "1234")
                {
                    return NotFound(ErrorHelper.Response(404, "Clave no encontrada."));
                }
                if (HashHelper.CheckHash(token.Usuario, token.Clave, token.Sal))
                {
                    var secretKey = config.GetValue<string>("SecretKey");
                    var key = Encoding.ASCII.GetBytes(secretKey);

                    var claims = new ClaimsIdentity();
                    claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, token.Usuario));

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = claims,
                        Expires = DateTime.UtcNow.AddHours(1), //horas de duracion del token
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var createdToken = tokenHandler.CreateToken(tokenDescriptor);

                    string bearer_token = tokenHandler.WriteToken(createdToken);
                    return Ok(bearer_token);
                }
                else
                {
                    return Forbid();
                }


            }
            catch (Exception ex)
            {
                throw new Exception("Mensaje.", ex);

            }


        }

        
       

    }
}
