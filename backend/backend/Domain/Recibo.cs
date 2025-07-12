namespace backend.Domain
{
    public enum CodigoError
    {
        Ninguno = 0,
        FueraDeServicio = 1,
        SinCambio = 2
    }
    public class Recibo
    {
        public List<Moneda> monedas { get; set; }
        public int error { get; set; }
        public Recibo(List<Moneda> monedas, CodigoError error)
        {
            this.monedas = monedas;
            this.error = (int)error;
        }
    }
}
