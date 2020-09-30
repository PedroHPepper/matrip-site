function RequestAutocomplete(url, id) {
    jQuery.ajax({
        url: url,
        type: "get",
        success: function (data) {
            var accentMap = {
                "á": "a",
                "ã": "a",
                "â": "a",
                "ç": "c",
                "é": "e",
                "ê": "e",
                "í": "i",
                "ó": "o",
                "ú": "u"
            };
            var normalize = function (term) {
                var ret = "";
                for (var i = 0; i < term.length; i++) {
                    ret += accentMap[term.charAt(i)] || term.charAt(i);
                }
                return ret;
            };
            jQuery(id).autocomplete({
                source: function (request, response) {
                    var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
                    response($.grep(data, function (value) {
                        value = value.label || value.value || value;
                        return matcher.test(value) || matcher.test(normalize(value));
                    }));
                }
            });
        }
    });
}