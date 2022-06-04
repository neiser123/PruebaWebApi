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
    public class BooksController : ControllerBase
    {
        private IMapper mapper;
        private readonly BooksService booksService = new BooksService(new BooksRepository(new PruebasSatelitesContext()));
        public BooksController()
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
            var Books = await booksService.GetAll();
            var booksDTO = Books.Select(x => mapper.Map<BooksDTO>(x));
            return Ok(booksDTO);

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
            var Books = await booksService.GetById(id);
            if (Books == null)
            {
                return NotFound();
            }
            var BooksDTO = mapper.Map<BooksDTO>(Books);
            return Ok(BooksDTO);

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
        public async Task<IActionResult> Post(BooksDTO booksDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var Books = mapper.Map<Books>(booksDTO);//volvemos la informacion del dto a una actividad
                Books = await booksService.Insert(Books);//guardamos por medio del servicio y el repositorio
                return Ok(Books);

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
        public async Task<IActionResult> Put(BooksDTO BooksDTO, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (BooksDTO.Id != id)
                    return BadRequest(ModelState);

                var Booksflag = await booksService.GetById(id);
                if (Booksflag == null)
                    return NotFound();

                var Books = mapper.Map<Books>(BooksDTO);//volvemos la informacion del dto a una actividad

                await booksService.Delete(id);//borramos
                Books = await booksService.Update(Books);//guardamos por medio del servicio y el repositorio
                return Ok(Books);

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


                var Booksflag = await booksService.GetById(id);
                if (Booksflag == null)
                    return NotFound();

                //if(!await BooksService.DeleteCheckOnEntry(id)) validamos llaves foraneas
                //await BooksService.Delete(id);//borramos

                await booksService.Delete(id);//borramos
                return Ok();

            }
            catch (Exception ex)
            {
                throw new Exception("Mensaje.", ex);

            }


        }


    }
}
