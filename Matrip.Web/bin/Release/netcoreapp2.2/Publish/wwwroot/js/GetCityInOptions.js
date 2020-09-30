function getSelectedOption(sel) {
    var opt;
    for (var i = 0, len = sel.options.length; i < len; i++) {
        opt = sel.options[i];
        if (opt.selected === true) {
            break;
        }
    }
    return opt;
}

function redirectWithCategoryName(categoryName) {
    // get references to select list and display text box
    var sel = document.getElementById('cities');
    
    var text = sel.options[sel.selectedIndex].text;
    var UF = text.substr(0, 2);
    var city = sel.options[sel.selectedIndex].value;

    window.location.href = "https://localhost:44340/tripbyCategory/" + categoryName + "/" + UF + "/" + encodeURI(city) + "/1";
}

function RedirectWithCityID(CityID, UF) {
    window.location.href = "https://localhost:44340/Trip/" + UF + "/" + CityID + "/1";
}