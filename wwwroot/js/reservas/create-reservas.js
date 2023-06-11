const inputUsuario = document.getElementById("UsuarioId");
const inputViaje = document.getElementById("ViajeId");
const numeroPersonas = document.getElementById("NumeroPersonas");
const PROVINCIA = document.getElementById("Provincia");
$(document).ready(function () {

});

inputUsuario.addEventListener("change", () => {

    $.ajax({
        url: `/Reservas/getDatosCliente/${inputUsuario.value}`,
        type: "GET",
        success: function (data) {

            let codpost = data.codpost;
            codpost = codpost.substring(0, 2);

            $("#infoCliente").show();
            $("#nombre").val(data.nombre);
            $("#apellidos").val(data.apeliidos);
            $("#email").val(data.email);
            $("#telefono").val(data.telefono);
            $("#dni").val(data.dni);
            $("#codigo-postal").val(data.codpost);
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
    $.ajax({
        url: url,
        type: "GET",
        success: function (data) {
            var number = parseFloat(data); // Parse the data to a float number
            var roundedNumber = number.toFixed(2); // Round the number to two decimal places
            var formattedNumber = roundedNumber.replace(".", ","); // Replace the decimal separator if necessary
            $("#PrecioString").val(formattedNumber);
        },
        error: function (error) {
            console.log(error);
        }
    });
});


numeroPersonas.addEventListener("keyup", () => {
    const idViaje = inputViaje.value;
    const url = `/Reservas/getPrecioViaje/${idViaje}`;
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
numeroPersonas.addEventListener("change", () => {
    const idViaje = inputViaje.value;
    const url = `/Reservas/getPrecioViaje/${idViaje}`;
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