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
	,checkNum: function (data) {      // checks if all characters 
		var valid="0123456789.";     // are valid numbers or a "."
		var ok=1;var checktemp;
		for(var i=0;i<data.length;i++) {
			checktemp=""+data.substring(i,i+1);
			if(valid.indexOf(checktemp)=="-1") return 0;
		}
		return 1;
	}
	,formatDate: function (dateobj) {
		return $.datepicker.formatDate('mm/dd/yy',dateobj);
	}
	,parseJSONDate: function (date) {
		return eval('new'+date.toString().replace(/\//g,' '));
	}
	,dollarAmount: function (Num) { // idea by David Turley
		dec=Num.indexOf(".");
		end=((dec> -1)?""+Num.substring(dec,Num.length):".00");
		Num=""+parseInt(Num);
		var temp1="";
		var temp2="";
		if(this.checkNum(Num)==0) {
			return "$0";
		}
		else {
			if(end.length==2) end+="0";
			if(end.length==1) end+="00";
			if(end=="") end+=".00";
			var count=0;
			for(var k=Num.length-1;k>=0;k--) {
				var oneChar=Num.charAt(k);
				if(count==3) {
					temp1+=",";
					temp1+=oneChar;
					count=1;
					continue;
				}
				else {
					temp1+=oneChar;
					count++;
				}
			}
			for(var k=temp1.length-1;k>=0;k--) {
				var oneChar=temp1.charAt(k);
				temp2+=oneChar;
			}
			temp2="$"+temp2+end;
			return temp2;
		}
	}
}