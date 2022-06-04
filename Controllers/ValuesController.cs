using AutoMapper;
using logica.Models;
using logica.Repositories.Implements;
using logica.Services.Implements;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PruebaWebApi.Controllers
{
    //    [ApiController]
    //    [Route("[controller]")]
    [Route("[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private IMapper mapper;
        private readonly AuthorsService authorsService = new AuthorsService(new AuthorsRepository(new PruebasSatelitesContext()));
        private readonly ActivitiesService activitiesService = new ActivitiesService(new ActivitiesRepository(new PruebasSatelitesContext()));
        private readonly BooksService booksService = new BooksService(new BooksRepository(new PruebasSatelitesContext()));
        private readonly CoverphotosService coverphotosService = new CoverphotosService(new CoverphotosRepository(new PruebasSatelitesContext()));
        private readonly UsersService usersService = new UsersService(new UsersRepository(new PruebasSatelitesContext()));


        public ValuesController()
        {
            this.mapper = Startup.MapperConfiguration.CreateMapper();
        }


        /// <summary>
        /// Obtiene todos los objetos del api de Activities .
        /// </summary>
        /// <remarks>
        /// Obtiene todos los objetos .
        /// </remarks>
        /// <param name="id">Id (GUID) del objeto.</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        [HttpGet]
        [Route("activities")]
        public async Task<IActionResult> GetActivities()
        {
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync("https://fakerestapi.azurewebsites.net/api/v1/Activities");
            List<Activities> activities = JsonConvert.DeserializeObject<List<Activities>>(json);
            // Activities activities = JsonConvert.DeserializeObject<Activities>(json);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            activitiesService.DeleteAll2(activities);

            //foreach (var item in activities)//borramos los registros
            //{   activitiesService.DeleteAll(item);}
            foreach (var item in activities)
            {
            var activities1 = await activitiesService.Insert(item);
            }
            return Ok(activities);
        }

        /// <summary>
        /// Obtiene todos los objetos del api authors .
        /// </summary>
        /// <remarks>
        /// Obtiene todos los objetos .
        /// </remarks>
        /// <param name="id">Id (GUID) del objeto.</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        [HttpGet]
        [Route("authors")]
        public async Task<IActionResult> GetAuthors()
        {
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync("https://fakerestapi.azurewebsites.net/api/v1/Authors");
            List<Authors> Authors = JsonConvert.DeserializeObject<List<Authors>>(json);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            authorsService.DeleteAll2(Authors);
            foreach (var item in Authors)
            {
                var activities1 = await authorsService.Insert(item);
            }
            return Ok(Authors);
        }
        /// <summary>
        /// Obtiene todos los objetos del api books .
        /// </summary>
        /// <remarks>
        /// Obtiene todos los objetos .
        /// </remarks>
        /// <param name="id">Id (GUID) del objeto.</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        [HttpGet]
        [Route("books")]
        public async Task<IActionResult> GetBooks()
        {
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync("https://fakerestapi.azurewebsites.net/api/v1/Books");
            List<Books> Books = JsonConvert.DeserializeObject<List<Books>>(json);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            booksService.DeleteAll2(Books);
            foreach (var item in Books)
            {
                var activities1 = await booksService.Insert(item);
            }
            return Ok(Books);
        }
        /// <summary>
        /// Obtiene todos los objetos del api coverphotos .
        /// </summary>
        /// <remarks>
        /// Obtiene todos los objetos .
        /// </remarks>
        /// <param name="id">Id (GUID) del objeto.</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        [HttpGet]
        [Route("coverphotos")]
        public async Task<IActionResult> Getcoverphotos()
        {
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync("https://fakerestapi.azurewebsites.net/api/v1/CoverPhotos");
            List<Coverphotos> Coverphotos = JsonConvert.DeserializeObject<List<Coverphotos>>(json);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            coverphotosService.DeleteAll2(Coverphotos);
            foreach (var item in Coverphotos)
            {
                var coverphotosService1 = await coverphotosService.Insert(item);
            }
            return Ok(Coverphotos);
        }

        /// <summary>
        /// Obtiene todos los objetos del api users .
        /// </summary>
        /// <remarks>
        /// Obtiene todos los objetos .
        /// </remarks>
        /// <param name="id">Id (GUID) del objeto.</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        [HttpGet]
        [Route("users")]
        public async Task<IActionResult> Getusers()
        {
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync("https://fakerestapi.azurewebsites.net/api/v1/Users");
            List<Users> Users = JsonConvert.DeserializeObject<List<Users>>(json);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            usersService.DeleteAll2(Users);
            foreach (var item in Users)
            {
                var Users1 = await usersService.Insert(item);
            }
            return Ok(Users);
        }


    }
}
