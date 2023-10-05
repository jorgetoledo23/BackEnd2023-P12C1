using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Model;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CargaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CargaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> add(CargaDTO Cdto)
        {
            var carga = new Carga()
            {
                Rut = Cdto.Rut,
                Nombres = Cdto.Nombres,
                Apellidos = Cdto.Apellidos,
                FechaNacimiento = Cdto.FechaNacimiento,
                RutTrabajador = Cdto.RutTrabajador,
            };
            _context.Add(carga);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<IActionResult> getAll()
        {
            return Ok(await _context.TblCargas.ToListAsync());
        }
    }
}
