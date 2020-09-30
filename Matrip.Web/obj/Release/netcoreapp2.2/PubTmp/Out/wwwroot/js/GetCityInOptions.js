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
    /*
    var text = sel.options[sel.selectedIndex].text;
    var UF = text.substr(0, 2);
    var city = sel.options[sel.selectedIndex].value;
    */
    var text = sel.value;
    var UF = text.split(" - ")[1];
    var city = text.split(" - ")[0];

    if (city != null && UF != null) {
        window.location.href = window.location.href + "tripbyCategory/" + categoryName + "/" + UF + "/" + encodeURI(city) + "/1";
    }

}

function RedirectWithCityID(CityID, UF)
{
    window.location.href = window.location.href + "Trip/" + UF + "/" + CityID + "/1";
}