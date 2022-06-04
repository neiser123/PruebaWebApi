using AutoMapper;
using logica.Models;
using logica.Repositories.Implements;
using logica.Services.Implements;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using logica.DTOs;
using Microsoft.AspNetCore.Authorization;


namespace PruebaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CoverphotosController : ControllerBase
    {
        private IMapper mapper;
        private readonly CoverphotosService coverphotosService = new CoverphotosService(new CoverphotosRepository(new PruebasSatelitesContext()));

        public CoverphotosController()
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
            var Coverphotos = await coverphotosService.GetAll();
            var CoverphotosDTO = Coverphotos.Select(x => mapper.Map<CoverphotosDTO>(x));
            return Ok(CoverphotosDTO);

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
            var Coverphotos = await coverphotosService.GetById(id);
            if (Coverphotos == null)
            {
                return NotFound();
            }
            var CoverphotosDTO = mapper.Map<CoverphotosDTO>(Coverphotos);
            return Ok(CoverphotosDTO);

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
        public async Task<IActionResult> Post(CoverphotosDTO CoverphotosDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var Coverphotos = mapper.Map<Coverphotos>(CoverphotosDTO);//volvemos la informacion del dto a una actividad
                Coverphotos = await coverphotosService.Insert(Coverphotos);//guardamos por medio del servicio y el repositorio
                return Ok(Coverphotos);

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
        public async Task<IActionResult> Put(CoverphotosDTO CoverphotosDTO, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (CoverphotosDTO.Id != id)
                    return BadRequest(ModelState);

                var Coverphotosflag = await coverphotosService.GetById(id);
                if (Coverphotosflag == null)
                    return NotFound();

                var Coverphotos = mapper.Map<Coverphotos>(CoverphotosDTO);//volvemos la informacion del dto a una actividad

                await coverphotosService.Delete(id);//borramos
                Coverphotos = await coverphotosService.Update(Coverphotos);//guardamos por medio del servicio y el repositorio
                return Ok(Coverphotos);

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


                var Coverphotosflag = await coverphotosService.GetById(id);
                if (Coverphotosflag == null)
                    return NotFound();

                //if(!await CoverphotosService.DeleteCheckOnEntry(id)) validamos llaves foraneas
                //await CoverphotosService.Delete(id);//borramos

                await coverphotosService.Delete(id);//borramos
                return Ok();

            }
            catch (Exception ex)
            {
                throw new Exception("Mensaje.", ex);

            }


        }

    }
}
