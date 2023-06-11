const DNI = document.getElementById("DNI");
const CODPOST = document.getElementById("CODPOST");
const TELEFONO = document.getElementById("Telefono");
const PROVINCIA = document.getElementById("PROVINCIA");
const EMAIL = document.getElementById("Email");
DNI.addEventListener("input", () => {

    let longitud = DNI.value.length;
    if (longitud == 8) {
        DNI.value += obtenerLetraDNI(DNI.value);
        CODPOST.focus();
    }

});
DNI.addEventListener("keydown", (event) => {
    let longitud = DNI.value.length;
    if (longitud === 8 || event.key === 'Backspace') {
        event.preventDefault();
        DNI.value = DNI.value.slice(0, -1);
    }
});
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

TELEFONO.addEventListener("keydown", (event) => {
    let longitud = TELEFONO.value.length;

    if (longitud == 9) {
        EMAIL.focus();
    }
});

TELEFONO.addEventListener("keyup", (e) => {
    var campo = e.target;
    var charCode = e.key.charCodeAt(0);

    if ((charCode >= 65 && charCode <= 90) || (charCode >= 97 && charCode <= 122)) {
        campo.value = campo.value.slice(0, -1);
    }
});
function obtenerLetraDNI(numeroDNI) {
    var letras = "TRWAGMYFPDXBNJZSQVHLCKE";
    var indice = numeroDNI % 23;
    return letras.charAt(indice);
}