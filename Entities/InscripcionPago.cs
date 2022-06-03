namespace WebApiKalum.Entities
{
    public class InscripcionPago
    {
        public string NoExpediente { get; set; }
        public string BoletaPago { get; set; }
        public string Anio { get; set; }
        public string FechaPago { get; set; }
        public string Monto { get; set;}

            virtual public Aspirante Aspirante { get; set; }
    }
}