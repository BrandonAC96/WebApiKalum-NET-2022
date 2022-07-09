using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Entities
{
    public class ExamenAdmision
    {
        public string ExamenId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaExamen { get; set; }
        public virtual List<Aspirante> Aspirantes { get; set; }
    }
}