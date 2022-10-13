using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculaAPI.Data;
using PeliculaAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculaAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PeliculaController : ControllerBase {
        private readonly AplicationDbContext _context;

        public PeliculaController(AplicationDbContext context) {
            _context = context;
        }

        /// <summary>
        /// Obtiene una lista con todas las Peliculas registradas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Pelicula>))]
        [ProducesResponseType(400)] // Bad Request
        public async Task<IActionResult> GetPeliculas() {
            var peliculas = await _context.Peliculas.OrderBy(pelicula => pelicula.NombrePelicula).Include(pelicula => pelicula.Categoria).ToListAsync();
            return Ok(peliculas);
        }

        /// <summary>
        /// Obtiene una Pelicula filtrada por su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "GetPelicula")]
        [ProducesResponseType(200, Type = typeof(Pelicula))]
        [ProducesResponseType(400)] // Bad Request
        [ProducesResponseType(404)] // Not Found
        public async Task<IActionResult> GetPelicula(int id) {
            var pelicula = await _context.Peliculas.Include(pelicula => pelicula.Categoria).FirstOrDefaultAsync(pelicula => pelicula.Id == id);
            if (pelicula == null) {
                return NotFound();
            }
            return Ok(pelicula);
        }

        /// <summary>
        /// Crea una nueva Pelicula
        /// </summary>
        /// <param name="pelicula"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)] // Bad Request
        [ProducesResponseType(500)] // Error Interno
        public async Task<IActionResult> CrearPelicula([FromBody] Pelicula pelicula) {
            if (pelicula == null) {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            await _context.AddAsync(pelicula);
            await _context.SaveChangesAsync();
            return CreatedAtRoute("GetPelicula", new { id = pelicula.Id }, pelicula);
        }
    }
}
