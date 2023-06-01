const site1 = document.getElementById('site1');
const site2 = document.getElementById('site2');
const timeRange = document.getElementById('timeRange');
const btnSubmit = document.getElementById('btnSubmit');


btnSubmit.addEventListener("click", () => {

    var pagina1 = site1.value;
    var pagina2 = site2.value;
    var rango = timeRange.value;


    $.ajax({
        url: "/Home/cargarTrends",
        type: "GET",
        data: {
            site1: pagina1,
            site2: pagina2,
            timeRange: rango
        },
        success: function (response) {
            $("#contenidoGoogleTrends").html(response);
        },
        error: function (response) {
            console.log("error");
        }
    });

});
