namespace WebApiKalum.Entities
{
    public class ResultadoExamenAdmision
    {
        public string NoExpediente { get; set; }
        public string Anio { get; set; }
        public string Descripcion { get; set; }
        public string Nota { get; set; }

            public virtual Aspirante Aspirante { get; set; }
    }
}