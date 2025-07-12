namespace backend.Domain
{
    public class Compra
    {
        public List<Refresco> refrescos;
        public List<Moneda> monedas;
        public Compra(List<Refresco> refrescos, List<Moneda> monedas)
        {
            this.refrescos = refrescos;
            this.monedas = monedas;
        }
    }
}
