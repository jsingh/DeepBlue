var capitalCallReceive={
	pageInit: false,
	init: function () {
		$(document).ready(function () {
			var capitalCallId=$("#CapitalCall").val();
			var fundId=$("#FundId").val();
			if(parseInt(capitalCallId)>0) {
				capitalCallReceive.selectFund(fundId);
			} else {
				setTimeout(function () {
					$("#CaptialCallDetail").hide();
				},200);
			}
			capitalCallReceive.pageInit=true;
		});
	}
	,selectFund: function (id) {
		$("#SpnLoading").show();
		var capitalCallId=0;
		if(this.pageInit) {
			$("#CapitalCall").val(0);
		} else {
			capitalCallId=$("#CapitalCall").val();
		}
		var dt=new Date();
		var url="/CapitalCall/GetCapitalCallList?fundId="+id+"&t="+dt.getTime();
		var ddl=document.getElementById("CapitalCallId");
		ddl.options.length=null;
		var listItem=new Option("Loading...","",false,false);
		ddl.options[0]=listItem;
		$.getJSON(url,function (data) {
			ddl.options.length=null;
			listItem=new Option("--Select One--","0",false,false);
			ddl.options[0]=listItem;
			for(i=0;i<data.length;i++) {
				listItem=new Option(data[i].Text,data[i].Value,false,false);
				if(capitalCallId==data[i].Value) {
					listItem.selected=true;
				}
				ddl.options[ddl.options.length]=listItem;
			}
		});
		$("#SpnLoading").hide();
		this.selectCapitalCall(capitalCallId);
	}
	,selectCapitalCall: function (id) {
		$("#SpnLoading").show();
		var dt=new Date();
		var url="/CapitalCall/FindCapitalCall?id="+id+"&t="+dt.getTime();
		$("#CaptialCallDetail").html("");
		$("#ItemCount").val(0);
		$(":input[type='text']").val("");
		$(":input[type='hidden'][name!='FundId']").val("0");
		$("#CapitalCall").val(id);
		if(parseInt(id)>0) {
			$.getJSON(url,function (data) {
				$("#SpnLoading").hide();
				$("#CCLItemTemplate").tmpl(data).appendTo("#CaptialCallDetail");
				$("#CapitalAmountCalled").val(data.CapitalAmountCalled);
				$("#CapitalCallDate").val(capitalCallReceive.formatDate(data.CapitalCallDate));
				$("#CapitalCallDueDate").val(capitalCallReceive.formatDate(data.CapitalCallDueDate));
				$("#CapitalCallDate").datepicker({ changeMonth: true,changeYear: true });
				$("#CapitalCallDueDate").datepicker({ changeMonth: true,changeYear: true });
				$("#FundName").val(data.FundName);
				var i;
				if(data.Items) {
					$("#ItemCount").val(data.Items.length);
					var table=$("table","#CapitalCallItems").flexigrid({ height: 0,useBoxStyle: false });
					$("tr",table).each(function () {
						var received=$("#Received[type='checkbox']",this).get(0);
						var receiveDate=$("td:last input",this).get(0);
						if(receiveDate) {
							$(receiveDate).datepicker({ changeMonth: true,changeYear: true });
						}
						if(received) {
							if($(received).attr("checkvalue")=="true") { received.checked=true; }
							receiveDate.disabled=!received.checked;
						}
					});
				}
			});
		} else {
			$("#SpnLoading").hide();
		}
	}
	,formatDate: function (dateVal) {
		var dt=new Date(parseInt(dateVal.substr(6)));
		if(dt.getFullYear()=="1900")
			return "";
		else
			return $.datepicker.formatDate('mm/dd/yy',dt);
	}
	,selectReceive: function (chk) {
		var tr=$(chk).parents("tr:first");
		var txt=$(":input[rdate='true']",tr).get(0);
		if(txt) {
			txt.disabled=!chk.checked;
		}
	}
	,onCreateCapitalCallBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	}
	,onCreateCapitalCallSuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(jQuery.trim(UpdateTargetId.html())!="") {
			jAlert(UpdateTargetId.html())
		} else {
			location.href="/CapitalCall/List";
		}
	}
	,onSubmit: function (formId) {
		var frm=document.getElementById(formId);
		var message='';
		Sys.Mvc.FormContext.getValidationForForm(frm).validate('submit');
		$(".field-validation-error",frm).each(function () {
			if(this.innerHTML!='') {
				message+=this.innerHTML+"\n";
			}
		});
		if(message!="") {
			jAlert(message);
			return false;
		} else {
			return true;
		}
		return true;
	}
}
