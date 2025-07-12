using backend.Domain;
using System.Linq;

namespace backend.Application
{
    public class InventarioService
    {
        public Inventario inventario;
        public InventarioService()
        {
            inventario = new Inventario();
        }
        public List<Refresco> GetRefrescos()
        {
            return inventario.refrescos;
        }
        public Recibo Facturar(Compra compra)
        {
            if (!HayMonedas())
            {
                return new Recibo(new List<Moneda>(), CodigoError.FueraDeServicio);
            }

            int totalCompra = CalcularTotalCompra(compra);
            int totalPagado = CalcularTotalPagado(compra);

            int vuelto = totalPagado - totalCompra;
            List<Moneda> monedasVuelto = new List<Moneda>();

            // Intentar dar vuelto con las monedas del inventario
            foreach (var monedaInventario in inventario.monedas)
            {
                if (monedaInventario.valor > 500)
                {
                    continue; // Saltar billete de 1000
                }
                if (vuelto == 0)
                {
                    break; // Pago completo
                }

                int cantidadNecesaria = vuelto / monedaInventario.valor;
                int cantidadDisponible = monedaInventario.cantidad;

                int cantidadAEntregar = Math.Min(cantidadNecesaria, cantidadDisponible);

                if (cantidadAEntregar > 0)
                {
                    monedasVuelto.Add(new Moneda(monedaInventario.valor, monedaInventario.nombre, cantidadAEntregar));
                    vuelto -= cantidadAEntregar * monedaInventario.valor;
                    monedaInventario.cantidad -= cantidadAEntregar;
                }
            }

            if (MonedasInsuficientes(vuelto))
            {
                // Reestablecer monedas descontadas en el inventario
                ReestablecerMonedas(monedasVuelto);
                return new Recibo(new List<Moneda>(), CodigoError.SinCambio);
            }

            // Agregar el pago del usuario para vueltos
            AgregarPago(compra.monedas);

            return new Recibo(monedasVuelto, CodigoError.Ninguno);
        }
        private void ReestablecerMonedas(List<Moneda> monedas)
        {
            foreach (var moneda in monedas)
            {
                foreach (var inventarioMoneda in inventario.monedas)
                {
                    if (inventarioMoneda.valor == moneda.valor)
                    {
                        inventarioMoneda.cantidad += moneda.cantidad;
                        break;
                    }
                }
            }
        }
        private void AgregarPago(List<Moneda> monedas)
        {
            foreach (var moneda in monedas)
            {
                foreach (var inventarioMoneda in inventario.monedas)
                {
                    if (inventarioMoneda.valor == moneda.valor)
                    {
                        inventarioMoneda.cantidad += moneda.cantidad;
                        break;
                    }
                }
            }
        }
        private int CalcularTotalCompra(Compra compra)
        {
            int total = 0;
            foreach (var refresco in compra.refrescos)
            {
                total += refresco.precio * refresco.cantidad;
            }
            return total;
        }
        private int CalcularTotalPagado(Compra compra)
        {
            int total = 0;
            foreach (var moneda in compra.monedas)
            {
                total += moneda.valor * moneda.cantidad;
            }
            return total;
        }
        private bool HayMonedas()
        {
            foreach (Moneda moneda in inventario.monedas)
            {
                if (moneda.cantidad > 0)
                {
                    return true;
                }
            }
            return false;
        }
        private bool MonedasInsuficientes(int vuelto)
        {
            return vuelto > 0;
        }
    }
}
