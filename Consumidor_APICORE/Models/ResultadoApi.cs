namespace Consumidor_APICORE.Models
{
    public class ResultadoApi
    {
        public string mensaje { get; set; }

        public List<Producto> response { get; set; }

        public Producto objeto { get; set; }
    }
}
