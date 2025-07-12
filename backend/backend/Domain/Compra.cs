namespace backend.Domain
{
    public class Compra
    {
        List<Refresco> refrescos;
        List<Moneda> monedas;
        public Compra(List<Refresco> refrescos, List<Moneda> monedas)
        {
            this.refrescos = refrescos;
            this.monedas = monedas;
        }
    }
}
