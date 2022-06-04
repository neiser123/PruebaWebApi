using logica.Models;
using logica.Repositories.Implements;
using logica.Services.Implements;
using Microsoft.AspNetCore.Mvc;
using logica.DTOs;
using AutoMapper;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;


namespace PruebaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private IMapper mapper;
        private readonly UsersService usersService = new UsersService(new UsersRepository(new PruebasSatelitesContext()));

        public UsersController()
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
            var Users = await usersService.GetAll();
            var UsersDTO = Users.Select(x => mapper.Map<UsersDTO>(x));
            return Ok(UsersDTO);

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
            var Users = await usersService.GetById(id);
            if (Users == null)
            {
                return NotFound();
            }
            var UsersDTO = mapper.Map<UsersDTO>(Users);
            return Ok(UsersDTO);

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
        public async Task<IActionResult> Post(UsersDTO UsersDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var Users = mapper.Map<Users>(UsersDTO);//volvemos la informacion del dto a una actividad
                Users = await usersService.Insert(Users);//guardamos por medio del servicio y el repositorio
                return Ok(Users);

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
        public async Task<IActionResult> Put(UsersDTO UsersDTO, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (UsersDTO.Id != id)
                    return BadRequest(ModelState);

                var Usersflag = await usersService.GetById(id);
                if (Usersflag == null)
                    return NotFound();

                var Users = mapper.Map<Users>(UsersDTO);//volvemos la informacion del dto a una actividad

                await usersService.Delete(id);//borramos
                Users = await usersService.Update(Users);//guardamos por medio del servicio y el repositorio
                return Ok(Users);

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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);


                var Usersflag = await usersService.GetById(id);
                if (Usersflag == null)
                    return NotFound();

                //if(!await UsersService.DeleteCheckOnEntry(id)) validamos llaves foraneas
                //await UsersService.Delete(id);//borramos

                await usersService.Delete(id);//borramos
                return Ok();

            }
            catch (Exception ex)
            {
                throw new Exception("Mensaje.", ex);

            }


        }

    }
}
