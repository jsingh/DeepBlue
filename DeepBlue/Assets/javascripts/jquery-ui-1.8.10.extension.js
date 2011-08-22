(function ($) {
	$.ui.autocomplete.prototype.options.autoSelect=true;
	$(".ui-autocomplete-input").live("blur",function (event) {
		var autocomplete=$(this).data("autocomplete");
		if(!autocomplete.options.autoSelect||autocomplete.selectedItem) { return; }
		var matcher=new RegExp("^"+$.ui.autocomplete.escapeRegex($.trim($(this).val()).toLowerCase())+"$","i");
		autocomplete.widget().children(".ui-menu-item").each(function () {
			var item=$(this).data("item.autocomplete");
			if(matcher.test($.trim(item.value.toLowerCase()))) {
				autocomplete.selectedItem=item;
				return false;
			}
		});
		if(autocomplete.selectedItem) {
			autocomplete._trigger("select",event,{ item: autocomplete.selectedItem });
		}
	});
	$(".ui-autocomplete-input").live("autocompletechange",function (event,ui) {
		if(!ui.item) {
			var autocomplete=$(this).data("autocomplete");
			autocomplete._trigger("select",event,{ item: { id: "0",value: "",label: "",otherid: "0",otherid2: "0",othervalues: [],option: null} });
		}
	});
} (jQuery));
 