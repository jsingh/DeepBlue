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
		$("#FundId").val(id);
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
		var tbl=document.getElementById("InvestorDetail");
		$("tr:not(:first)",tbl).remove();
		$("tr:first",tbl).get(0).style.display="none";
		$("#ItemCount").val(0);
		$(":input[type='text']").val("");
		$(":input[type='hidden']").val("0");
		$("#CapitalCall").val(id);
		if(parseInt(id)>0) {
			$.getJSON(url,function (data) {
				$("#SpnLoading").hide();
				$("#CapitalAmountCalled").val(data.CapitalAmountCalled);
				$("#CapitalCallDate").val(capitalCallReceive.formatDate(data.CapitalCallDate));
				$("#CapitalCallDueDate").val(capitalCallReceive.formatDate(data.CapitalCallDueDate));
				$("#FundName").val(data.FundName);
				var i;
				$("tr:first",tbl).get(0).style.display="";
				if(data.Items) {
					$("#ItemCount").val(data.Items.length);
					for(i=0;i<data.Items.length;i++) {
						var tr=capitalCallReceive.addRow(tbl,i);
						$("#CapitalCallLineItemId",tr).val(data.Items[i].CapitalCallLineItemId);
						$("#InvestorName",tr).html(data.Items[i].InvestorName);
						$("#CapitalAmountCalled",tr).val(data.Items[i].CapitalAmountCalled);
						$("#ManagementFees",tr).val(data.Items[i].ManagementFees);
						$("#InvestmentAmount",tr).val(data.Items[i].InvestmentAmount);
						$("#ManagementFeeInterest",tr).val(data.Items[i].ManagementFeeInterest);
						$("#InvestedAmountInterest",tr).val(data.Items[i].InvestedAmountInterest);
						var received=$("#Received",tr);
						$(":input[type='hidden'][name='"+received.attr("name")+"']").val("false");
						received.attr("value","true").get(0).checked=data.Items[i].Received;
						var receiveDate=$(":input[rdate='true']",tr);
						if(data.Items[i].Received) {
							receiveDate.get(0).disabled=false;
						}else{
							receiveDate.get(0).disabled=true;
						}
						receiveDate.attr("id",i).attr("class","");
						receiveDate.datepicker({ changeMonth: true,changeYear: true });
						receiveDate.val(capitalCallReceive.formatDate(data.Items[i].ReceivedDate));
					}
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
	,addRow: function (tbl,index) {
		if(index==0) {
			return $("tr:first",tbl).get(0);
		} else {
			var tr=document.createElement("tr");
			var firstRow=$("tr:first",tbl);
			$("td",firstRow).each(function () {
				var td=document.createElement("td");
				td.innerHTML=this.innerHTML.replace(/1_/g,(index+1)+"_");
				$(":input",td).val("");
				$(":input[type='hidden']",td).val("0");
				$(tr).append(td);
			});
			if((index+1)%2==0)
				tr.className="erow";
			$(tbl).append(tr);
			return tr;
		}
	}
	,selectReceive: function (chk) {
		var tr=$(chk).parents("tr:first");
		var txt=$(":input[rdate='true']",tr).get(0);
		if(txt)
			txt.disabled=!chk.checked;
	}
	,onCreateCapitalCallBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	}
	,onCreateCapitalCallSuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(jQuery.trim(UpdateTargetId.html())!="") {
			alert(UpdateTargetId.html())
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
			alert(message);
			return false;
		} else {
			return true;
		}
		return true;
	}
}
