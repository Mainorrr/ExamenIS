using backend.Domain;
using System.Linq;

namespace backend.Application
{
    public class InventarioService
    {
        public Inventario inventario;
        public InventarioService(Inventario inventario)
        {
            this.inventario = inventario;
        }
        public List<Refresco> GetRefrescos()
        {
            return inventario.refrescos;
        }
        public void ImprimirEstadoInventario(string desc)
        {  
            Console.WriteLine($"==== {desc} ====");
            Console.WriteLine("Estado del inventario de refrescos:");
            foreach (var refresco in inventario.refrescos)
            {
                Console.WriteLine($"- {refresco.nombre}: precio={refresco.precio}, cantidad={refresco.cantidad}");
            }
            Console.WriteLine("Estado del inventario de monedas:");
            foreach (var moneda in inventario.monedas)
            {
                Console.WriteLine($"- {moneda.nombre}: valor={moneda.valor}, cantidad={moneda.cantidad}");
            }
        }
        public Recibo Facturar(Compra compra)
        {
            ImprimirEstadoInventario("Antes de efectuar compra");
            if (!HayMonedas())
            {
                return new Recibo(new List<Moneda>(), CodigoError.FueraDeServicio);
            }

            int totalCompra = CalcularTotalCompra(compra);
            int totalPagado = CalcularTotalPagado(compra);
            int vuelto = totalPagado - totalCompra;

            List<Moneda> monedasVuelto = new List<Moneda>();

            foreach (var monedaInventario in inventario.monedas.OrderByDescending(m => m.valor))
            {
                if (monedaInventario.valor > 500)
                {
                    continue; // Omitir 1000 colones
                }
                if (vuelto == 0)
                {
                    break;
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
                ReestablecerMonedas(monedasVuelto);
                return new Recibo(new List<Moneda>(), CodigoError.SinCambio);
            }

            // Actualizar inventario de maquina expendedora
            AgregarPago(compra.monedas);
            RestarRefrescos(compra.refrescos);

            ImprimirEstadoInventario("Luego de efectuar compra");
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
        private void RestarRefrescos(List<Refresco> refrescosComprados)
        {
            foreach (var compraRefresco in refrescosComprados)
            {
                var refrescoInventario = inventario.refrescos.FirstOrDefault(r => r.nombre == compraRefresco.nombre);
                if (refrescoInventario != null)
                {
                    refrescoInventario.cantidad -= compraRefresco.cantidad;
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
        public bool HayMonedas()
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
        public bool MonedasInsuficientes(int vuelto)
        {
            return vuelto > 0;
        }
    }
}
