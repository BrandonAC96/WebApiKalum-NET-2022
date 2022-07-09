using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;


namespace WebApiKalum.Entities
{
    public class Cargo
    {
        public string CargoId { get; set; }
        public string Descripcion { get; set; }
        public string Prefijo { get; set;}
        [Required]
        [Precision(10,2)]
        public Decimal Monto { get; set; }
        public Boolean GeneraMora { get; set; }
        public int PorcentajeMora { get; set; }
        public virtual List<CuentaXCobrar> CuentasXCobrar { get; set; }
    }
}