const inputUsuario = document.getElementById("UsuarioId");
const inputViaje = document.getElementById("ViajeId");
const numeroPersonas = document.getElementById("NumeroPersonas");
const precioString = document.getElementById("precio");
const NumeroPersonas = document.getElementById("NumeroPersonas");
//let valorId = @Model.Id;
//let viajeId = @Model.ViajeId;

$(document).ready(function () {


    $.ajax({
        url: `/Reservas/getDatosCliente/${inputUsuario.value}`,
        type: "GET",
        success: function (data) {
            $("#infoCliente").show();
            $("#nombre").val(data.nombre);
            $("#apellidos").val(data.apeliidos);
            $("#email").val(data.email);
            $("#telefono").val(data.telefono);
            $("#dni").val(data.dni);
            $("#codigo-postal").val(data.codpost);
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
inputUsuario.addEventListener("change", () => {

    $.ajax({
        url: `/Reservas/getDatosCliente/${inputUsuario.value}`,
        type: "GET",
        success: function (data) {
            $("#infoCliente").show();
            $("#nombre").val(data.nombre);
            $("#apellidos").val(data.apeliidos);
            $("#email").val(data.email);
            $("#telefono").val(data.telefono);
            $("#dni").val(data.dni);
            $("#codigo-postal").val(data.codpost);
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



$(document).ready(function () {



    $.ajax({
        url: "/Reservas/GetPrecio",
        type: "GET",
        data: { id: reservasData.Id },
        success: function (data) {
            if (data == null) {
                data = 0;
            }
            var number = String(data.precio);
            number = number.replace(".", ",");
            $("#PrecioString").val(number);

        }, error: function (error) {
            console.log(error);
        }
    });

    numeroPersonas.addEventListener("change", () => {

        var precio = document.getElementById("PrecioString").value;
        var numPersonas = document.getElementById("NumeroPersonas");
        console.log(numPersonas.value);
        $.ajax({
            url: "/Reservas/getPrecioViaje",
            type: "GET",
            data: { id: reservasData.ViajeId },
            success: function (data) {
                if (data == null) {
                    data = 0;
                }
                precioNumber = Number(data);
                console.log(precioNumber);
                precioNumber = precioNumber * numPersonas.value;
                console.log(precioNumber);
                precioNumber = String(precioNumber);
                precioNumber = precioNumber.replace(".", ",");
                console.log(precioNumber);
                $("#PrecioString").val(precioNumber);

            }, error: function (error) {
                console.log(error);
            }
        });





    });




});