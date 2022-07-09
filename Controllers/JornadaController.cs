using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Jornadas")]
    public class JornadaController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<JornadaController> Logger; 
        public JornadaController(KalumDbContext dbContext, ILogger<JornadaController> logger)
        {
            this.DbContext = dbContext;
            this.Logger = logger;
        }   
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JornadaController>>> Get()
        {
            List<Jornada> jornadas = null;
            Logger.LogDebug("Iniciando proceso de consulta de Jornadas en la base de datos");

            //Tarea 1
            jornadas = await DbContext.Jornada.Include(c => c.Aspirantes).Include(c => c.Inscripciones).ToListAsync();
            
            //Tarea 2
            if(jornadas == null || jornadas.Count == 0)
            {
                Logger.LogWarning("No existen Jornadas");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecutó la petición de forma exitosa");
            return Ok(jornadas);
        }
        [HttpGet("{id}", Name = "GetJornada")]
        public async Task<ActionResult<Jornada>> GetJornada(string id)
        {
            Logger.LogDebug("Iniciando proceso de bùsqueda con el id " + id);
            var jornada = await DbContext.Jornada.FirstOrDefaultAsync(ct => ct.JornadaId == id);
            if (jornada == null)
            {
                Logger.LogWarning("No existe Jornada con el id " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el prcoeso de búsqueda de forma exitosa");
            return Ok(jornada);
        }
        [HttpPost]
        public async Task<ActionResult<Jornada>> Post([FromBody] Jornada value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar nueva Jornada");
            value.JornadaId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.Jornada.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando proceso de crear nueva Jornada");
            return new CreatedAtRouteResult("GetJornada",new {id = value.JornadaId}, value);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Jornada>> Delete(string id)
        {
            Logger.LogDebug("Iniciando proceso de Eliminación");
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(ct => ct.JornadaId == id);
            if(jornada == null)
            {
                Logger.LogWarning($"No se encontró ninguna Jornada con el Id {id}");
                return NotFound();
            }
            else
            {
                DbContext.Jornada.Remove(jornada);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se elimino la Jornada con id {id}");
                return jornada;
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] Jornada value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualización de la  Jornada con el id{id}");
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(ct => ct.JornadaId == id);
            if(jornada == null)
            {
                Logger.LogWarning($"No existe la Jornada con el Id {id}");
                return BadRequest();
            }
            jornada.Nombre = value.Nombre;
            jornada.Descripcion = value.Descripcion;
            DbContext.Entry(jornada).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Los datos han sido actualizados correctamente.");
            return NoContent();
        }
    }
}