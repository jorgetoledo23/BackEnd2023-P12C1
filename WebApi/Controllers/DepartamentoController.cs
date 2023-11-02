using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Model;

namespace WebApi.Controllers
{
    [Route("api/v3/[controller]")]

    //https://localhost/api/v3/departamento/

    [ApiController]
    [Authorize(Roles = "Trabajador")] // 1 Rol Autorizado
    [Authorize(Roles = "Trabajador, Ayudante")]   // 2 Roles Autorizados
    public class DepartamentoController : ControllerBase
    {

        //Acceso a la BD
        private readonly AppDbContext _context;

        //Constructor => __init__(self)
        public DepartamentoController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet] // Obtener => Leer Base de Datos
        [Route("getAll")] // Obtener todos los Dptos
        public async Task<IActionResult> getAll()
        {
                                // Select * From TblDepartamentos
            var dptos = await _context.TblDepartamentos.ToListAsync();
            return Ok(dptos);
        }

        [HttpPost] // Escribir en la Base de Datos
        [Route("addDpto")] // Agregar Dpto

        public async Task<IActionResult> addDpto(Departamento dpto)
        {
            // INSERT INTO TblDepartamentos (Nombre, Descripcion) VALUES ('TI', 'Tecnologias de la Informacion')

            var existe = await _context.TblDepartamentos
                .Where(x => x.Nombre == dpto.Nombre)
                .FirstOrDefaultAsync();

            if(existe == null)
            {
                _context.Add(dpto);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest("Departamento Existe!");

            
        }

        [HttpPut]
        [Route("uptDpto")]
        public async Task<IActionResult> uptDpto(int id, Departamento dpto)
        {
            var existe =await _context.TblDepartamentos.Where(x => x.Cod_Dpto == id)
                .FirstOrDefaultAsync();

            if(existe == null)
            {
                return NotFound();
            }

            existe.Nombre = dpto.Nombre;
            existe.Descripcion = dpto.Descripcion;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        [Route("delDpto")]
        public async Task<IActionResult> delDpto(int id)
        {
            var existe = await _context.TblDepartamentos.Where(x => x.Cod_Dpto == id)
                .FirstOrDefaultAsync();

            if (existe == null)
            {
                return NotFound();
            }

            _context.Remove(existe);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpGet]
        [Route("getByFirstKey")]
        public async Task<IActionResult> getByFirstKey(string StartKey, string EndKey)
        {
            var dptosEncontrados = await _context.TblDepartamentos
                .Where(x => x.Nombre.StartsWith(StartKey) && x.Nombre.EndsWith(EndKey))
                .ToListAsync();
            return Ok(dptosEncontrados);
        }

        //Peticion GET que filtre los departamentos segun un filtro dado
        [HttpGet]
        [Route("getByFilter")]
        public async Task<IActionResult> getByFilter(string Filter)
        {
            var dptosEncontrados = await _context.TblDepartamentos
                .Where(x => x.Nombre.Contains(Filter) || x.Descripcion.Contains(Filter))
                .ToListAsync();
            return Ok(dptosEncontrados);
        }

    }
}
 