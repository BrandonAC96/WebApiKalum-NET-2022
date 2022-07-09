using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Dtos;
using AutoMapper;
using WebApiKalum.Utilities;

namespace WebApiKalum.Controllers
{
    [Route("v1/KalumManagement/Aspirantes")]
    [ApiController]
    public class AspiranteController: ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<AspiranteController> Logger;

        public IMapper Mapper { get; }

        public AspiranteController(KalumDbContext _DbContext, ILogger<AspiranteController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }
        [HttpPost]
        public async Task<ActionResult<Aspirante>> Post([FromBody] Aspirante value)
        {
            Logger.LogDebug("Iniciando proceso para almacenar un registro de alumno");
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == value.CarreraId);
            if(carreraTecnica == null)
            {
                Logger.LogInformation($"No existe carrera técnica con el id {value.CarreraId}");
                return BadRequest();
            }
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == value.JornadaId);
            if(jornada == null)
            {
                Logger.LogInformation($"No existe jornada con el id {value.JornadaId}");
                return BadRequest();
            }
            ExamenAdmision examenAdmision = await DbContext.ExamenAdmision.FirstOrDefaultAsync(e => e.ExamenId == value.ExamenId);
            if(examenAdmision == null)
            {
                Logger.LogInformation($"No existe el examen de admision con el id{value.ExamenId}");
                return BadRequest();
            }
            await DbContext.Aspirante.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"Se ha creado el aspirante con éxito");
            return Ok(value);
        }
        [HttpGet]
        [ServiceFilter(typeof(ActionFilter))]
        public async Task<ActionResult<IEnumerable<AspiranteListDTO>>> Get()
        {
            Logger.LogDebug("Iniciando proceso de Aspirante");
            List<Aspirante> lista = await DbContext.Aspirante.Include(a => a.Jornada).Include(a => a.CarreraTecnica).Include(a => a.ExamenAdmision).ToListAsync();
        
            if(lista == null || lista.Count == 0)
            {
                Logger.LogWarning("No existen registros en la base");
                return new NoContentResult();
            }
            List<AspiranteListDTO> aspirantes = Mapper.Map<List<AspiranteListDTO>>(lista);
            Logger.LogInformation("La consulta se ejecuto con exito");
            return Ok(aspirantes);
            }

        [HttpGet("{id}", Name = "GetAspirante")]
        public async Task<ActionResult<Aspirante>> GetAspirante(string id)
        {
            Logger.LogDebug("Iniciando proceso de bùsqueda con el numero de Exp. " + id);
            var aspirante = await DbContext.Aspirante.FirstOrDefaultAsync(ct => ct.NoExpediente == id);
            if (aspirante == null)
            {
                Logger.LogWarning("No existe Aspirante con el Exp. " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el prcoeso de búsqueda de forma exitosa");
            return Ok(aspirante);
    }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] Aspirante value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualización del Aspirante con el Exp.{id}");
            Aspirante aspirante = await DbContext.Aspirante.FirstOrDefaultAsync(ct => ct.NoExpediente == id);
            if(aspirante == null)
            {
                Logger.LogWarning($"No existe Aspirante con el Exp {id}");
                return BadRequest();
            }
            aspirante.Apellidos = value.Apellidos;
            aspirante.Nombres = value.Nombres;
            aspirante.Direccion = value.Direccion;
            aspirante.Telefono = value.Telefono;
            aspirante.Email = value.Email;
            DbContext.Entry(aspirante).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Los datos han sido actualizados correctamente.");
            return NoContent();
        }

    [HttpDelete("{id}")]
     public async Task<ActionResult<Aspirante>> Delete(string id)
    {
        Logger.LogDebug("Iniciando proceso de Eliminación");
        Aspirante aspirante = await DbContext.Aspirante.FirstOrDefaultAsync(ct => ct.NoExpediente == id);
        if(aspirante == null)
        {
            Logger.LogWarning($"No se encontró ningun Aspirante con el Exp {id}");
            return NotFound();
        }
        else
        {
            DbContext.Aspirante.Remove(aspirante);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"Se elimino el Aspirante con el Exp {id}");
            return aspirante;
        }

}
}
}