namespace backend.Domain
{
    public class Moneda
    {
        public int valor { get; set; } 
        public string nombre { get; set; }
        public int cantidad { get; set; }
        public Moneda() { }
        public Moneda(int valor, string nombre, int cantidad)
        {
            this.valor = valor;
            this.nombre = nombre;
            this.cantidad = cantidad;
        }
    }
}
