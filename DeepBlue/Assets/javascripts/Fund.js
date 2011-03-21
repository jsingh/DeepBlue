var fund={
	rowIndex: 0,
	rateDetailHTML: '',
	rateTierFirstRow: null,
	pageInit: false,
	init: function () {
		$(".ddlist").each(function () {
			this.value=$(this).attr("val");
			if(this.id=="MultiplierTypeId")
				$(this).change();
		});
		$(".rate-detail .tblrateschedule").each(function () {
			$(this).flexigrid();
			$("tbody tr:first",this).each(function () {
				fund.applyDatePicker(this);
			});
		});
		$(".rate-detail").each(function () {
			var tbl=$(".tblrateschedule",this);
			$("tr:first",tbl).each(function () {
				$("td",this).each(function () {
					var txt=$(":input[inputname='StartDate']",this).get(0);
					if(txt) {
						fund.dateChecking(txt);
					}
				});
			});
		});

		var rateSchedule=$(".rate-schedules .rate-detail:first");
		var div=document.createElement("div");
		div.innerHTML=rateSchedule.html();
		$(div).hide();
		$("body").append(div);

		var tempTable=$(".tblrateschedule:first",div);
		$("tr",tempTable).each(function () {
			$("td",this).each(function () {
				$("#SpnStartDate",this).html("");
				$("#SpnEndDate",this).html("");
			});
		});
		$(":input",div).each(function () {
			if(this.type=="text")
				this.value="";
			if(this.type=="hidden")
				this.value="0";
		});
		fund.rateDetailHTML=div.innerHTML;
		fund.rateTierFirstRow=document.createElement("tr");
		var firstrow=$("tbody tr:first",tempTable);
		$("td",firstrow).each(function () {
			var td=document.createElement("td");
			td.innerHTML=this.innerHTML;
			firstrow.append(td);
		});
		$(div).remove();
		this.resizeIframe();
		this.pageInit=true;
	}
	,add: function () {
		var dt=new Date();
		var url="/Fund/New/?t="+dt.getTime();
		this.open(url);
	}
	,edit: function (id) {
		var dt=new Date();
		var url="/Fund/Edit/"+id+"?t="+dt.getTime();
		this.open(url);
	}
	,open: function (url) {
		$("#addFundDialog").remove();
		var iframe=document.createElement("div");
		iframe.id="addFundDialog";
		iframe.innerHTML+="<div id='loading'><img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...</div>";
		iframe.innerHTML+='<iframe id="iframe_modal" allowtransparency="true" marginheight="0" marginwidth="0"  width="100%" frameborder="0" class="externalSite"  />';
		var ifrm=$("#iframe_modal",iframe).get(0);
		$(ifrm).css("height","100px").unbind('load');
		$(ifrm).load(function () { $("#loading",iframe).remove(); });
		ifrm.src=url;
		$(iframe).dialog({
			title: "Fund",
			autoOpen: true,
			width: 850,
			modal: true,
			position: 'top',
			autoResize: true
		});
	}
	,resizeIframe: function () {
		$("document").ready(function () {
			var theFrame=$("#iframe_modal",parent.document.body);
			if(theFrame) {
				theFrame.height($("body").height()+10);
			}
		});
	}
	,onSubmit: function (formId) {
		var frm=document.getElementById(formId);
		var message='';
		$(".field-validation-error",frm).each(function () {
			if(this.innerHTML!='') {
				message+=this.innerHTML+"\n";
			}
		});
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
		var UnfundedAmount=parseFloat($("#UnfundedAmount",frm).val());
		var CommitmentAmount=parseFloat($("#CommitmentAmount",frm).val());
		if(isNaN(UnfundedAmount)) { UnfundedAmount=0; }
		if(isNaN(CommitmentAmount)) { CommitmentAmount=0; }
		if(CommitmentAmount>UnfundedAmount) { message+="Transaction Amount should be less than Unfunded Commitment Amount\n"; }
		if(message!="") {
			alert(message);
			return false;
		}
		return fund.onFundRateValidation();
	}
	,changeRS: function (ddl) {
		var tr=$(ddl).parents("tr:first");
		var tbl=tr.parent();
		var trPrev=tr.prev();
		var MultiplierTypeId=$("#MultiplierTypeId",tr).get(0);
		var rateobj=$("#Rate",tr).get(0);
		var flatFeeObj=$("#FlatFee",tr).get(0);
		var mtid=MultiplierTypeId.value;
		switch(MultiplierTypeId.value) {
			case "0":
				rateobj.disabled=true;
				flatFeeObj.disabled=true;
				break;
			case "1":
				rateobj.disabled=false;
				flatFeeObj.disabled=true;
				break;
			case "2":
				flatFeeObj.disabled=false;
				rateobj.disabled=true;
				break;
		}
		if(this.pageInit) {
			if(trPrev.get(0)) {
				if(parseInt(MultiplierTypeId.value)>0) {
					var index=$("tr",tbl).index(trPrev);
					var prevMultiplierTypeId=$("#MultiplierTypeId",trPrev).get(0);
					if(prevMultiplierTypeId.value=="0") {
						alert("Year "+(index+1)+" Fee Calculation Type is required");
						//ddl.value=0;
						prevMultiplierTypeId.focus();
						return false;
					}
				}
			}
		}
		return true;
	}
	,changeRate: function (txt) {
		var rate=parseInt(txt.value);
		if(rate>100) {
			txt.focus();
			alert("Rate must be under 100%");
			txt.value="";
			return false;
		}
	}
	,onFundRateValidation: function () {
		var result=true;
		$(".ddlist").each(function () {
			if(result) {
				if(this.id=="MultiplierTypeId") {
					result=fund.changeRS(this);
					if(result==false)
						return;
				}
			}
		});
		/*$(".rate-detail").each(function () {
		var InvestorTypeId=$("#InvestorTypeId",this).val();
		if(parseInt(InvestorTypeId)>0) {
		$(".tblrateschedule",this).each(function () {
		if(result)
		result=fund.checkRateTable(this);
		else
		return;
		});
		}
		});*/
		return result;
	}
	,checkRateTable: function (tbl) {
		var trfirst=$("tr:first",tbl);
		var message="";
		var txt=$(":input[inputname='StartDate']",tbl).get(0);
		var result=true;
		if(txt) {
			if(txt.value!="") {
				$("tr",tbl).each(function () {
					if(result==false) {
						return;
					}
					var MultiplierTypeId=$("#MultiplierTypeId",this).get(0);
					var Rate=$("#Rate",this).get(0);
					var FlatFee=$("#FlatFee",this).get(0);
					if(MultiplierTypeId&&Rate&&FlatFee) {
						var msg="";var focusinput;
						switch(MultiplierTypeId.value) {
							case "0": msg="Fee Calculation Type is required";focusinput=MultiplierTypeId;break;
							case "1": if(parseInt(Rate.value)>100) { msg="Rate must be under 100%"; } else if(parseInt(Rate.value)<=0) { msg="Rate is required"; } focusinput=Rate;break;
							case "2": if(parseInt(FlatFee.value)<=0) { msg="Flat Fee is required"; } focusinput=FlatFee;break;
						}
						if(msg!="") { alert(msg);if(focusinput) { focusinput.focus(); } result=false; }
					}
				});
			}
		}
		return result;
	}
	,onTaxIdAvailable: function (message) {
		if(message!='')
			alert(message);
	}
	,closeDialog: function (reload) {
		$("#addFundDialog").dialog('close');
		if(reload==true) {
			$("#FundList").flexReload();
		}
	}
	,onCreateFundBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	}
	,onCreateFundSuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(UpdateTargetId.html()!="")
			alert(UpdateTargetId.html());
		else
			parent.fund.closeDialog(true);
	}
	,applyDatePicker: function (tr) {
		$(":input",tr).each(function () {
			if($(this).attr("type")=="text") {
				var name=$(this).attr("name");
				if(name.indexOf("StartDate")> -1||name.indexOf("EndDate")> -1) {
					this.className="";
					this.id=fund.rowIndex;
					fund.rowIndex++;
					$(this).datepicker({ changeMonth: true,changeYear: true });
				}
			}
		});
	}
	,addRateSchedule: function () {
		var FundRateSchedulesCount=document.getElementById("FundRateSchedulesCount");
		FundRateSchedulesCount.value=parseInt(FundRateSchedulesCount.value)+1;
		var rateDetail=document.createElement("div");
		rateDetail.innerHTML=fund.rateDetailHTML;
		var newRateDetail=document.createElement("div");
		newRateDetail.className="rate-detail";
		newRateDetail.innerHTML=rateDetail.innerHTML.replace(/1_/g,(FundRateSchedulesCount.value)+"_");
		$(".rate-schedules").append(newRateDetail);
		$("#FundRateScheduleId",newRateDetail).val("0");
		$("#InvestorTypeId",newRateDetail).val("0");
		$("#IsDelete",newRateDetail).val("");
		$(newRateDetail).show();
		var tbl=$(".tblrateschedule",newRateDetail);
		$("#TiersCount",newRateDetail).val($("tr",tbl).length-1);
		$("tbody tr:first",tbl).each(function () {
			fund.applyDatePicker(this);
		});
		this.resizeIframe();
	}
	,addNewRow: function (addlink) {
		var tbl=$(addlink).parents("table:first");
		this.addRow(tbl);
		var tr=$("tr:first",tbl);
		var txt=$(":input[inputname='StartDate']",tr).get(0);
		this.dateChecking(txt);
		this.resizeIframe();
	}
	,changeInvestorType: function (invType) {
		if(invType.value!="0") {
			$(".investortype").each(function () {
				if($(this).attr("name")!=$(invType).attr("name")) {
					if(this.value==invType.value) {
						alert("Investor type already chosen");
						invType.value="0";
					}
				}
			});
		}
	}
	,deleteInvestorType: function (img) {
		if(confirm("Are you sure you want to delete this rate schedule?")) {
			var rateDetail=$(img).parents(".rate-detail:first");
			var FundRateScheduleId=$("#FundRateScheduleId",rateDetail).val();
			if(parseInt(FundRateScheduleId)>0) {
				$.get("/Fund/DeleteFundRateSchedule/?id="+FundRateScheduleId,function (data) {
					$(rateDetail).remove();
				});
			} else {
				$(rateDetail).remove();
			}
		}
	}
	,deleteTier: function (img) {
		if(confirm("Are you sure you want to delete this rate schedule tier?")) {
			var tr=$(img).parents("tr:first");
			var ManagementFeeRateScheduleId=parseInt($("#ManagementFeeRateScheduleId",tr).val());
			if(parseInt(ManagementFeeRateScheduleId)>0) {
				$.get("/Fund/DeleteFundRateSchedule/?id="+FundRateScheduleId,function (data) {
					$(tr).remove();
				});
			} else {
				$(tr).remove();
			}
		}
	}
	,dateChecking: function (txt) {
		try {
			if(txt.value!="") {
				var tr=$(txt).parents("tr:first");
				var tbl=$(tr).parents("table:first");
				var eDate=Date.DateAdd("yyyy",1,txt.value);
				var endDate=Date.DateAdd("d",-1,this.formatDate(eDate));
				$("#EndDate",tr).val(this.formatDate(endDate));
				$("#SpnEndDate",tr).html(this.formatDate(endDate));
				this.addAdditionalRow(tbl,9-$("tr",tbl).length);
				var index=0;
				$("tr",tbl).each(function () {
					if(index!=0) {
						var trPrev=$(this).prev();
						var ewDate=$("#EndDate",trPrev).val();
						fund.assignDates(this,ewDate);
					}
					index++;
				});
				this.resizeIframe();
			}
			return true;
		} catch(e) {
			//alert(e);
		}
	}
	,addAdditionalRow: function (tbl,count) {
		var i=0;
		for(i=0;i<count;i++) {
			fund.addRow(tbl);
		}
	}
	,addRow: function (tbl) {
		var rateDetail=tbl.parents(".rate-detail:first");
		var InvestorTypeId=$("#InvestorTypeId",rateDetail);
		var index=InvestorTypeId.attr("name").replace("_InvestorTypeId","");
		var TiersHidden=$("#TiersCount",rateDetail).get(0);
		var TiersCount=parseInt($("tr",tbl).length);
		var tr=document.createElement("tr");
		var trLast=$("tbody tr:eq(7)",tbl).get(0);
		if(trLast) {
			$("td",trLast).each(function () {
				var td=document.createElement("td");
				td.innerHTML=this.innerHTML.replace(/\$8\$/g,"$"+(TiersCount)+"$");
				var year="Year "+TiersCount;
				$("#SpnName",td).html(year);
				$("#Name",td).val(year);
				$("input",td).each(function () {
					if($(this).attr("name").indexOf("StartDate")>0) {
						var hdn=document.createElement("input");
						hdn.type="hidden";hdn.value=this.value;hdn.name=this.name;hdn.id="StartDate";
						var spn=document.createElement("span");
						spn.id="SpnStartDate";spn.innerHTML=this.value;
						$(this).remove();
						$("span",td).remove();
						$("div",td).append(hdn).append(spn);
					}
				});
				$("input",td).val("");
				$("#ManagementFeeRateScheduleTierId",td).val("0");
				$("#ManagementFeeRateScheduleId",td).val("0");
				$("select",td).val("0");
				$("textarea",td).val("");
				$(tr).append(td);
			});
			$(tbl).append(tr);
		}
		if(TiersHidden) {
			TiersHidden.value=TiersCount-1;
		}
	}
	,assignDates: function (tr,startDate) {
		var sDate=Date.DateAdd("d",1,startDate);
		var eDate=Date.DateAdd("yyyy",1,this.formatDate(sDate));
		var endDate=Date.DateAdd("d",-1,this.formatDate(eDate));
		$("#StartDate",tr).val(this.formatDate(sDate));
		$("#SpnStartDate",tr).html(this.formatDate(sDate));
		$("#EndDate",tr).val(this.formatDate(endDate));
		$("#SpnEndDate",tr).html(this.formatDate(endDate));
	}
	,formatDate: function (dateobj) {
		return $.datepicker.formatDate('mm/dd/yy',dateobj);
	}
}
