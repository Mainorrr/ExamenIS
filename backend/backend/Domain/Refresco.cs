namespace backend.Domain
{
    public class Refresco
    {
        public int precio { get; set; }
        public string nombre { get; set; }
        public int cantidad { get; set; }
        public Refresco(int precio, string nombre, int cantidad)
        {
            this.precio = precio;
            this.nombre = nombre;
            this.cantidad = cantidad;
        }
    }
}
