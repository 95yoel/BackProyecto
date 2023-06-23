const DNI = document.getElementById("DNI");
const CODPOST = document.getElementById("CODPOST");
const TELEFONO = document.getElementById("Telefono");
const PROVINCIA = document.getElementById("PROVINCIA");
const EMAIL = document.getElementById("Email");

//OBTIENE LA LETRA DEL DNI AL INTRODUCIR LOS 8 NUMEROS
DNI.addEventListener("input", () => {

    let longitud = DNI.value.length;
    if (longitud == 8) {
        DNI.value += obtenerLetraDNI(DNI.value);
        CODPOST.focus();
    }

});

//BORRA EVITANDO QUE SE INTRODUZCAN NUEVAS LETRAS
DNI.addEventListener("keydown", (event) => {
    let longitud = DNI.value.length;
    if (longitud === 8 || event.key === 'Backspace') {
        event.preventDefault();
        DNI.value = DNI.value.slice(0, -1);
    }
});
//EVITA QUE SE INTRODUZCAN LETRAS EN EL DNI
DNI.addEventListener("keyup", (e) => {
    let longitud = DNI.value.length;
    var campo = e.target;
    var charCode = e.key.charCodeAt(0);
    if (longitud <= 8) {
        if ((charCode >= 65 && charCode <= 90) || (charCode >= 97 && charCode <= 122)) {
            campo.value = campo.value.slice(0, -1);
        }
    }

});

//EVITA QUE SE INTRODUZCAN MAS DE 9 NUMEROS EN EL DNI
DNI.addEventListener("input", () => {
    let longitud = DNI.value.length;
    if (longitud > 9) {
        DNI.value = DNI.value.slice(0, -1);
        CODPOST.focus();
    }
});


CODPOST.addEventListener("input", () => {
    let longitud = CODPOST.value.length;
    if (longitud == 2) {
        
        console.log(CODPOST.value);
        //LLAMA AL METODO GETPROVINCIA DEL CONTROLADOR USUARIOS PARA OBTENER LA PROVINCIA  CON EL CODIGO POSTAL DE 2 DIGITOS
        $.ajax({
            url: "/Usuarios/GetProvincia",
            type: 'POST',
            data: { codProv: CODPOST.value },
            success: function (data) {
                console.log(data);
                PROVINCIA.value = data;
            },
            error: function (xhr, status, error) {
                console.log(xhr);
            }
        });

    }
    if (longitud == 5) {
        TELEFONO.focus();
    }

});

//CAMBIA EL FOCO AL EMAIL CUANDO EL TELEFONO YA TIENE 9 NUMEROS
TELEFONO.addEventListener("keydown", (event) => {
    let longitud = TELEFONO.value.length;

    if (longitud == 9) {
        EMAIL.focus();
    }
});


//EVITAR QUE SE LE PUEDA INTRODUCIR LETRAS AL TELEFONO
TELEFONO.addEventListener("keyup", (e) => {
    var campo = e.target;
    var charCode = e.key.charCodeAt(0);

    if ((charCode >= 65 && charCode <= 90) || (charCode >= 97 && charCode <= 122)) {
        campo.value = campo.value.slice(0, -1);
    }
});

//FUNCION PARA OBTENER LA LETRA DEL DNI
function obtenerLetraDNI(numeroDNI) {
    var letras = "TRWAGMYFPDXBNJZSQVHLCKE";
    var indice = numeroDNI % 23;
    return letras.charAt(indice);
}