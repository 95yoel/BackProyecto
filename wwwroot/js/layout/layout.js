const elementoHora = document.getElementById("hora");


function actualizarHora() {
    //OBTENER HORA
    var fecha = new Date();
    var dia = fecha.getDate();
    var mes = fecha.getMonth() + 1;
    var ano = fecha.getFullYear();
    var horas = fecha.getHours();
    var minutos = fecha.getMinutes();
    var segundos = fecha.getSeconds();

    //AÑADIR HORAS
    if (dia < 10) {
        dia = "0" + dia;
    }
    if (mes < 10) {
        mes = "0" + mes;
    }
    if (horas < 10) {
        horas = "0" + horas;
    }
    if (minutos < 10) {
        minutos = "0" + minutos;
    }
    if (segundos < 10) {
        segundos = "0" + segundos;
    }

    var horaActual = dia + "/" + mes + "/" + ano + " - " + horas + ":" + minutos + ":" + segundos;

    //ASIGNAR HORA ACTUAL AL ELEMENTO
    elementoHora.textContent = horaActual;
}

//ACTUALIZAR HORA CADA SEGUNDO
setInterval(actualizarHora, 1000);