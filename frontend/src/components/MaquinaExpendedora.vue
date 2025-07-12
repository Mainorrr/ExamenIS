<template>
  <div>
    <h3>Mi máquina expendedora</h3>
    <div style="display:flex; justify-content:space-around; margin-bottom:20px;">
      <div v-for="(producto, index) in productos" :key="producto.nombre" style="text-align:center;">
        <div><b>{{ producto.nombre }}</b></div>
        <div>{{ producto.precio }} ¢</div>
        <div>{{ producto.stock }} disponibles</div>
        <div>Su compra<br></div>
        <button @click="restar(index)">-</button>
        <span>{{ producto.cantidad }}</span>
        <button @click="sumar(index)">+</button>
      </div>
    </div>

    <div style="text-align:center; margin-bottom:30px;">
      <div><b>{{ totalPagar }} ¢</b></div>
      <div>Total a pagar</div>
    </div>

    <h3>Su pago</h3>
    <div style="display:flex; justify-content:space-around; margin-bottom:20px;">
      <div>
        Billetes 1000 ¢
        <input type="number" min="0" v-model.number="pago.billete1000" @keydown="bloquearNegativo" />
      </div>
      <div>
        Monedas 500 ¢
        <input type="number" min="0" v-model.number="pago.moneda500" @keydown="bloquearNegativo" />
      </div>
      <div>
        Monedas 100 ¢
        <input type="number" min="0" v-model.number="pago.moneda100" @keydown="bloquearNegativo" />
      </div>
      <div>
        Monedas 50 ¢
        <input type="number" min="0" v-model.number="pago.moneda50" @keydown="bloquearNegativo" />
      </div>
      <div>
        Monedas 25 ¢
        <input type="number" min="0" v-model.number="pago.moneda25" @keydown="bloquearNegativo" />
      </div>
    </div>

    <div style="text-align:center; margin-bottom:30px;">
      <button @click="comprar">Comprar</button>
    </div>

    <div v-if="mensajeError.visible" style="text-align:center; color:red; margin-bottom: 30px;">
      <h4>{{ mensajeError.titulo }}</h4>
      <p>{{ mensajeError.texto }}</p>
    </div>

    <div v-if="compraRealizada" style="display:flex; justify-content:space-around;">
      <div>
        <b>Su pago</b><br />
        <div v-if="pagoConfirmado.billete1000 > 0">1000 ¢ x {{ pagoConfirmado.billete1000 }}</div>
        <div v-if="pagoConfirmado.moneda500 > 0">500 ¢ x {{ pagoConfirmado.moneda500 }}</div>
        <div v-if="pagoConfirmado.moneda100 > 0">100 ¢ x {{ pagoConfirmado.moneda100 }}</div>
        <div v-if="pagoConfirmado.moneda50 > 0">50 ¢ x {{ pagoConfirmado.moneda50 }}</div>
        <div v-if="pagoConfirmado.moneda25 > 0">25 ¢ x {{ pagoConfirmado.moneda25 }}</div>
      </div>

      <div>
        <b>Su vuelto es de {{ vuelto }} ¢</b><br />
        <b>Desglose:</b><br />
        <div v-for="(cant, val) in desgloseVuelto" :key="val">
          {{ val }} ¢ x {{ cant }}
        </div>
      </div>

      <div>
        <b>Sus productos</b><br />
        <div v-for="prod in productosConfirmados" :key="prod.nombre">
          {{ prod.nombre }} x {{ prod.cantidad }}
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { reactive, computed, ref, onMounted } from 'vue'

let productos = reactive([])

const productosSeleccionados = computed(() => {
  return productos.filter(prod => prod.cantidad > 0)
})

const productosConfirmados = ref([])

const pago = {
  billete1000: 0,
  moneda500: 0,
  moneda100: 0,
  moneda50: 0,
  moneda25: 0,
}

const pagoConfirmado = {
  billete1000: 0,
  moneda500: 0,
  moneda100: 0,
  moneda50: 0,
  moneda25: 0,
}

const mensajeError = reactive({
  visible: false,
  titulo: '',
  texto: ''
})

const compraRealizada = ref(false)
const vuelto = ref(0)
const desgloseVuelto = reactive({})

const totalPagar = computed(() => {
  return productos.reduce((acc, prod) => acc + prod.precio * prod.cantidad, 0)
})

function mostrarError(titulo, texto) {
  mensajeError.titulo = titulo
  mensajeError.texto = texto
  mensajeError.visible = true
}

function ocultarError() {
  mensajeError.visible = false
}

