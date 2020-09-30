function passeioStorage(passeio, i){
    localStorage.setItem("passeio", passeio);
    localStorage.setItem("idpasseio", i);
}

function showStates(passeio, i){
    var region = localStorage.getItem('region');

    var div1 = document.createElement('div');
    var plus = document.createTextNode('+');
    div1.appendChild(plus);
    var a = document.createElement('a');
    a.setAttribute('href', 'compra.html');
    var stringRegion = document.createTextNode(passeio.name);
    a.appendChild(stringRegion);
    a.appendChild(div1);
    a.addEventListener('click', function(){
        passeioStorage(passeio.name, i);
    });
    var div2 = document.createElement('div');
    div2.appendChild(a);
    var div3 = document.createElement('div');
    div3.setAttribute('class', 'location_title text-center');
    div3.appendChild(div2);
    var img = document.createElement('img');
    img.setAttribute('src', passeio.photo);
    img.setAttribute('alt', '');
    var div4 = document.createElement('div');
    div4.setAttribute('class', 'location');
    div4.setAttribute('id', 'bloco');
    div4.appendChild(img);
    div4.appendChild(div3);
    div4.style.width = "calc((100% - 100px) / 3)";

    document.getElementById("estados").appendChild(div4); 
}

function getRegion(){
    var regionStorage = localStorage.getItem("region");
    document.getElementById("homeTitle").innerHTML = "Passeios de " + regionStorage;

    return regionStorage; 
}


var regionStorage = getRegion();

var saoluis = ["City Tour Centro Histórico", "Praias", "Rota da Juçara"];
var alcantara = ["City Tour"];
var lencois = ["Atrativos em Barreirinhas", "Atrativos em Santo Amaro"];
var raposa = ["Atrativos em São José de Ribamar", "Passeio Náutico Raposa"];
var carolina = ["Atrativos de Carolina"];

    if(regionStorage == 'São Luís'){
        var regiao = saoluis;  
    }else if(regionStorage =='Alcântara'){
        var regiao = alcantara;
    }else if(regionStorage == 'Lençóis Maranhenses'){
        var regiao = lencois;
    }else if(regionStorage == 'Raposa e Ribamar'){
        var regiao = raposa;
    }else if(regionStorage == 'Carolina'){
        var regiao = carolina;
    }else{
        window.location.replace("index.html");
    }

    var idregiao = localStorage.getItem("idregiao");
    
    jQuery.getJSON('json/passeio'+idregiao+'.json', function(data) {         
        //$("#titulo").append(data.passeio[idpasseio].name);
       // $('.texto').append(data.passeio[idpasseio].description)
        
        for(let i=0; i<data.passeio.length; i++){
            showStates(data.passeio[i], i);
        }
        
    });
    
    
    

