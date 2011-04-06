var jHelper={
	isNumeric: function (evt) {
		evt=(evt)?evt:window.event
		var charCode=(evt.which)?evt.which:evt.keyCode;
		if((charCode>31&&(charCode<48||charCode>57))&&(charCode!=37&&charCode!=39)) {
			return false;
		}
		return true
	}
	,isCurrency: function (evt) {
		evt=(evt)?evt:window.event
		var charCode=(evt.which)?evt.which:evt.keyCode;
		if((charCode>31&&(charCode<48||charCode>57))&&(charCode!=37&&charCode!=39&&charCode!=46)) {
			return false;
		}
		return true
	}
	,cfloat: function (value) {
		var retValue=parseFloat(value);
		if(isNaN(retValue))
			return 0;
		else
			return retValue;
	}
	,checkEmail: function (txt) {
		if($.trim(txt.value)!="") {
			var rExp=new RegExp("^[\\w-_\.]*[\\w-_\.]\@[\\w]\.+[\\w]+[\\w]$");
			if(rExp.test(txt.value)==false) {
				alert("Invalid Email");
				txt.value="";
			}
		}
	}
	,checkWebAddress: function (txt) {
		if($.trim(txt.value)!="") {
			if(txt.value.toLowerCase().indexOf("http://")<0&&txt.value.toLowerCase().indexOf("https://")<0) {
				txt.value="http://"+txt.value;
			}
			var rExp=new RegExp("^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$");
			if(rExp.test(txt.value)==false) {
				alert("Invalid Web Address");
				txt.value="";
			}
		}
	}
}