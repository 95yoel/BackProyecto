const btnCrearUsuario = document.getElementById("btnCrearUsuario");



btnCrearUsuario.addEventListener("click", function () {

    //LLAMAR PARA OBTENER EL FORMULARIO DE CREAR USUARIO
	$.ajax({
		url: "/Usuarios/PartialCreate",
		type: "GET",
		success: function (response) {
			$("#contenidoUsuarios").html(response);
		},
		error: function (response) {
			console.log(response);
		}
	});
});



function editarUsuario(id) {
    var url = "/Usuarios/Edit/" + id;

    //LLAMAR PARA OBTENER EL FORMULARIO DE EDITAR USUARIO
    $.ajax({
        url: "/Usuarios/Edit",
        type: "GET",
        data: { id: id },
        success: function (response) {
            $("#contenidoUsuarios").load(url);
        },
        error: function (response) {
            console.log("error");
        }
    });
};