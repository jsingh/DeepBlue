var logon={
	init: function () {
		//logon.logonpos();			
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
		var ww = $(window).width();
		var wh = $(window).height();
		var lh  = $("#logon").innerHeight();
		var lw  = $("#logon").innerWidth();
		var top = ((wh/ 2) - (lh/2));
		var left = ((ww/ 2) - (lw/2));
		$("#logon")
		.css({
		"top": top, "left": left, "position": "absolute"
		});
	}
}
$(document).ready(function(){
	 logon.logonpos();
});
$(window).resize(function(){
	logon.logonpos();
});
