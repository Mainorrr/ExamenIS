namespace backend.Domain
{
    public class Inventario
    {
        public List<Refresco> refrescos;
        public List<Moneda> monedas; // Usadas para vuelto
        public Inventario() {}
        public void Init()
        {
            refrescos = new List<Refresco>
            {
                new(precio: 800, nombre: "Coca-Cola", cantidad: 10),
                new(precio: 950, nombre: "Fanta",     cantidad: 10),
                new(precio: 750, nombre: "Pepsi",     cantidad: 8),
                new(precio: 975, nombre: "Sprite",    cantidad: 15)
            };
            monedas = new List<Moneda>
            {
                new(valor: 25, nombre: "Veinticinco", cantidad: 25),
                new(valor: 50, nombre: "Cincuenta", cantidad: 50),
                new(valor: 100, nombre: "Cien", cantidad: 30),
                new(valor: 500, nombre: "Quinientos", cantidad: 20),
                // Solo se cuentan, no se da vueltos con esto
                new(valor: 1000, nombre: "Mil", cantidad: 0)
            };
        }
    }
}
