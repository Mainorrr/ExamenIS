using NUnit.Framework;
using backend.Domain;
using backend.Application;
using System.Collections.Generic;

namespace UnitTesting
{
    public class InventarioServiceTests
    {
        private InventarioService _inventarioService;
        private Inventario _inventario;

        [SetUp]
        public void Setup()
        {
            _inventario = new Inventario
            {
                refrescos = new List<Refresco>(),
                monedas = new List<Moneda>()
            };

            _inventarioService = new InventarioService(_inventario);
        }

        [Test]
        public void MonedasInsuficientes_VueltoMayorQueCero_RetornaTrue()
        {
            int vuelto = 100;
            var resultado = _inventarioService.MonedasInsuficientes(vuelto);
            Assert.IsTrue(resultado);
        }

        [Test]
        public void MonedasInsuficientes_VueltoEsCero_RetornaFalse()
        {
            int vuelto = 0;
            var resultado = _inventarioService.MonedasInsuficientes(vuelto);
            Assert.IsFalse(resultado);
        }

        [Test]
        public void MonedasInsuficientes_VueltoNegativo_RetornaFalse()
        {
            int vuelto = -50;
            var resultado = _inventarioService.MonedasInsuficientes(vuelto);
            Assert.IsFalse(resultado);
        }

        [Test]
        public void HayMonedas_ConMonedasDisponibles_RetornaTrue()
        {
            _inventario.monedas = new List<Moneda>
            {
                new Moneda { valor = 100, nombre = "Cien", cantidad = 3 },
                new Moneda { valor = 500, nombre = "Quinientos", cantidad = 0 }
            };

            var resultado = _inventarioService.HayMonedas();

            Assert.IsTrue(resultado);
        }

        [Test]
        public void HayMonedas_SinMonedasDisponibles_RetornaFalse()
        {
            _inventario.monedas = new List<Moneda>
            {
                new Moneda { valor = 25, nombre = "Veinticinco", cantidad = 0 },
                new Moneda { valor = 50, nombre = "Cincuenta", cantidad = 0 }
            };

            var resultado = _inventarioService.HayMonedas();

            Assert.IsFalse(resultado);
        }

        [Test]
        public void HayMonedas_ListaVaciaDeMonedas_RetornaFalse()
        {
            _inventario.monedas = new List<Moneda>();

            var resultado = _inventarioService.HayMonedas();

            Assert.IsFalse(resultado);
        }

        [Test]
        public void GetRefrescos_ConRefrescosDevuelveListaCorrecta()
        {
            var refrescosEsperados = new List<Refresco>
            {
                new Refresco { nombre = "Coca-Cola", precio = 800, cantidad = 5 },
                new Refresco { nombre = "Pepsi", precio = 750, cantidad = 3 }
            };

            _inventario.refrescos = refrescosEsperados;

            var resultado = _inventarioService.GetRefrescos();

            Assert.AreEqual(2, resultado.Count);
            Assert.AreEqual("Coca-Cola", resultado[0].nombre);
            Assert.AreEqual("Pepsi", resultado[1].nombre);
        }

        [Test]
        public void GetRefrescos_SinRefrescosDevuelveListaVacia()
        {
            _inventario.refrescos = new List<Refresco>();

            var resultado = _inventarioService.GetRefrescos();

            Assert.IsNotNull(resultado);
            Assert.IsEmpty(resultado);
        }
    }
}
