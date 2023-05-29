const precioString = document.getElementById("PrecioString");
const nombre = document.getElementById("Nombre");
const destinoId = document.getElementById("DestinoId");
const tipoViajeId = document.getElementById("TipoViajeId");

precioString.addEventListener("keydown", () => {
	precioString.value = precioString.value.replace(".", ",");
});
precioString.addEventListener("keyup", () => {
	
	precioString.value = precioString.value.replace(".", ",");
});
