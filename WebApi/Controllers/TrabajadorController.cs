using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Model;

namespace WebApi.Controllers
{
    [Route("api/v3/[controller]")]
    [ApiController]
    public class TrabajadorController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TrabajadorController(AppDbContext context)
        {
            _context = context;
        }

        //TODO: Delete
        //TODO: Update
        //TODO: GetByName
        //TODO: GetByComuna
        //TODO: GetByRut
        //TODO: GetByCargo

        [HttpGet]
        [Route("getAll")]
        public async Task<IActionResult> getAll()
        {
            var allTrabs = await _context.TblTrabajadores.ToListAsync();
            return Ok(allTrabs);
        }

        [HttpPost]
        public async Task<IActionResult> addTrabj(TrabajadorDTO Tdto)
        {
            var T = new Trabajador()
            {
                Rut = Tdto.Rut,
                Nombres = Tdto.Nombres,
                Apellidos = Tdto.Apellidos,
                Cod_Dpto = Tdto.Cod_Dpto,
                Correo = Tdto.Correo,
                Telefono = Tdto.Telefono,
                Direccion = Tdto.Direccion,
                Comuna = Tdto.Comuna,
            };
            _context.TblTrabajadores.Add(T);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
