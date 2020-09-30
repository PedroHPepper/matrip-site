function checkGuardians() {
    var guardians = document.getElementsByClassName("guardian");
    if (guardians != null) {
        var achou = false;
        for (var i = 0; i < guardians.length; i++) {
            if (guardians[i].value != null && guardians[i].value != "0") {
                achou = true;
                break;
            }
        }
        if (!achou) {
            event.preventDefault();
            alert("Deve ser selecionado pelo menos um maior de idade para o passeio.");
        } else {
            document.getElementsByClassName('myForm')[0].submit();
        }
    }
}
function submitButton() {
    var selectSubtrip = document.getElementById("selectSubtrip");
    var inputTourist = document.getElementsByClassName("TouristQuantity");
    var TotalTourist = 0;
    for (var j = 0; j < inputTourist.length; j++) {
        TotalTourist += parseInt(inputTourist[j].value);
    }
    
    if (selectSubtrip.value != "") {
        var subTripID = selectSubtrip.value.replace('Subtrip:', '');
        var maxTourist = parseInt(document.getElementById("Vacancy:" + subTripID).value);

        var statusSelection = document.getElementById("statusSelection:" + subTripID);//$("#statusSelection:" + subtripID + " option:selected").val();
        var statusOption = statusSelection.options[statusSelection.selectedIndex];
        var statusOptionValue = statusOption.value;
        var status = statusOptionValue.split("/")[0];
        if (status != "Individual" && status != "Private") {
            var numPeoplePackage = status.split(":")[1];
            if (TotalTourist != numPeoplePackage) {
                event.preventDefault();
                alert("Número de pessoas não coincide com um dos pacotes.")
            }
        }
        if (TotalTourist > maxTourist) {
            event.preventDefault();
            alert("Número de turistas passou da capacidade mínima do passeio.");
        } else if (TotalTourist == 0) {
            event.preventDefault();
            alert("Por favor, selecione algum turista para o passeio.");
        } else {
            checkGuardians();
        }
        
    } else {
        event.preventDefault();
        alert("Favor, selecione algum passeio neste roteiro.");
    }
    
};
function hiddenSubtripDivs(subtripID) {
    var subtripDivList = document.getElementsByClassName("subtripDiv");
    for (var i = 0; i < subtripDivList.length; i++) {
        if (subtripDivList[i].id != subtripID) {
            subtripDivList[i].style.display = "none";
        } else {
            subtripDivList[i].style.display = "block";
        }
    }
}
function checkTotal() {
    var input = document.getElementsByTagName("input");
    var selectSubtrip = document.getElementById("selectSubtrip");
    var total = 0;

    if (selectSubtrip.value != "") {
        var subtripID = selectSubtrip.value.substring('Subtrip:'.length);
        hiddenSubtripDivs(subtripID);

        var statusSelection = document.getElementById("statusSelection:" + subtripID);
        var statusOption = statusSelection.options[statusSelection.selectedIndex];
        var statusOptionValue = statusOption.value;
        var status = statusOptionValue.split("/")[0];

        var divStatus = document.getElementById("statusSelectionArea:" + subtripID);
        divStatus.style.display = "block";
        if (status == "Individual") {
            var PreçoInput = document.getElementById(selectSubtrip.value + "-" + status);
            var SubtripPrice = parseFloat(PreçoInput.value);
            for (var z = 0; z < input.length; z++) {
                if (input[z].id == "AgeDiscountID") {
                    var AgeDiscountID = input[z].value;
                    var Discount = document.getElementById("Discount:" + AgeDiscountID).value;
                    var PeopleQuantity = document.getElementById("PeopleQuantity:" + AgeDiscountID).value;

                    total += parseFloat(PeopleQuantity) * (SubtripPrice - (SubtripPrice * (parseFloat(Discount) / 100.0)));
                }
            }
        } else {
            var PreçoInput = document.getElementById(selectSubtrip.value + "-" + status);
            var SubtripPrice = parseFloat(PreçoInput.value);

            total += SubtripPrice;
        }

        var divService = document.getElementById("divService:" + subtripID);
        if (divService != null) {
            divService.style.display = "block";
            for (var l = 0; l < input.length; l++) {
                if (input[l].id == "ServiceOf:" + subtripID) {
                    var serviceID = input[l].value.replace('Service:', '');

                    var serviceValueInput = document.getElementById(input[l].value);
                    var ServiceValue = serviceValueInput.value;

                    var serviceQuantityInput = document.getElementById('ServiceQuantity:' + serviceID);
                    var serviceQuantity = serviceQuantityInput.value;

                    total += (parseFloat(ServiceValue) * parseFloat(serviceQuantity));
                }
            }
        }
    }

    document.getElementById("totalValue").value = total;
    document.getElementsByClassName("num")[0].innerHTML = "R$ " + total.toLocaleString('pt-br', { minimumFractionDigits: 2 });
}

