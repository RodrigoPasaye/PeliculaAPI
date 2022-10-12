using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculaAPI.Data;
using PeliculaAPI.Models;
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

        [HttpGet]
        public async Task<IActionResult> GetCategorias() {
            var categorias = await _context.Categorias.OrderBy(categoria => categoria.Nombre).ToListAsync();
            return Ok(categorias);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategoria(int id) {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(categoria => categoria.Id == id);
            if (categoria == null) {
                return NotFound();
            }
            return Ok(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> CrearCategoria([FromBody] Categoria categoria) {
            if (categoria == null) {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            await _context.AddAsync(categoria);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
