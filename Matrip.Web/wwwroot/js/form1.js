function submitForm() {
   // Get the first form with the name
   // Usually the form name is not repeated
   // but duplicate names are possible in HTML
   // Therefore to work around the issue, enforce the correct index
   var frm = document.getElementsByClassName('contact_input')[0];
   alert("Dados Enviados com Sucesso!!!")
   frm.submit(); // Submit the form
   frm.reset();  // Reset all form data
   return false; // Prevent page refresh
}

jQuery('#passeio').val(localStorage.getItem('passeio'));