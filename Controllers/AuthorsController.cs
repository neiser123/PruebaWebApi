using AutoMapper;
using logica.DTOs;
using logica.Models;
using logica.Repositories.Implements;
using logica.Services.Implements;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorsController : ControllerBase
    {
        private readonly AuthorsService authorsService = new AuthorsService(new AuthorsRepository(new PruebasSatelitesContext()));
        private IMapper mapper;


        public AuthorsController()
        {
            this.mapper = Startup.MapperConfiguration.CreateMapper();
        }
        /// <summary>
        /// Obtiene todos los objetos .
        /// </summary>
        /// <remarks>
        /// Obtiene todos los objetos .
        /// </remarks>
        /// <param name="id">Id (GUID) del objeto.</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetAll()
        {
            var Authors = await authorsService.GetAll();
            var AuthorsDTO = Authors.Select(x => mapper.Map<AuthorsDTO>(x));
            return Ok(AuthorsDTO);

        }
        /// <summary>
        /// Obtiene un objeto por su Id.
        /// </summary>
        /// <remarks>
        /// Aquí una descripción mas larga si fuera necesario. Obtiene un objeto por su Id.
        /// </remarks>
        /// <param name="id">Id (GUID) del objeto.</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        [HttpGet]
        [Route("GetId/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var Authors = await authorsService.GetById(id);
                if (Authors == null)
                {
                    return NotFound();
                }
                var AuthorsDTO = mapper.Map<AuthorsDTO>(Authors);
                return Ok(AuthorsDTO);
            }
            catch (Exception ex)
            {

                throw new Exception("Mensaje.", ex); 
            }
         

        }
        /// <summary>
        /// guarda el registro a nivel de bd(enviar la estructura del json bien).
        /// </summary>
        /// <remarks>
        /// guarda el registro a nivel de bd(enviar la estructura del json bien).
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        [HttpPost]
        public async Task<IActionResult>Post (AuthorsDTO AuthorsDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var Authors = mapper.Map<Authors>(AuthorsDTO);//volvemos la informacion del dto a una actividad
                Authors = await authorsService.Insert(Authors);//guardamos por medio del servicio y el repositorio
                return Ok(Authors);

            }
            catch (Exception ex)
            {
                throw new Exception("Mensaje.", ex);

            }


        }

        /// <summary>
        /// actualiza un registro  por su Id.
        /// </summary>
        /// <remarks>
        ///  actualiza un registro  por su Id(el id que se envia en la url debe coincidir con el que se envia en la estructura del json bien)..
        /// </remarks>
        /// <param name="id">Id (GUID) del objeto.</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> Put(AuthorsDTO AuthorsDTO,int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (AuthorsDTO.Id != id)
                    return BadRequest(ModelState);

                var Authorsflag = await authorsService.GetById(id);
                if (Authorsflag == null)
                    return NotFound();

                var Authors = mapper.Map<Authors>(AuthorsDTO);//volvemos la informacion del dto a una actividad

                 await authorsService.Delete(id);//borramos
                Authors = await authorsService.Update(Authors);//guardamos por medio del servicio y el repositorio
                return Ok(Authors);

            }
            catch (Exception ex)
            {
                throw new Exception("Mensaje.", ex);

            }


        }
        /// <summary>
        /// borra un registro  por su Id.
        /// </summary>
        /// <remarks>
        ///  borra un registro  por su Id(el id que se envia en la url debe coincidir con el que se envia en la estructura del json bien).
        /// </remarks>
        /// <param name="id">Id (GUID) del objeto.</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete( int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);


                var Authorsflag = await authorsService.GetById(id);
                if (Authorsflag == null)
                    return NotFound();

                //if(!await AuthorsService.DeleteCheckOnEntry(id)) validamos llaves foraneas
                //await AuthorsService.Delete(id);//borramos

                await authorsService.Delete(id);//borramos
                return Ok();

            }
            catch (Exception ex)
            {
                throw new Exception("Mensaje.", ex);

            }


        }



    }
}
