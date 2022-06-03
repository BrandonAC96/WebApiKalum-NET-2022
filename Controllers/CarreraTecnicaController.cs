using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using System.Linq;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/[controller]")]
    public class CarreraTecnicaController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        public CarreraTecnicaController(KalumDbContext dbContext)
        {
            this.DbContext = dbContext;
        }   
        [HttpGet]
        public ActionResult<List<CarreraTecnica>> Get()
        {
            List<CarreraTecnica> carrerasTecnicas = null;
            carrerasTecnicas = DbContext.CarreraTecnica.Include(c => c.Aspirantes).Include(c => c.Inscripciones).ToList();
            if(carrerasTecnicas == null || carrerasTecnicas.Count == 0)
            {
                return new NoContentResult();
            }
            return Ok(carrerasTecnicas);
        }
    }
}