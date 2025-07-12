namespace backend.Domain
{
    public class Inventario
    {
        List<Refresco> refrescos;
        List<Moneda> monedas;
        public Inventario()
        {
            refrescos = new List<Refresco>
            {
                new(precio: 800, nombre: "Coca-Cola", cantidad: 10),
                new(precio: 950, nombre: "Fanta",     cantidad: 10),
                new(precio: 750, nombre: "Pepsi",     cantidad: 8),
                new(precio: 975, nombre: "Sprite",    cantidad: 15)
            };
        }
    }
}
