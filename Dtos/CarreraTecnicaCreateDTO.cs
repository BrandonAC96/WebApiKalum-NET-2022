using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Dtos
{
    public class CarreraTecnicaCreateDTO
    {
       [StringLength (maximumLength: 128, MinimumLength = 5, ErrorMessage ="La cantidad Minima de caracteres es {2} y maxima {1} para el campo {0}")]
        public string NombreCompleto {get; set;} 
    }
}