namespace backend.Domain
{
    public class Refresco
    {
        private int precio { get; set; }
        private string nombre { get; set; }
        private int cantidad { get; set; }
        public Refresco(int precio, string nombre, int cantidad)
        {
            this.precio = precio;
            this.nombre = nombre;
            this.cantidad = cantidad;
        }
    }
}
