using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApiKalum.Controllers

{
    [ApiController]
    [Route("v1/KalumManagement/ExamenesAdmisiones")]
    public class ExamenAdmisionController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<ExamenAdmisionController> Logger; 
        public ExamenAdmisionController(KalumDbContext dbContext, ILogger<ExamenAdmisionController> logger)
        {
            this.DbContext = dbContext;
            this.Logger = logger;
        }   
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamenAdmisionController>>> Get()
        {
            List<ExamenAdmision> examenadmisiones = null;
            Logger.LogDebug("Iniciando proceso de consulta de Exámenes de Admision en la base de datos");

            //Tarea 1
            examenadmisiones = await DbContext.ExamenAdmision.Include(c => c.Aspirantes).ToListAsync();
            
            //Tarea 2
            if(examenadmisiones == null || examenadmisiones.Count == 0)
            {
                Logger.LogWarning("No existen Exámenes de Admision");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecutó la petición de forma exitosa");
            return Ok(examenadmisiones);
        }
        [HttpGet("{id}", Name = "GetExamenAdmision")]
        public async Task<ActionResult<ExamenAdmision>> GetExamenAdmision(string id)
        {
            Logger.LogDebug("Iniciando proceso de bùsqueda con el id " + id);
            var examenadmisiones = await DbContext.ExamenAdmision.FirstOrDefaultAsync(ct => ct.ExamenId == id);
            if (examenadmisiones == null)
            {
                Logger.LogWarning("No existe Examen de Admision con el id " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el prcoeso de búsqueda de forma exitosa");
            return Ok(examenadmisiones);
        }
        [HttpPost]
        public async Task<ActionResult<ExamenAdmision>> Post([FromBody] ExamenAdmision value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar nueva fecha de Examen de Admision");
            value.ExamenId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.ExamenAdmision.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando proceso de crear nueva fecha de Examen de Admision");
            return new CreatedAtRouteResult("GetExamenAdmision",new {id = value.ExamenId}, value);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ExamenAdmision>> Delete(string id)
        {
            Logger.LogDebug("Iniciando proceso de Eliminación");
            ExamenAdmision examenAdmision = await DbContext.ExamenAdmision.FirstOrDefaultAsync(ct => ct.ExamenId == id);
            if(examenAdmision == null)
            {
                Logger.LogWarning($"No se encontró ninguna fecha de Examen de admision con el Id {id}");
                return NotFound();
            }
            else
            {
                DbContext.ExamenAdmision.Remove(examenAdmision);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se elimino el Examen de Admision con id {id}");
                return examenAdmision;
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] ExamenAdmision value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualización del Examen de Admision con el id{id}");
            ExamenAdmision examenAdmision = await DbContext.ExamenAdmision.FirstOrDefaultAsync(ct => ct.ExamenId == id);
            if(examenAdmision == null)
            {
                Logger.LogWarning($"No existe Examen de Admision el Id {id}");
                return BadRequest();
            }
            examenAdmision.FechaExamen = value.FechaExamen;
            DbContext.Entry(examenAdmision).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Los datos han sido actualizados correctamente.");
            return NoContent();
        }
    }
}