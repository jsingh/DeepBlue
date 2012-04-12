(function ($) {


	// Shortuct functions
	jAlert=function (message,title,callback) {
		//$.alerts.alert(message,title,callback);
		if(title==undefined) {
			title="DeepBlue";
		}
		var tmpl='<div class="modal" id="jqueryAlertBox">'+
			    '<div class="modal-header"><a class="close" data-dismiss="modal">x</a><h3>${title}</h3></div>'+
			    '<div class="modal-body">'+
			      '<p>{{html message}}</p>'+
				'</div>'+
				'<div class="modal-footer"><button id="ok" class="btn btn-primary">OK</button>'+
			  '</div>';
		$("#jqueryAlertBox").remove();

		if(message!=undefined) {
			message=message.replace(/\n/g,'<br />');
		} else {
			message="";
		}
		var data={ "title": title,"message": message };
		$.template("alertboxtemplate",tmpl);
		$.tmpl("alertboxtemplate",data).appendTo($("body"));
		var m=$("#jqueryAlertBox");
		$("#ok",m).click(function () {
			$(m).modal('hide');
			if(callback) { callback(); }
		});
		$(m).modal();
		$("#ok",m).focus();
	}

	jConfirm=function (message,title,callback) {
		//$.alerts.confirm(message,title,callback);
	};

	jPrompt=function (message,value,title,callback) {
		//$.alerts.prompt(message,value,title,callback);
	};

})(jQuery);