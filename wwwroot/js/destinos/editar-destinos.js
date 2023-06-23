const foto = document.getElementById('foto');

let display = "none";


//MOSTRAR / OCULTAR FOTO

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