const contenidoReservas = document.getElementById("contenidoReservas");
const btnNuevoReserva = document.getElementById("btnNuevoReserva");
const btnEditarReserva = document.getElementById("btnEditarReserva");



btnNuevoReserva.addEventListener("click", function (event) {

	event.preventDefault();
	$.ajax({
		url: "/Reservas/Create",
		type: "GET",
		success: function (response) {
			$("#contenidoReservas").html(response);

		},
		error: function (response) {

		}
	});
});


function editarReserva(id) {
	$.ajax({
		url: "/Reservas/Edit",
		type: "GET",
		data: { id: id },
		success: function (response) {
			$("#contenidoReservas").html(response);

		},
		error: function (response) {
			console.log(response);
		}
	});
};

$(document).ready(function () {

});