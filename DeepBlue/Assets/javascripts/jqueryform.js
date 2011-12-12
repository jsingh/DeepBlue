(function ($) {
	$.fn.serializeForm=function () {
		var frm=this;
		$(":input[onkeydown]",frm).each(function () {
			this.value=this.value.replace("%","");
		});
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
		return $(frm).serializeArray();
	};
})(jQuery);