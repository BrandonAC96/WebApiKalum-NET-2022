namespace WebApiKalum.Entities
{
    public class InversionCarreraTecnica
    {
        public string InversionId { get; set; }
        public string CarreraId { get; set; }
        public string MontoInscripcion { get; set; }
        public string NumeroPagos { get; set; }
        public decimal MontoPago { get; set; }

        public virtual CarreraTecnica CarreraTecnica { get; set; }
    }
}