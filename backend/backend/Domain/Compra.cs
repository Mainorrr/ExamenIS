namespace backend.Domain
{
    public class Compra
    {
        public List<Refresco> refrescos { get; set; }
        public List<Moneda> monedas { get; set; }
        public Compra() { }
        public Compra(List<Refresco> refrescos, List<Moneda> monedas)
        {
            this.refrescos = refrescos;
            this.monedas = monedas;
        }
    }
}
