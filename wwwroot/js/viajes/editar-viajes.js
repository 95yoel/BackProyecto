const precioString = document.getElementById("precio");
var id = document.getElementById("Id").value;

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
