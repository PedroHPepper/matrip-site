function submit(){
  window.location.replace('form1.html');
}


$(function() {

    var idregiao = localStorage.getItem("idregiao");
    var idpasseio = localStorage.getItem("idpasseio");
    
    $.getJSON('json/passeio'+idregiao+'.json', function(data) {         
      $("#titulo").append(data.passeio[idpasseio].name);
      $('.texto').append(data.passeio[idpasseio].description)
  });

  
  



 
});
 
