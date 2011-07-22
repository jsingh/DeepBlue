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
	,trimTextArea: function (target) {
		$("textarea",target).each(function () { this.value=$.trim(this.value); });
	}
	,parseJSONDate: function (date) {
		try { return eval('new'+date.toString().replace(/\//g,' ')); } catch(e) { return date; }
	}
	,dollarAmount: function (Num) {
		var t=document.createElement("input");t.type="text";
		$(t).val(Num).formatCurrency();
		return $(t).val();
	}
	,numberFormat: function (Num) {
		var t=document.createElement("input");t.type="text";
		$(t).val(Num).formatCurrency({ "roundToDecimalPlace": 0 });
		return $(t).val().replace("$","");
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
		if(ddl) {
			ddl.options.length=null;
			if(data!=null) {
				for(i=0;i<data.length;i++) {
					listItem=new Option(data[i].Text,data[i].Value,false,false);
					ddl.options[ddl.options.length]=listItem;
				}
			}
		}
	}
	,formSubmit: function (formId,checkAutoComplete) {
		try {
			var frm=document.getElementById(formId);
			var message='';
			if(isNaN(checkAutoComplete)) { checkAutoComplete=true; }
			if(checkAutoComplete) { $(".field-validation-error",frm).each(function () { if(this.innerHTML!='') { message+=this.innerHTML+"\n"; } }); }
			if(message!="") { alert(message);return false; }
			Sys.Mvc.FormContext.getValidationForForm(frm).validate('submit');
			$(".field-validation-error",frm).each(function () { if(this.innerHTML!='') { message+=this.innerHTML+"\n"; } });
			if(message!="") { alert(message);return false; } else { return true; }
		} catch(e) { alert(e); }
		return true;
	}
	,resizeIframe: function (h) {
		$("document").ready(function () {
			var theFrame=$("#iframe_modal",parent.document.body);
			if(theFrame) {
				var body=$(theFrame).contents().find('body');
				var bdyheight=body.height();
				if(parseInt(h)>0) { bdyheight+=h; }
				theFrame.height(bdyheight);
			}
		});
	}
	,createDialog: function (url,param) {
		var iframe=document.getElementById("addDialog");
		if(iframe) {
			$("#loading",iframe).show();
			var ifrm=$("#iframe_modal",iframe).empty().get(0);
			$(ifrm).contents().find('body').empty();
			ifrm.src=url;
			$(iframe).dialog('open');
		} else {
			iframe=document.createElement("div");
			iframe.id="addDialog";
			iframe.innerHTML+="<div id='loading'><img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...</div>";
			iframe.innerHTML+='<iframe id="iframe_modal" allowtransparency="true" marginheight="0" marginwidth="0" scrolling="no" width="100%" frameborder="0" class="externalSite" />';
			var ifrm=$("#iframe_modal",iframe).get(0);
			$(ifrm).css("height","100px").unbind('load');
			$("body").append(iframe);
			$(iframe).dialog(param);
			ifrm.src=url;
			$(ifrm).load(function () {
				$("#loading",iframe).hide();
			});
		}
	}
	,formatDateTxt: function (target) {
		$(".datefield",target).each(function () { if($.trim(this.value)!="") { var dt=jHelper.formatDate(jHelper.parseJSONDate(this.value));if(dt.toString()=="01/01/1") { this.value=""; } else { this.value=dt; } } });
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
		$(":input:[type='file']",target).val("");
		$(":input[type='hidden']",target).val("");
		$(":input:checkbox",target).each(function () { this.checked==false; });
		$("select",target).val(0);
	}
	,formatDollar: function (target) {
		$(".money",target).each(function () {
			var amt=parseFloat($(this).attr("val"));
			if(isNaN(parseFloat(amt))) { amt=$.trim($(this).html()); }
			if(isNaN(parseFloat(amt))) { amt=0; }
			if(amt==0) { this.innerHTML=""; }
			else {
				this.innerHTML=jHelper.dollarAmount(amt.toString());
			}
		});
	}
	,formatDateHtml: function (target) {
		$(".dispdate",target).each(function () {
			if($.trim(this.innerHTML)!="") {
				try {
					var dt=jHelper.formatDate(jHelper.parseJSONDate(this.innerHTML));
					if(dt.toString()=="01/01/1") {
						this.innerHTML="";
					} else {
						this.innerHTML=dt;
					}
				} catch(e) { alert(e); }
			}
		});
	}
	,waterMark: function (target) {
		$(".wm",target).each(function () {
			var v=this.value;
			$(this).focus(function () { this.value="";$(this).unbind('focus'); });
			//.blur(function () { if($.trim(this.value)=="") { this.value=v; } });
		});
	}
	,ajImg: function () { return "<img src='/Assets/images/ajax.jpg'/>&nbsp;"; }
	,loading: function (t,v) { if(isNaN(v)) { v="Loading"; } $(t).html(this.ajImg()+v+"..."); }
	,setUpToolTip: function (target) {
		$(".tooltiptxt",target).each(function () { jHelper.tooltip(this); });
	}
	,tooltip: function (target) {
		$(target).unbind('mouseover')
		.mouseover(function () {
			$(".tooltip").remove();
			var t=document.createElement("div");
			var mtop=0;//parseInt($(target).attr("top"));
			if(isNaN(mtop)) { mtop=0; }
			t.className="tooltip";
			$(target).after(t);
			setTimeout(function () {
				var v=$(target).val();
				if($.trim(v)=="") {
					$(t).hide();
				} else {
					$(t).html(v);
					var p=$(target).offset();
					$(t).css({ "margin-top": "-49px","left": p.left+50 })
					$(t).fadeIn('slow');
				}
			},100);
		})
		.mouseout(function () {
			$(".tooltip").fadeOut('slow');
		});
	}
	,applyGridClass: function (target) {
		$("tr:even",target).removeClass("row").removeClass("arow").addClass("row");
		$("tr:odd",target).removeClass("row").removeClass("arow").addClass("arow");
	}
}
$.extend(window,{
	formatDate: function (dt) { if(dt==null) { return ""; } var d=jHelper.formatDate(jHelper.parseJSONDate(dt));if(d=="01/01/1"||d=="01/01/1900") { return ""; } else { return d; } }
	,formatCurrency: function (d) { if(d==null) { d=0; } if(isNaN(d)) { d=0; } return jHelper.dollarAmount(d.toString()); }
	,formatPercentage: function (d) { if(d==null) { d=0; } if(isNaN(d)) { d=0; } if(d==0) { return ""; } else { return d; } }
	,checkNullOrZero: function (d) { if(d==null) { d=0; } if(isNaN(d)) { d=0; } if(d==0) { return ""; } else { return d; } }
});