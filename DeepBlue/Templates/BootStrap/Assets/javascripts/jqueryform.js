(function ($) {
	$.fn.serializeForm=function () {
		var frm=this;
		try {
			$(":input[webaddress='true']",frm).each(function () {
				if($.trim(this.value)!="") {
					var regexp=/(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/
					if(regexp.test(this.value)==false) {
						if(this.value.toLowerCase().indexOf("http://")<=0
							&&
							this.value.toLowerCase().indexOf("https://")<=0
							) {
							this.value="http://"+this.value;
						}
					}
				}
			});
		} catch(e) {
			//alert(e); 
		}
		var params=$(frm).serializeArray();
		params=jHelper.checkParams(frm,params);
		$.each(params,function (i,par) {
			var element=$(":input[name='"+par.name+"'][onkeydown]",frm).get(0);
			if(element) {
				var value=par.value;
				value=value.replace(/\$/g,'');
				value=value.replace(/\%/g,'');
				value=value.replace(/\,/g,'');
				par.value=value;
			}
		});
		return params;
	};
})(jQuery);