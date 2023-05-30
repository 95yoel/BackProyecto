const precioString = document.getElementById("precio");
var id = document.getElementById("Id").value;

const foto = document.getElementById('foto');
let display = "none";

const url = `/Reservas/getPrecioViaje/${id}`;

$.ajax({
	url: url,
	type: "GET",
	success: function (data) {
		var number = String(data);
		number = number.replace(".", ",");
		$("#PrecioString").val(number);
		
	},
	error: function (error) {
		console.log(error);
	}
});


foto.addEventListener('click', () => {

	console.log(display);
	if (display == "none") {
		document.getElementById('verFoto').style.display = "block";
		display = "block";
	} else {
		document.getElementById('verFoto').style.display = "none";
		display = "none";
	}

});
