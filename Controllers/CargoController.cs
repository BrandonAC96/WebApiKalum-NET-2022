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
            var cargos = await DbContext.Cargo.FirstOrDefaultAsync(ct => ct.CargoId == id);
            if (cargos == null)
            {
                Logger.LogWarning("No existe Cargo con el id " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el proceso de búsqueda de forma exitosa");
            return Ok(cargos);
        }
        [HttpPost]
        public async Task<ActionResult<Cargo>> Post([FromBody] Cargo value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar nuevo Cargo");
            value.CargoId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.Cargo.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando proceso de crear nuevo Cargo");
            return new CreatedAtRouteResult("GetCargo",new {id = value.CargoId}, value);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Cargo>> Delete(string id)
        {
            Logger.LogDebug("Iniciando proceso de Eliminación");
            Cargo cargo = await DbContext.Cargo.FirstOrDefaultAsync(ct => ct.CargoId == id);
            if(cargo == null)
            {
                Logger.LogWarning($"No se encontró ningun Cargo con el Id {id}");
                return NotFound();
            }
            else
            {
                DbContext.Cargo.Remove(cargo);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se elimino el Cargo con id {id}");
                return cargo;
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] Cargo value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualización del Cargo con el id{id}");
            Cargo cargo = await DbContext.Cargo.FirstOrDefaultAsync(ct => ct.CargoId == id);
            if(cargo == null)
            {
                Logger.LogWarning($"No existe Cargo con el Id {id}");
                return BadRequest();
            }
            cargo.Descripcion = value.Descripcion;
            cargo.Prefijo = value.Prefijo;
            cargo.Monto = value.Monto;
            cargo.GeneraMora = value.GeneraMora;
            cargo.PorcentajeMora = value.PorcentajeMora;
            DbContext.Entry(cargo).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Los datos han sido actualizados correctamente.");
            return NoContent();
        }
    }
}