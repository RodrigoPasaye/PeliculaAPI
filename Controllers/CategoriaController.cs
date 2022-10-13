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
    public class CategoriaController : ControllerBase {
        private readonly AplicationDbContext _context;

        public CategoriaController(AplicationDbContext context) {
            _context = context;
        }

        /// <summary>
        /// Obtiene una lista con todas las Categorias registradas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Categoria>))]
        [ProducesResponseType(400)] // Bad Request
        public async Task<IActionResult> GetCategorias() {
            var categorias = await _context.Categorias.OrderBy(categoria => categoria.Nombre).ToListAsync();
            return Ok(categorias);
        }

        /// <summary>
        /// Obtiene una Categoria filtrada por su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "GetCategoria")]
        [ProducesResponseType(200, Type = typeof(Categoria))]
        [ProducesResponseType(400)] // Bad Request
        [ProducesResponseType(404)] // Not Found
        public async Task<IActionResult> GetCategoria(int id) {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(categoria => categoria.Id == id);
            if (categoria == null) {
                return NotFound();
            }
            return Ok(categoria);
        }

        /// <summary>
        /// Crea una nueva Categoria
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)] // Bad Request
        [ProducesResponseType(500)] // Error Interno
        public async Task<IActionResult> CrearCategoria([FromBody] Categoria categoria) {
            if (categoria == null) {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            await _context.AddAsync(categoria);
            await _context.SaveChangesAsync();
            return CreatedAtRoute("GetCategoria", new { id = categoria.Id }, categoria);
        }
    }
}
