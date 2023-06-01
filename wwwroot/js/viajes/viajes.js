const contenidoViajes = document.getElementById("contenidoViajes");
const btnNuevoViaje = document.getElementById("btnNuevoViaje");
const btnEditarViaje = document.getElementById("btnEditarViaje");



btnNuevoViaje.addEventListener("click", function (event) {

	event.preventDefault();
	$.ajax({
		url: "/Viajes/Create",
		type: "GET",
		success: function (response) {
			$("#contenidoViajes").html(response);

		},
		error: function (response) {
			
		}
	});
});


function editarViaje(id){
	$.ajax({
		url: "/Viajes/Edit",
		type: "GET",
		data: { id: id },
		success: function (response) {
			$("#contenidoViajes").html(response);

		},
		error: function (response) {
			console.log(response);
		}
	});
};

$(document).ready(function () {

});