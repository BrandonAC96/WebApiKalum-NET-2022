using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApiKalum.Controllers
{
[ApiController]
[Route("v1/KalumManagement/Cargos")]
public class CargoController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<CargoController> Logger; 
        public CargoController(KalumDbContext dbContext, ILogger<CargoController> logger)
        {
            this.DbContext = dbContext;
            this.Logger = logger;
        }   
        [HttpGet]      
        public async Task<ActionResult<IEnumerable<CargoController>>> Get()
        {
            List<Cargo> cargos = null;
            Logger.LogDebug("Iniciando proceso de consulta de Cargos en la base de datos");

            //Tarea 1
            cargos = await DbContext.Cargo.Include(c => c.CuentasXCobrar).ToListAsync();
            
            //Tarea 2
            if(cargos == null || cargos.Count == 0)
            {
                Logger.LogWarning("No existen Cargos");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecutó la petición de forma exitosa");
            return Ok(cargos);
        }
        [HttpGet("{id}", Name = "GetCargo")]
        public async Task<ActionResult<Cargo>> GetCargo(string id)
        {
            Logger.LogDebug("Iniciando proceso de bùsqueda con el id " + id);
            var cargos = await DbContext.Cargo.Include(c => c.CuentasXCobrar).FirstOrDefaultAsync(ct => ct.CargoId == id);
            if (cargos == null)
            {
                Logger.LogWarning("No existe Cargo con el id " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el proceso de búsqueda de forma exitosa");
            return Ok(cargos);
        }
    }
}