function sumar(index) {
  ocultarError()
  if (productos[index].cantidad < productos[index].stock) {
    productos[index].cantidad++
  } else {
    mostrarError(
      'Disponibilidad Agotada',
      `La cantidad de refrescos seleccionada para ${productos[index].nombre} ya es la máxima disponible.`
    )
  }
}

function restar(index) {
  ocultarError()
  if (productos[index].cantidad > 0) {
    productos[index].cantidad--
  } else {
    mostrarError('Selección Inválida', 'No se pueden seleccionar menos de 0 refrescos.')
  }
}

// Evitar los numeros negativos en cantidades de billetes y monedas
function bloquearNegativo(event) {
  if (event.key === '-' || event.code === 'Minus') {
    event.preventDefault()
  }
}

function totalPagado() {
  return pago.billete1000 * 1000 +
         pago.moneda500 * 500 +
         pago.moneda100 * 100 +
         pago.moneda50 * 50 +
         pago.moneda25 * 25
}

async function obtenerDesglose(pago, productos) {
  try {
    const compra = {
      refrescos: productos.map(p => ({
        nombre: p.nombre,
        precio: p.precio,
        cantidad: p.cantidad
      })),
      monedas: [
        { valor: 1000, nombre: "Mil", cantidad: pago.billete1000 },
        { valor: 500, nombre: "Quinientos", cantidad: pago.moneda500 },
        { valor: 100, nombre: "Cien", cantidad: pago.moneda100 },
        { valor: 50, nombre: "Cincuenta", cantidad: pago.moneda50 },
        { valor: 25, nombre: "Veinticinco", cantidad: pago.moneda25 }
      ]
    }

    const response = await fetch('https://localhost:7093/api/Inventario/Compra', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(compra)
    })

    if (!response.ok) throw new Error('Error al comunicarse con el backend.')

    const recibo = await response.json()
    
    console.log('Recibo:', recibo)


    // Manejo de errores del recibo
    switch (recibo.error) {
      case 1: // Fuera de servicio
        mostrarError('Fuera de servicio', 'La máquina expendedora no tiene monedas en este momento.')
        return null
      case 2: // Sin cambio
        mostrarError('Sin cambio', 'La máquina tiene monedas, pero no puede dar el vuelto exacto.')
        return null
      case 0:
      default: {
        const desglose = {}
        for (const moneda of recibo.monedas) {
          desglose[moneda.valor] = moneda.cantidad
        }
        return desglose
      }
    }
  } catch (error) {
    mostrarError('Error de conexión', 'Ocurrió un error al obtener el desglose del backend.')
    console.error(error)
    return null
  }
}

async function comprar() {
  ocultarError()
  compraRealizada.value = false

  const pagado = totalPagado()

  if (pagado < totalPagar.value) {
    mostrarError(
      'Pago insuficiente',
      'El pago dado es insuficiente para realizar su compra, agregue más dinero.'
    )
    return
  }

  const cambio = pagado - totalPagar.value
  const desglose = await obtenerDesglose(pago, productosSeleccionados.value)

  // Si el backend devuelve null, ya se mostró el error correspondiente
  if (!desglose) return

  // Guardar el pago confirmado
  Object.assign(pagoConfirmado, pago)
  vuelto.value = cambio

  // Actualizar el desglose en pantalla
  Object.keys(desgloseVuelto).forEach(key => delete desgloseVuelto[key])
  Object.entries(desglose).forEach(([key, val]) => {
    desgloseVuelto[key] = val
  })

  // Guardar productos seleccionados
  productosConfirmados.value = productosSeleccionados.value.map(prod => ({
    nombre: prod.nombre,
    cantidad: prod.cantidad
  }))

  // Reiniciar el pago
  Object.keys(pago).forEach(key => {
    pago[key] = 0
  })

  // Actualizar productos desde el backend
  await cargarProductos()

  // Marcar como realizada la compra
  compraRealizada.value = true
}


async function cargarProductos() {
  try {
    const response = await fetch('https://localhost:7093/api/Inventario/Refrescos')
    if (!response.ok) throw new Error('Error al cargar productos del backend')

    const refrescos = await response.json()

    // Limpiar almacenamiento de productos previo
    productos.splice(0, productos.length)

    // Guardar los refrescos y sus atributos
    refrescos.forEach(r => {
      productos.push({
        nombre: r.nombre,
        precio: r.precio,
        stock: r.cantidad,
        cantidad: 0
      })
    })
  } catch (error) {
    mostrarError('Error de conexión', 'No se pudieron cargar los productos del backend.')
  }
}

onMounted(() => {
  cargarProductos()
})
</script>
