const btnCrearUsuario = document.getElementById("btnCrearUsuario");

btnCrearUsuario.addEventListener("click", function () {
	$.ajax({
		url: "/Usuarios/PartialCreate",
		type: "GET",
		success: function (response) {
			console.log(response);
			$("#contenidoUsuarios").html(response);
		},
		error: function (response) {
			console.log(response);
		}
	});
});