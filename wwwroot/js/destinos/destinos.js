const contenidoDestinos = document.getElementById("contenidoDestinos");
const btnNuevoDestino = document.getElementById("btnNuevoDestino");
const btnEditarDestino = document.getElementById("btnEditarDestino");



btnNuevoDestino.addEventListener("click", function (event) {

	event.preventDefault();

	//CARGAR VISTA PARCIAL CREATE
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

	//CARGAR VISTA PARCIAL EDIT
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