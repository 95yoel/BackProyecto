const inputUsuario = document.getElementById("UsuarioId");
const inputViaje = document.getElementById("ViajeId");
const numeroPersonas = document.getElementById("NumeroPersonas");
const precioString = document.getElementById("precio");
const NumeroPersonas = document.getElementById("NumeroPersonas");
const PROVINCIA = document.getElementById("Provincia");
//let valorId = @Model.Id;
//let viajeId = @Model.ViajeId;


//EVENTO QUE SE EJECUTA AL CARGAR LA PAGINA 
$(document).ready(function () {

    //LLAMADA AJAX PARA OBTENER LOS DATOS DEL CLIENTE
    $.ajax({
        url: `/Reservas/getDatosCliente/${inputUsuario.value}`,
        type: "GET",
        success: function (data) {

            //OBTENEMOS LOS DOS PRIMEROS DIGITOS DEL CODIGO POSTAL
            let codpost = data.codpost;
            codpost = codpost.substring(0, 2);

            $("#infoCliente").show();
            $("#nombre").val(data.nombre);
            $("#apellidos").val(data.apeliidos);
            $("#email").val(data.email);
            $("#telefono").val(data.telefono);
            $("#dni").val(data.dni);
            $("#codigo-postal").val(data.codpost);
            //LAMADA AJAX PARA OBTENER LA PROVINCIA A PARTIR DE LOS DOS PRIMEROS DIGITOS DEL CODIGO POSTAL
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
            //ASIGNACION DE ENLACES DE TELEFONO Y EMAIL
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

//EVENTO QUE SE EJECUTA AL CAMBIAR EL VALOR DEL INPUT DE USUARIO
inputUsuario.addEventListener("change", () => {

    $.ajax({
        url: `/Reservas/getDatosCliente/${inputUsuario.value}`,
        type: "GET",
        success: function (data) {

            //OBTENER DOS PRIMEROS DIGITOS DEL CODIGO POSTAL
            let codpost = data.codpost;
            codpost = codpost.substring(0, 2);

            $("#infoCliente").show();
            $("#nombre").val(data.nombre);
            $("#apellidos").val(data.apeliidos);
            $("#email").val(data.email);
            $("#telefono").val(data.telefono);
            $("#dni").val(data.dni);
            $("#codigo-postal").val(data.codpost);

            //LLAMADA AJAX PARA OBTENER LA PROVINCIA A PARTIR DE LOS DOS PRIMEROS DIGITOS DEL CODIGO POSTAL
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



$(document).ready(function () {


    //LLAMADA PARA CAMBIAR EL FORMATO DEL PRECIO
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

    //EVENTO QUE SE EJECUTA AL CAMBIAR EL VALOR DEL INPUT DE NUMERO DE PERSONAS
    numeroPersonas.addEventListener("change", () => {

        var precio = document.getElementById("PrecioString").value;
        var numPersonas = document.getElementById("NumeroPersonas");
        console.log(numPersonas.value);

        //LLAMADA AJAX PARA OBTENER EL PRECIO DEL VIAJE Y FORMATEARLO
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