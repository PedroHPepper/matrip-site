function categoryStorage(num){
    localStorage.setItem("categoria", num);
    
}
function regionStorage(region, id){
    localStorage.setItem("region", region);
    localStorage.setItem("idregiao", id);
}
function langStorage(lang){
    localStorage.setItem("lang", lang);
}
function langTranslate(id, br, en, es){
    
    document.getElementById(id).innerHTML=br;
    if(localStorage.getItem("lang")=="br" || localStorage.getItem("lang")==null){
        nome.innerHTML=br;
        //document.write(br);
    }
    if(localStorage.getItem("lang")=="en"){
        nome.innerHTML=en;
        //document.write(en);
    }
    if(localStorage.getItem("lang")=="es"){
        nome.innerHTML=es;
        //document.write(es);
    }
    
}
