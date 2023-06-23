const inputUsuario = document.getElementById("UsuarioId");
const inputViaje = document.getElementById("ViajeId");
const numeroPersonas = document.getElementById("NumeroPersonas");
const PROVINCIA = document.getElementById("Provincia");

$(document).ready(function () {

});

//EVENTO QUE SE EJECUTA CUANDO EL INPUT CAMBIA DE VALOR
inputUsuario.addEventListener("change", () => {
    //LLAMAR AL CONTROLADOR PARA OBTENER LOS DATOS DEL CLIENTE
    $.ajax({
        url: `/Reservas/getDatosCliente/${inputUsuario.value}`,
        type: "GET",
        success: function (data) {

            //OBTENER 2 PRIMEROS DIGITOS DEL CODIGO POSTAL
            let codpost = data.codpost;
            codpost = codpost.substring(0, 2);

            //ASIGNAR VALORES A LOS INPUTS
            $("#infoCliente").show();
            $("#nombre").val(data.nombre);
            $("#apellidos").val(data.apeliidos);
            $("#email").val(data.email);
            $("#telefono").val(data.telefono);
            $("#dni").val(data.dni);
            $("#codigo-postal").val(data.codpost);

            //LLAMAR AL CONTROLADOR PARA OBTENER LA PROVINCIA CON LOS 2 PRIMEROS DIGITOS DEL CODIGO POSTAL
            $.ajax({
                url: "/Usuarios/GetProvincia",
                type: 'POST',
                data: { codProv: codpost },
                success: function (data) {
                    PROVINCIA.value = data;
                },
                error: function (xhr, status, error) {
                    console.log(xhr);
                }
            });

            //ASIGNAR HREF A LOS BOTONES
            if (data.telefono != null) {
                $("#btnLlamar").attr("href", "tel:" + data.telefono);
            }
            if (data.email != null) {
                $("#btnEmail").attr("href", "mailto:" + data.email);
            }

        },
        error: function (error) {
            console.log(error);
        }
    });

});

inputViaje.addEventListener("change", () => {
    const idViaje = inputViaje.value;
    const url = `/Reservas/getPrecioViaje/${idViaje}`;

    //LLAMADA AJAX PARA OBTENER EL PRECIO DEL VIAJE Y CAMBIAR SU FORMATO
    $.ajax({
        url: url,
        type: "GET",
        success: function (data) {
            var number = parseFloat(data); 
            var roundedNumber = number.toFixed(2); 
            var formattedNumber = roundedNumber.replace(".", ",");
            $("#PrecioString").val(formattedNumber);
        },
        error: function (error) {
            console.log(error);
        }
    });
});

//EVENTO QUE SE EJECUTA CUANDO SE DEJA DE PULSAR UNA TECLA
numeroPersonas.addEventListener("keyup", () => {
    const idViaje = inputViaje.value;
    const url = `/Reservas/getPrecioViaje/${idViaje}`;

    //LLAMADA AJAX PARA OBTENER EL PRECIO DEL VIAJE Y CALCULAR EL PRECIO TOTAL
    $.ajax({
        url: url,
        type: "GET",
        success: function (data) {
            $("#PrecioString").val(data * numeroPersonas.value);
        },
        error: function (error) {
            console.log(error);
        }
    });
});

//EVENTO QUE SE EJECUTA CUANDO SE CAMBIA EL VALOR DEL INPUT
numeroPersonas.addEventListener("change", () => {
    const idViaje = inputViaje.value;
    const url = `/Reservas/getPrecioViaje/${idViaje}`;

    //LLAMADA AJAX PARA OBTENER EL PRECIO DEL VIAJE Y CALCULAR EL PRECIO TOTAL
    $.ajax({
        url: url,
        type: "GET",
        success: function (data) {
            $("#PrecioString").val(data * numeroPersonas.value);
        },
        error: function (error) {
            console.log(error);
        }
    });
});