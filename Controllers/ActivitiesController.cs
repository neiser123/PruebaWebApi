using AutoMapper;
using logica.DTOs;
using logica.Models;
using logica.Repositories.Implements;
using logica.Services.Implements;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PruebaWebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
   [Authorize]
    public class ActivitiesController : ControllerBase
    {
        private IMapper mapper;
        //private readonly ActivitiesService activitiesService= new ActivitiesService(new ActivitiesRepository(PruebasSatelitesContext.Create()));
        private readonly ActivitiesService activitiesService= new ActivitiesService(new ActivitiesRepository(new PruebasSatelitesContext()));

        public ActivitiesController()
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
            var activities = await activitiesService.GetAll();
            var activitiesDTO = activities.Select(x => mapper.Map<ActivitiesDTO>(x));
            return Ok(activitiesDTO);

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
            var activities = await activitiesService.GetById(id);
            if (activities==null)
            {
                return NotFound();
            }
            var activitiesDTO = mapper.Map<ActivitiesDTO>(activities);
            return Ok(activitiesDTO);

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
        public async Task<IActionResult>Post (ActivitiesDTO activitiesDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var activities = mapper.Map<Activities>(activitiesDTO);//volvemos la informacion del dto a una actividad
                activities = await activitiesService.Insert(activities);//guardamos por medio del servicio y el repositorio
                return Ok(activities);

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
        public async Task<IActionResult> Put(ActivitiesDTO activitiesDTO,int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (activitiesDTO.id != id)
                    return BadRequest(ModelState);

                var activitiesflag = await activitiesService.GetById(id);
                if (activitiesflag == null)
                    return NotFound();

                var activities = mapper.Map<Activities>(activitiesDTO);//volvemos la informacion del dto a una actividad

                 await activitiesService.Delete(id);//borramos
                activities = await activitiesService.Update(activities);//guardamos por medio del servicio y el repositorio
                return Ok(activities);

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

                var activitiesflag = await activitiesService.GetById(id);
                if (activitiesflag == null)
                    return NotFound();

                //if(!await activitiesService.DeleteCheckOnEntry(id)) validamos llaves foraneas
                //await activitiesService.Delete(id);//borramos

                await activitiesService.Delete(id);//borramos
                return Ok();

            }
            catch (Exception ex)
            {
                throw new Exception("Mensaje.", ex);

            }


        }

    }
}
