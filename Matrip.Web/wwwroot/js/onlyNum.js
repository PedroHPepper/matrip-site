function SomenteNumero(e) {
    var tecla; //Armazena a tecla pressionada.
        
    if (e.which) {
        tecla = e.which;
    } else {
        tecla = e.keyCode;
    }

    if ((tecla >= 48 && tecla <= 57) || (e.which == 08)) {
        return true;
    } else {
        return false;
    }
}