(function ($) {
	$.fn.serializeForm=function () {
		var frm=this;
		$(":input[onkeydown]",frm).each(function () {
			this.value=this.value.replace("%","");
		});
		return $(frm).serializeArray();
	};
})(jQuery);