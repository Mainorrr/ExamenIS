using NUnit.Framework;
using backend.Application;
using backend.Domain;
using System.Collections.Generic;
using System.Linq;

namespace UnitTesting
{
    public class FacturarTests
    {
        private Inventario _inventario;
        private InventarioService _service;

        [SetUp]
        public void Setup()
        {
            _inventario = new Inventario();
            _inventario.Init();
            _service = new InventarioService(_inventario);
        }

        [Test]
        public void Facturar_CompraValidaConVuelto_RetornaReciboConVueltoYErrorNinguno()
        {
            var compra = new Compra
            {
                refrescos = new List<Refresco> { new Refresco { nombre = "Coca-Cola", precio = 800, cantidad = 1 } },
                monedas = new List<Moneda> { new Moneda { nombre = "Mil", valor = 1000, cantidad = 1 } }
            };

            var recibo = _service.Facturar(compra);

            Assert.AreEqual((int)CodigoError.Ninguno, recibo.error);
            Assert.AreEqual(200, recibo.monedas.Sum(m => m.valor * m.cantidad));
        }

        [Test]
        public void Facturar_CompraExacta_SinVuelto_RetornaReciboSinMonedasYErrorNinguno()
        {
            var compra = new Compra
            {
                refrescos = new List<Refresco> { new Refresco { nombre = "Pepsi", precio = 750, cantidad = 1 } },
                monedas = new List<Moneda>
                {
                    new Moneda { nombre = "Cincuenta", valor = 50, cantidad = 1 },
                    new Moneda { nombre = "Cien", valor = 100, cantidad = 2 },
                    new Moneda { nombre = "Quinientos", valor = 500, cantidad = 1 }
                }
            };

            var recibo = _service.Facturar(compra);

            Assert.AreEqual((int)CodigoError.Ninguno, recibo.error);
            Assert.IsEmpty(recibo.monedas);
        }

        [Test]
        public void Facturar_SinMonedasEnInventario_RetornaErrorFueraDeServicio()
        {
            foreach (var moneda in _inventario.monedas)
                moneda.cantidad = 0;

            var compra = new Compra
            {
                refrescos = new List<Refresco> { new Refresco { nombre = "Coca-Cola", precio = 800, cantidad = 1 } },
                monedas = new List<Moneda> { new Moneda { nombre = "Mil", valor = 1000, cantidad = 1 } }
            };

            var recibo = _service.Facturar(compra);

            Assert.AreEqual((int)CodigoError.FueraDeServicio, recibo.error);
        }

        [Test]
        public void Facturar_CambioInsuficiente_RetornaErrorSinCambio()
        {
            foreach (var moneda in _inventario.monedas)
                moneda.cantidad = (moneda.valor == 25) ? 2 : 0;

            var compra = new Compra
            {
                refrescos = new List<Refresco> { new Refresco { nombre = "Pepsi", precio = 750, cantidad = 1 } },
                monedas = new List<Moneda> { new Moneda { nombre = "Mil", valor = 1000, cantidad = 1 } }
            };

            var recibo = _service.Facturar(compra);

            Assert.AreEqual((int)CodigoError.SinCambio, recibo.error);
        }

        [Test]
        public void Facturar_DescuentaRefrescoDelInventario()
        {
            var refrescoAntes = _inventario.refrescos.First(r => r.nombre == "Pepsi").cantidad;

            var compra = new Compra
            {
                refrescos = new List<Refresco> { new Refresco { nombre = "Pepsi", precio = 750, cantidad = 1 } },
                monedas = new List<Moneda> { new Moneda { nombre = "Mil", valor = 1000, cantidad = 1 } }
            };

            _service.Facturar(compra);

            var refrescoDespues = _inventario.refrescos.First(r => r.nombre == "Pepsi").cantidad;
            Assert.AreEqual(refrescoAntes - 1, refrescoDespues);
        }

        [Test]
        public void Facturar_AgregaMonedasPagadasAlInventario()
        {
            var quinientosAntes = _inventario.monedas.First(m => m.valor == 500).cantidad;

            var compra = new Compra
            {
                refrescos = new List<Refresco> { new Refresco { nombre = "Sprite", precio = 975, cantidad = 1 } },
                monedas = new List<Moneda>
                {
                    new Moneda { nombre = "Quinientos", valor = 500, cantidad = 2 }
                }
            };

            _service.Facturar(compra);

            var quinientosDespues = _inventario.monedas.First(m => m.valor == 500).cantidad;
            Assert.AreEqual(quinientosAntes + 2, quinientosDespues);
        }
    }
}
