using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Alumnos")]
    public class AlumnoController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<AlumnoController> Logger; 
        public AlumnoController(KalumDbContext dbContext, ILogger<AlumnoController> logger)
        {
            this.DbContext = dbContext;
            this.Logger = logger;
        }   
        [HttpGet]        
        public async Task<ActionResult<IEnumerable<AlumnoController>>> Get()
        {
            List<Alumno> alumnos = null;
            Logger.LogDebug("Iniciando proceso de consulta de Alumnos en la base de datos");

            //Tarea 1
            alumnos = await DbContext.Alumno.Include(c => c.CuentasXCobrar).Include(c => c.Inscripciones).ToListAsync();
            
            //Tarea 2
            if(alumnos == null || alumnos.Count == 0)
            {
                Logger.LogWarning("No existen Alumnos");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecutó la petición de forma exitosa");
            return Ok(alumnos);
        }
        [HttpGet("{id}", Name = "GetAlumno")]
        public async Task<ActionResult<Alumno>> GetAlumno(string id)
        {
            Logger.LogDebug("Iniciando proceso de bùsqueda con el id " + id);
            var alumnos = await DbContext.Alumno.FirstOrDefaultAsync(ct => ct.Carne == id);
            if (alumnos == null)
            {
                Logger.LogWarning("No existe Alumno con el id " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el proceso de búsqueda de forma exitosa");
            return Ok(alumnos);
        }
        [HttpPost]
        public async Task<ActionResult<Alumno>> Post([FromBody] Alumno value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar nuevo Alumno");
            await DbContext.Alumno.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando proceso de crear nuevo Alumno");
            return new CreatedAtRouteResult("GetAlumno",new {id = value.Carne}, value);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Alumno>> Delete(string id)
        {
            Logger.LogDebug("Iniciando proceso de Eliminación");
            Alumno alumno = await DbContext.Alumno.FirstOrDefaultAsync(ct => ct.Carne == id);
            if(alumno == null)
            {
                Logger.LogWarning($"No se encontró ningún Alumno con el Id {id}");
                return NotFound();
            }
            else
            {
                DbContext.Alumno.Remove(alumno);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se elimino el alumno con id {id}");
                return alumno;
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] Alumno value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualización de un alumno con el id{id}");
            Alumno alumno = await DbContext.Alumno.FirstOrDefaultAsync(ct => ct.Carne == id);
            if(alumno == null)
            {
                Logger.LogWarning($"No existe el Alumno con el Id {id}");
                return BadRequest();
            }
            alumno.Nombres = value.Nombres;
            DbContext.Entry(alumno).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Los datos han sido actualizados correctamente.");
            return NoContent();
        }
    }
}