var logon={
	init: function () {
		//logon.logonpos();			
		var frm=document.forms[0];
		var userName=$("#UserName",frm);
		userName.focus();
		var err="";
		$(".field-validation-error").each(function () { err+="<p>"+this.innerHTML+"</p>"; });
		if($.trim(err)!="") {
			var alertRow=$("#AlertRow");
			alertRow.empty();
			var data={ iswarning: false,message: err };
			$("#alertTemplate").tmpl(data).appendTo(alertRow);
			$(".alert").alert();
		}
	}
	,submit: function (frm) {
		var err="";
		var userName=$("#UserName",frm);
		var passWord=$("#Password",frm);
		var entityCode=$("#EntityCode",frm);
		var txt;

		if($.trim(userName.val())=="") {
			err+="<p>UserName is required</p>";
			txt=userName;
		}
		if($.trim(passWord.val())=="") {
			if(err=="")
				txt=passWord;

			err+="<p>Password is required</p>";
		}
		if($.trim(entityCode.val())=="") {
			if(err=="")
				txt=entityCode;

			err+="<p>Entity Code is required</p>";
		}
		if(err!="") {
			var alertRow=$("#AlertRow");
			alertRow.empty();
			var data={ iswarning: true,message: err };
			$("#alertTemplate").tmpl(data).appendTo(alertRow);
			$(".alert").alert();
			txt.focus();
			return false;
		}
		return true;
	}
}

 