using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Entities
{
    public class CarreraTecnica
    {
        [Required (ErrorMessage = "{0} es requerido.")]
        public string CarreraId {get; set;}
        [Required (ErrorMessage = "{0} es requerido.")]
        [StringLength (maximumLength: 128, MinimumLength = 5, ErrorMessage ="La cantidad Minima de caracteres es {2} y maxima {1} para el campo {0}")]
        public string Nombre {get; set;}
        public virtual List<Aspirante> Aspirantes { get; set;}  
        public virtual List<Inscripcion> Inscripciones { get; set; }
        public virtual List<InversionCarreraTecnica> InversionesCarrerasTecnicas { get; set; }
    }
}