var logon={
	init: function () {
		logon.logonpos();			
		var frm=document.forms[0];
		var userName=$("#UserName",frm);
		userName.focus();
		var err="";
		$(".field-validation-error").each(function () {  err+=this.innerHTML+"\n"; });
		if($.trim(err)!="") {
			jAlert(err);
		}
	}
	,submit: function (frm) {
		var err="";
		var userName=$("#UserName",frm);
		var passWord=$("#Password",frm);
		var txt;
		if($.trim(userName.val())=="") {
			err+="UserName is required\n";
			txt=userName;
		}
		if($.trim(passWord.val())=="") {
			if(err=="")
				txt=passWord;

			err+="Password is required\n";
		}
		if(err!="") {
			jAlert(err);
			txt.focus();
			return false;
		}
		return true;
	},
	logonpos: function(){
		var winwidth = $(window).width();
		if(winwidth < 1007) winwidth= 1007;
		$("#logon").css("top",(($(window).height()/ 2) - ($("#logon").height()/2)) -38);
		$("#logon").css("left",((winwidth/ 2) - ($("#logon").width()/2)));
	}
}

window.onresize = logon.logonpos;