/*
function checkTotal() {
    var input = document.getElementsByTagName("input");
    var total = 0;
    for (var i = 0; i < input.length; i++) {
        if (input[i].type.toLowerCase() == "checkbox" && input[i].classList.contains("subtripCheckbox")) {
            if (input[i].checked) {
                var subtripID = input[i].value.substring('Subtrip:'.length);

                var statusSelection = document.getElementById("statusSelection:" + subtripID);//$("#statusSelection:" + subtripID + " option:selected").val();
                var statusOption = statusSelection.options[statusSelection.selectedIndex];
                var statusOptionValue = statusOption.value;
                var status = statusOptionValue.split("/")[0];

                var divStatus = document.getElementById("statusSelectionArea:" + subtripID);
                divStatus.style.display = "block";
                if (status == "Individual") {
                    var PreçoInput = document.getElementById(input[i].value + "-" + status);
                    var SubtripPrice = parseFloat(PreçoInput.value);
                    for (var z = 0; z < input.length; z++) {
                        if (input[z].id == "AgeDiscountID") {
                            var AgeDiscountID = input[z].value;
                            var Discount = document.getElementById("Discount:" + AgeDiscountID).value;
                            var PeopleQuantity = document.getElementById("PeopleQuantity:" + AgeDiscountID).value;

                            total += parseFloat(PeopleQuantity) * (SubtripPrice - (SubtripPrice * (parseFloat(Discount) / 100.0)));
                        }
                    }
                } else  {
                    var PreçoInput = document.getElementById(input[i].value + "-" + status);
                    var SubtripPrice = parseFloat(PreçoInput.value);

                    total += SubtripPrice;
                }
                        
                var divService = document.getElementById("divService:" + subtripID);
                if (divService != null) {
                    divService.style.display = "block";
                    for (var l = 0; l < input.length; l++) {
                        if (input[l].id == "ServiceOf:" + subtripID) {
                            var serviceID = input[l].value.replace('Service:', '');

                            var serviceValueInput = document.getElementById(input[l].value);
                            var ServiceValue = serviceValueInput.value;

                            var serviceQuantityInput = document.getElementById('ServiceQuantity:' + serviceID);
                            var serviceQuantity = serviceQuantityInput.value;

                            total += (parseFloat(ServiceValue) * parseFloat(serviceQuantity));
                        }
                    }
                }
                   
               
            }
            if (input[i].checked == false) {
                var subtripID = input[i].value.substring('Subtrip:'.length);
                var divService = document.getElementById("divService:" + subtripID);
                if (divService != null) {
                    divService.style.display = "none";
                }

                var divStatus = document.getElementById("statusSelectionArea:" + subtripID);
                
                divStatus.style.display = "none";
            }
        }
    }
    document.getElementsByName("total")[0].value = total;
    document.getElementsByClassName("num")[0].innerHTML = "R$ " + total.toLocaleString('pt-br', { minimumFractionDigits: 2 });
}*/
jQuery(document).ready(function () {
    document.getElementsByClassName("num")[0].innerHTML = "R$ " + parseFloat(document.getElementById("totalValue").value).toLocaleString('pt-br', { minimumFractionDigits: 2 });
});