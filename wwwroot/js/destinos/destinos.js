const contenidoDestinos = document.getElementById("contenidoDestinos");
const btnNuevoDestino = document.getElementById("btnNuevoDestino");
const btnEditarDestino = document.getElementById("btnEditarDestino");



btnNuevoDestino.addEventListener("click", function (event) {

	event.preventDefault();
	$.ajax({
		url: "/Destinos/Create",
		type: "GET",
		success: function (response) {
			$("#contenidoDestinos").html(response);

		},
		error: function (response) {

		}
	});
});


function editarDestino(id) {
	$.ajax({
		url: "/Destinos/Edit",
		type: "GET",
		data: { id: id },
		success: function (response) {
			$("#contenidoDestinos").html(response);

		},
		error: function (response) {
			console.log(response);
		}
	});
};

$(document).ready(function () {

});