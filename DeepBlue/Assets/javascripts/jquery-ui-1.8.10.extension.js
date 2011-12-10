var cache={};
(function ($) {
	$.ui.autocomplete.prototype.options.autoSelect=true;
	$.ui.autocomplete.prototype.options.cacheNames=["/Admin/FindCountrys","/Admin/FindStates"];
	/*
	$(".ui-autocomplete-input").live("blur",function (event) {
		var autocomplete=$(this).data("autocomplete");
		if(!autocomplete.options.autoSelect||autocomplete.selectedItem) { return; }

		var matcher=new RegExp("^"+$.ui.autocomplete.escapeRegex($(this).val())+"$","i");
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
	*/
	$(".ui-autocomplete-input").live("autocompletechange",function (event,ui) {
		var autocomplete=$(this).data("autocomplete");
		if(!autocomplete.options.autoSelect||autocomplete.selectedItem) { return; }
		if(!ui.item) {
			var matcher=new RegExp("^"+$.ui.autocomplete.escapeRegex($(this).val())+"$","i");
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
		}
		if(!ui.item) {
			autocomplete._trigger("select",event,{ item: { id: "0",value: "",label: "",otherid: "0",otherid2: "0",othervalues: [],option: null} });
		}
	});
	$(".ui-autocomplete-input").live("autocompletesearch",function (event,ui) {
		var autocomplete=$(this).data("autocomplete");
		var source=autocomplete.options.source;
		var search=autocomplete.options.search;
		var cacheNames=autocomplete.options.cacheNames;
		var isCacheName=false;
		for(var i=0;i<cacheNames.length;i++) {
			if(cacheNames[i]==source) {
				isCacheName=true;
			}
		}
		if(source in cache) {
			$(this).autocomplete("option","source",cache[source]);
		}
		return true;
	});
	$(".ui-autocomplete-input").live("autocompleteopen",function (event,ui) {
		var autocomplete=$(this).data("autocomplete");
		var data=new Array();
		autocomplete.widget().children(".ui-menu-item").each(function () {
			data[data.length]=$(this).data("item.autocomplete");
		});
		var source=autocomplete.options.source;
		var search=autocomplete.options.search;
		var cacheNames=autocomplete.options.cacheNames;
		var isCacheName=false;
		for(var i=0;i<cacheNames.length;i++) {
			if(cacheNames[i]==source) {
				isCacheName=true;
			}
		}
		if(isCacheName&&search=="") {
			cache[source]=data;
			$(this).autocomplete("option","source",data);
		}
	});
} (jQuery));
 