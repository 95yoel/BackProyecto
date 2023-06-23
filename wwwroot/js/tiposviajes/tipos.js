const contenidoTipos = document.getElementById("contenidoTipos");
const btnNuevoTipo = document.getElementById("btnNuevoTipo");
const btnEditarTipo = document.getElementById("btnEditarTipo");


//LLAMADA A LA VISTA PARCIAL DE CREAR TIPO DE VIAJE
btnNuevoTipo.addEventListener("click", function (event) {

	event.preventDefault();
	$.ajax({
		url: "/TiposViajes/PartialCreate",
		type: "GET",
		success: function (response) {
			$("#contenidoTipos").html(response);

		},
		error: function (response) {

		}
	});
});

//LLAMADA A LA VISTA PARCIAL DE EDITAR TIPO DE VIAJE
function editarTipo(id) {
	
	$.ajax({
		url: "/TiposViajes/Edit",
		type: "GET",
		data: { id: id },
		success: function (response) {
			$("#contenidoTipos").html(response);

		},
		error: function (response) {
			console.log(response);
		}
	});
};

$(document).ready(function () {

});