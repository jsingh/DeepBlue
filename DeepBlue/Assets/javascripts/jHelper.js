﻿var jHelper={
	isNumeric: function (evt,txt) {
		evt=(evt)?evt:window.event
		var charCode=(evt.which)?evt.which:evt.keyCode;
		if((charCode>=48&&charCode<=57)||(charCode>=96&&charCode<=105)
			||(charCode==8)
			||(charCode==37)
			||(charCode==39)
			||(charCode==46)
			||(charCode==5)
			||(charCode==9)
			||(charCode==188)
			) {
			return true;
		}
		return false;
	}
	,isCurrency: function (evt) {
		evt=(evt)?evt:window.event
		var charCode=(evt.which)?evt.which:evt.keyCode;
		if((charCode>=48&&charCode<=57)||(charCode>=96&&charCode<=105)
			||(charCode==8)
			||(charCode==37)
			||(charCode==39)
			||(charCode==46)
			||(charCode==190)
			||(charCode==110)
			||(charCode==5)
			||(charCode==9)
			||(charCode==188)
			) {
			return true;
		}
		return false;
	}
	,cFloat: function (value) {
		try {
			if($.trim(value)=="") {
				return 0;
			}
			if(value==undefined) {
				return 0;
			}
			var retvalue=value;
			retvalue=retvalue.replace(/\$/g,'');
			retvalue=retvalue.replace(/\%/g,'');
			retvalue=retvalue.replace(/\,/g,'');
			var v=parseFloat(retvalue);
			if(isNaN(v)==false) {
				return v;
			} else {
				return 0;
			}
		} catch(e) {
			//alert(e);
			return value;
		}
	}
	,checkEmail: function (txt) {
		if($.trim(txt.value)!="") {
			var rExp=new RegExp("^[\\w-_\.]*[\\w-_\.]\@[\\w]\.+[\\w]+[\\w]$");
			if(rExp.test(txt.value)==false) { jAlert("Invalid Email");txt.value=""; }
		}
	}
	,checkWebAddress: function (txt) {
		if($.trim(txt.value)!="") {
			if(txt.value.toLowerCase().indexOf("http://")<0&&txt.value.toLowerCase().indexOf("https://")<0) { txt.value="http://"+txt.value; }
			//var rExp=new RegExp("^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$");
			//if(rExp.test(txt.value)==false) { jAlert("Invalid Web Address");txt.value=""; }
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
		try { return $.datepicker.formatDate('mm/dd/yy',dateobj); } catch(e) { return ""; }
	}
	,trimTextArea: function (target) {
		$("textarea",target).each(function () { this.value=$.trim(this.value); });
	}
	,parseJSONDate: function (date) {
		try { return eval('new'+date.toString().replace(/\//g,' ')); } catch(e) { return date; }
	}
	,dollarAmount: function (Num) {
		try {
			var t=document.createElement("input");t.type="text";
			$(t).val(Num).formatCurrency();
			var v=$(t).val();
			if(v.toString()=="null")
				return "";
			else
				return v;
		}
		catch(e) {
			return "";
		}
	}
	,numberFormat: function (Num) {
		try {
			var t=document.createElement("input");t.type="text";
			$(t).val(Num).formatCurrency({ "roundToDecimalPlace": 0 });
			return $(t).val().replace("$","");
		}
		catch(e) {
			return "";
		}
	}
	,serialize: function (target) {
		var params=[];
		$(":input",target).each(function () {
			var type=$(this).attr("type");
			switch(type) {
				case "checkbox": if(this.checked) { params[params.length]=jHelper.getParam(this); } break;
				case "radio": if(this.checked) { params[params.length]=jHelper.getParam(this); } break;
				default: params[params.length]=jHelper.getParam(this);break;
			}
		});
		params=jHelper.checkParams(target,params);
		return params;
	}
	,checkParams: function (target,params) {
		$.each(params,function (i,par) {
			var element=$(":input[name='"+par.name+"'][onkeydown]",target).get(0);
			if(element) {
				var value=par.value;
				value=value.replace(/\$/g,'');
				value=value.replace(/\%/g,'');
				value=value.replace(/\,/g,'');
				par.value=value;
			}
		});
		return params;
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
			if(message!="") { jAlert(message);return false; }
			Sys.Mvc.FormContext.getValidationForForm(frm).validate('submit');
			$(".field-validation-error",frm).each(function () { if(this.innerHTML!='') { message+=this.innerHTML+"\n"; } });
			if(message!="") { jAlert(message);return false; } else { return true; }
		} catch(e) { jAlert(e); }
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
			iframe.innerHTML+="<div id='loading'>"+jHelper.loadingHTML()+"</div>";
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
		$("select",target).each(function () {
			var v=$(this).attr("val");
			if(v==undefined) { v=""; }
			if(v!="") { this.value=v; }
		});
		$(":input:checkbox",target).each(function () {
			var v=$(this).attr("val");
			if(v==undefined) { v=""; }
			if(v.toLowerCase()=="true") { this.checked=true; }
		});
	}
	,applyDatePicker: function (target) {
		$(".datefield",target).each(function () { $(this).datepicker({ changeMonth: true,changeYear: true }); });
	}
	,resetFields: function (target) {
		$(":input:text",target).val("");
		$(":input:[type='file']",target).val("");
		$(":input[type='hidden']",target).val("");
		$(":input:checkbox",target).each(function () { this.checked=false; });
		$("select option:selected",target).each(function () {
			this.selected=false;
		});
		$(".jqComboBox .ui-autocomplete-input",target).each(function () {
			var sel=$("#"+$(this).attr("selectid"),target);
			var opt=$("option:eq(0)",sel);
			this.value=opt.text();
		});
	}
	,formatDollar: function (target) {
		$(".money",target).each(function () {
			var va=$.trim($(this).attr("val"));
			if(va=="") {
				va=$.trim($(this).html());
			}
			var amt=jHelper.cFloat(va);
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
				} catch(e) { jAlert(e); }
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

	,setUpToolTip: function (target) {
	}
	,tooltip: function (target) {
	}
	,applyGridClass: function (target) {
		$("tr:even",target).removeClass("row").removeClass("arow").addClass("row");
		$("tr:odd",target).removeClass("row").removeClass("arow").addClass("arow");
	}
	,applyFlexGridClass: function (target) {
		$("tr:even",target).removeClass("erow").addClass("erow");
		$("tr:odd",target).removeClass("erow").addClass("grow");
	}
	,applyFlexEditGridClass: function (target) {
		$("tr.disprow:even",target).removeClass("erow");
		$("tr.disprow:odd",target).removeClass("erow").addClass("erow");
	}
	,expandExpMenu: function (that) {
		var pos=$(that).offset();
		$(".exportlist").css({ "z-index": 9999,"top": pos.top+13,"left": pos.left }).show();
	}
	,chooseExpMenu: function (id,name) {
		$("#ExportId").val(id);
		$("#lnkExportName").html(name);
		$(".exportlist").hide();
	}
	,checkAmtTxt: function (target) {
		$(":input[onkeydown]",target).each(function () {
			var attr=$(this).attr("onkeydown");
			if(attr.indexOf("jHelper.isCurrency")>0) {
				if($.trim(this.value)!="") {
					var number=$(this).val();
					number=$.formatNumber(number,{ format: "#,###",locale: "us" });
					$(this).val(number);
				}
			}
		});
	}
	,gridEditRow: function (target) {
		$("td",target)
		.each(function () {
			var td=$(this);
			td.css("cursor","pointer");
			var img=$(".editbtn",td).get(0);
			if(!img) {
				td.click(function () {
					var l=$(":input:visible",this).length;
					if(l<=0) {
						var tr=$(this).parents("tr:first");
						$(".editbtn",tr).click();
					}
				});
			}
		});
	}
	,jqTransCheckBox: function (target) { try { $(":checkbox",target).each(function () { if(this.style.display!="none") { $(this).jqTransCheckBox(); } }); } catch(e) { } }
	,getImagePath: function (imageName) {
		return deepBlue.rootUrl+"/Assets/images/"+imageName;
	}
	,imageHTML: function (imageName) {
		return "<img src='"+deepBlue.rootUrl+"/Assets/images/"+imageName+"' />";
	}
	,ajImg: function () { return this.imageHTML("ajax.jpg")+"&nbsp;"; }
	,loading: function (t,v) { if(isNaN(v)) { v="Loading"; } $(t).html(this.ajImg()+v+"..."); }
	,loadingHTML: function () { return this.ajImg()+"Loading..."; }
	,savingHTML: function () { return this.ajImg()+"Saving..."; }
	,deleteHTML: function () { return this.ajImg()+"Delete..."; }
	,uploadingHTML: function () { return this.ajImg()+"Upload..."; }
	,calculatingHTML: function () { return this.ajImg()+"Upload..."; }
	,jqCheckBox: function (target) { 
		$(":input:checkbox",target).jqCheckBox(); 
	}
	,jqComboBox: function (target) { $("select",target).combobox(); }
	,removejqComboBox: function (target) { $("select",target).combobox("destroy"); }
	,removejqCheckBox: function (target) { $(".jqCheckBox",target).remove();$(".jqCDisplay",target).remove();$(".jqHidden",target).removeClass(); }
	,resizeDialog: function () {
		$(".ui-dialog-content:visible").each(function () {
			var dialog=$(this).data("dialog");
			dialog.option("position",dialog.options.position);
		});
	}
}
$(window).resize(function () {
	jHelper.resizeDialog();
});
$.extend(window,{
	formatDate: function (dt) { try { if(dt==null) { return ""; } var d=jHelper.formatDate(jHelper.parseJSONDate(dt));if(d=="01/01/1"||d=="01/01/1900") { return ""; } else { return d; } } catch(e) { return ""; } }
	,formatCurrency: function (d) {
		if(d==null) { d=0; }
		if(isNaN(d)) { d=0; }
		return jHelper.dollarAmount(d.toString());
	}
	,formatPercentage: function (d) {
		var s=$.formatNumber(d,{ format: "#.0%" });
		if(s==".0%") { s=""; }
		return s;
	}
	,formatNumber: function (d,f) {
		var format="#,###.00";
		if(f==0) { format="#,###"; }
		var n=$.formatNumber(d,{ format: format,locale: "us" });
		if(n==".00") { n=""; }
		if($.trim(n)!="") {
			var l3=n.substr(n.length-3,n.length);
			if(l3==".00") {
				n=n.substr(0,n.length-3);
			}
		}
		return n;
	}
	,checkNullOrZero: function (d) { if(d==null) { d=0; } if(isNaN(d)) { d=0; } if(d==0) { return ""; } else { return d; } }
	,formatEditor: function (v) { if(!v) v="";if($.trim(v)!="") return v.replace(/\n/g,"<br/>");else return ""; }
	,getCustomFieldValue: function (values,customFieldId,dataTypeId) {
		var i=0;var value="";
		if(values!=null) {
			for(i=0;i<values.length;i++) {
				if(values[i].CustomFieldId==customFieldId&&values[i].DataTypeId==dataTypeId) {
					switch(dataTypeId.toString()) {
						case "1": value=checkNullOrZero(values[i].IntegerValue);break;
						case "2": value=values[i].TextValue;break;
						case "3": value=values[i].BooleanValue;break;
						case "4": value=values[i].DateValue;break;
						case "5": value=checkNullOrZero(values[i].CurrencyValue);break;
					}
				}
			}
		}
		return value;
	}
});
