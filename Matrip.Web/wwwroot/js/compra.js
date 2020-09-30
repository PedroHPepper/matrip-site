function submit(){
  window.location.replace('form1.html');
}


jQuery(function() {

    var idregiao = localStorage.getItem("idregiao");
    var idpasseio = localStorage.getItem("idpasseio");
    
    jQuery.getJSON('json/passeio'+idregiao+'.json', function(data) {         
      jQuery("#titulo").append(data.passeio[idpasseio].name);
      jQuery('.texto').append(data.passeio[idpasseio].description)
  });

  
  



 
});
 
