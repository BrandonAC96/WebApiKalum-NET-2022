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
            var examenadmisiones = await DbContext.ExamenAdmision.Include(c => c.Aspirantes).FirstOrDefaultAsync(ct => ct.ExamenId == id);
            if (examenadmisiones == null)
            {
                Logger.LogWarning("No existe Examen de Admision con el id " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el prcoeso de búsqueda de forma exitosa");
            return Ok(examenadmisiones);
        }
    }
}