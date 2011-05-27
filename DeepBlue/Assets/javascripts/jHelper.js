var jHelper={
	isNumeric: function (evt) {
		evt=(evt)?evt:window.event
		var charCode=(evt.which)?evt.which:evt.keyCode;
		if((charCode>31&&(charCode<48||charCode>57))&&(charCode!=37&&charCode!=39&&charCode!=45)) {
			return false;
		}
		return true
	}
	,isCurrency: function (evt) {
		evt=(evt)?evt:window.event
		var charCode=(evt.which)?evt.which:evt.keyCode;
		if((charCode>31&&(charCode<48||charCode>57))&&(charCode!=37&&charCode!=39&&charCode!=46&&charCode!=45)) {
			return false;
		}
		return true
	}
	,cfloat: function (value) {
		var retValue=parseFloat(value);if(isNaN(retValue)) { return 0; } else { return retValue; }
	}
	,checkEmail: function (txt) {
		if($.trim(txt.value)!="") {
			var rExp=new RegExp("^[\\w-_\.]*[\\w-_\.]\@[\\w]\.+[\\w]+[\\w]$");
			if(rExp.test(txt.value)==false) { alert("Invalid Email");txt.value=""; }
		}
	}
	,checkWebAddress: function (txt) {
		if($.trim(txt.value)!="") {
			if(txt.value.toLowerCase().indexOf("http://")<0&&txt.value.toLowerCase().indexOf("https://")<0) { txt.value="http://"+txt.value; }
			var rExp=new RegExp("^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$");
			if(rExp.test(txt.value)==false) { alert("Invalid Web Address");txt.value=""; }
		}
	}
	,checkNum: function (data) {
		var valid="0123456789.";
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
		try { return eval('new'+date.toString().replace(/\//g,' ')); } catch(e) { return date; }
	}
	,dollarAmount: function (Num) { // idea by David Turley
		dec=Num.indexOf(".");
		end=((dec> -1)?""+Num.substring(dec,Num.length):".00");
		Num=""+parseInt(Num);
		var temp1="";var temp2="";
		if(end.length==2) { end+="0"; } if(end.length==1) { end+="00"; } if(end=="") { end+=".00"; }
		var count=0;
		for(var k=Num.length-1;k>=0;k--) { var oneChar=Num.charAt(k);if(count==3) { temp1+=",";temp1+=oneChar;count=1;continue; } else { temp1+=oneChar;count++; } }
		for(var k=temp1.length-1;k>=0;k--) { var oneChar=temp1.charAt(k);temp2+=oneChar; }
		temp2="$"+temp2+end;
		if(parseFloat(Num)<0) { temp2=temp2.replace("$-,","$(").replace("$-","$(")+")"; }
		return temp2;
	}
	,serialize: function (target) {
		var param=[];
		$(":input",target).each(function () {
			var type=$(this).attr("type");
			switch(type) {
				case "checkbox": if(this.checked) { param[param.length]=jHelper.getParam(this); } break;
				case "radio": if(this.checked) { param[param.length]=jHelper.getParam(this); } break;
				default: param[param.length]=jHelper.getParam(this);break;
			}
		});
		return param;
	}
	,getParam: function (input) {
		return { name: $(input).attr("name"),value: $(input).val() };
	}
	,loadDropDown: function (ddl,data) {
		ddl.options.length=null;
		if(data!=null) {
			for(i=0;i<data.length;i++) {
				listItem=new Option(data[i].Text,data[i].Value,false,false);
				ddl.options[ddl.options.length]=listItem;
			}
		}
	}
	,formSubmit: function (formId,checkAutoComplete) {
		try {
			var frm=document.getElementById(formId);
			var message='';
			if(isNaN(checkAutoComplete)) { checkAutoComplete=true; }
			if(checkAutoComplete) {
				$(".field-validation-error",frm).each(function () {
					if(this.innerHTML!='') {
						message+=this.innerHTML+"\n";
					}
				});
			}
			if(message!="") {
				alert(message);
				return false;
			}
			Sys.Mvc.FormContext.getValidationForForm(frm).validate('submit');
			$(".field-validation-error",frm).each(function () {
				if(this.innerHTML!='') {
					message+=this.innerHTML+"\n";
				}
			});
			if(message!="") {
				alert(message);
				return false;
			} else {
				return true;
			}
		} catch(e) {
			alert(e);
		}
		return true;
	}
	,resizeIframe: function (h) {
		$("document").ready(function () {
			var theFrame=$("#iframe_modal",parent.document.body);
			if(theFrame) {
				var bdyheight=$("body").height();
				if(parseInt(h)>0) { bdyheight+=h; }
				theFrame.height(bdyheight);
			}
		});
	}
	,createDialog: function (url,param) {
		$("#addDialog").remove();
		var iframe=document.createElement("div");
		iframe.id="addDialog";
		iframe.innerHTML+="<div id='loading'><img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...</div>";
		iframe.innerHTML+='<iframe id="iframe_modal" allowtransparency="true" marginheight="0" marginwidth="0"  width="100%" frameborder="0" class="externalSite"  />';
		var ifrm=$("#iframe_modal",iframe).get(0);
		$(ifrm).css("height","100px").unbind('load');
		$(ifrm).load(function () { $("#loading",iframe).remove(); });
		ifrm.src=url;
		$(iframe).dialog(param);
	}
	,formatDateTxt: function (target) {
		$(".datefield",target).each(function () { if($.trim(this.value)!="") { this.value=jHelper.formatDate(jHelper.parseJSONDate(this.value)); } });
	}
	,checkValAttr: function (target) {
		$("select",target).each(function () { var v=$(this).attr("val");if(v!="") { this.value=v; } });
		$(":input:checkbox",target).each(function () { var v=$(this).attr("val");if(v.toLowerCase()=="true") { this.checked=true; } });
	}
	,applyDatePicker: function (target) {
		$(".datefield",target).each(function () { $(this).datepicker({ changeMonth: true,changeYear: true }); });
	}
	,resetFields: function (target) {
		$(":input:text",target).val("");
		$(":input:checkbox",target).each(function () { this.checked==false; });
		$("select",target).val(0);
	}
	,formatDollar: function (target) {
		$(".money",target).each(function () {
			var amt=parseFloat($(this).attr("val"));
			if(isNaN(amt)) { amt=parseFloat($(this).html()); }
			if(isNaN(amt)) { this.innerHTML=""; } else { this.innerHTML=jHelper.dollarAmount(amt.toString()); }
		});
	}
	,formatDateHtml: function (target) {
		$(".dispdate",target).each(function () {
			if($.trim(this.innerHTML)!="") {
				try {
					this.innerHTML=jHelper.formatDate(jHelper.parseJSONDate(this.innerHTML));
				} catch(e) { alert(e); }
			}
		});
	}
	,waterMark: function (target) {
		$(".wm",target).each(function () {
			var v=this.value;
			$(this).focus(function () { this.value=""; }).blur(function () { if($.trim(this.value)=="") { this.value=v; } });
		});
	}
}