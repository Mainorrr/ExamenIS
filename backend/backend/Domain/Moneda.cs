namespace backend.Domain
{
    public class Moneda
    {
        int valor { get; set; } 
        string nombre { get; set; }
        public Moneda(int valor, string nombre)
        {
            this.valor = valor;
            this.nombre = nombre;
        }
    }
}
