var accountingEntryTemplate={
	rowIndex: 0
	,index: -1
	,formIndex: 0
	,rateDetailHTML: ''
	,rateTierFirstRow: null
	,pageInit: false
	,newAccountingEntryTemplateData: null
	,init: function() {
		jHelper.waterMark($("body"));
	}
	,setup: function(target) {
		setTimeout(function() {
			jHelper.checkValAttr(target);
			jHelper.jqComboBox(target);
		});
	}
	,selectTab: function(that,detailid) {
		$(".section-tab").removeClass("section-tab-sel");
		$(".section-det").hide();
		$(that).addClass("section-tab-sel");
		$("#"+detailid).show();
	}
	,expand: function() {
		$(".headerbox").click(function() {
			//$(".headerbox").show();
			//$(".expandheader").hide();
			//$(".detail").hide();
			$(this).hide();
			var parent=$(this).parent();
			$(".expandheader",parent).show();
			var detail=$(".detail",parent);
			var display=detail.attr("issearch");
			detail.show();
		});
		$(".expandheader").click(function() {
			var expandheader=$(this);
			var parent=$(expandheader).parent();
			expandheader.hide();
			var detail=$(".detail",parent);
			detail.hide();
			$(".headerbox",parent).show();
		});
	}
	,loadTemplate: function(data,target) {
		target.empty();
		$("#AccountingEntryTemplateAddTemplate").tmpl(data).appendTo(target);
		accountingEntryTemplate.expand();
		accountingEntryTemplate.setup(target);
		jHelper.checkValAttr(target);
		jHelper.jqCheckBox(target);
		$("#percentage",target).hide();
		if(data.AccountingEntryAmountTypeID=="3") {
			$("#percentage",target).show();
		}
		$("#FundName",target).autocomplete({ source: "/Fund/FindFunds"
			,minLength: 1
			,select: function(event,ui) {
				$("#FundID",target).val(ui.item.id);
			},appendTo: "body",delay: 300
		});
		$("#VirtualAccountName",target).autocomplete({ source: "/Accounting/FindVirtualAccounts"
			,minLength: 1
			,select: function(event,ui) {
				$("#VirtualAccountID",target).val(ui.item.id);
			},appendTo: "body",delay: 300
		});
		$("#AccountingTransactionTypeName",target).autocomplete({ source: "/Accounting/FindAccountingTransactionTypes"
			,minLength: 1
			,select: function(event,ui) {
				$("#AccountingTransactionTypeID",target).val(ui.item.id);
			},appendTo: "body",delay: 300
		});
		$("#AccountingEntryAmountTypeName",target).autocomplete({ source: "/Accounting/FindAccountingEntryAmountTypes"
			,minLength: 1
			,select: function(event,ui) {
				$("#AccountingEntryAmountTypeID",target).val(ui.item.id);
				$("#percentage",target).hide();
				if(ui.item.id==3) {
					$("#percentage",target).show();
				}
			},appendTo: "body",delay: 300
		});
	}
	,deleteAccountingEntryTemplate: function(img,id) {
		if(confirm("Are you sure you want to delete this virtual account?")) {
			img.src=jHelper.getImagePath("ajax.jpg");
			$.get("/Accounting/DeleteAccountingEntryTemplate/"+id,function(data) {
				if(data!="") {
					jAlert(data);
				} else {
					$("#AccountingEntryTemplateList").flexReload();
				}
			});
		}
	}
	,deleteTab: function(id,isConfirm) {
		var isRemove=true;
		if(isConfirm) {
			isRemove=confirm("Are you sure you want to remove this accountingEntryTemplate?");
		}
		if(isRemove) {
			$("#Tab"+id).remove();
			$("#Edit"+id).remove();
			$("#tabdel"+id).remove();
			$("#TabAccountingEntryTemplateGrid").click();
		}
	}
	,edit: function(id,accountingEntryTemplateName) {
		var tbl=$("#AccountingEntryTemplateList");
		$("#EditRow"+id,tbl).remove();
		$("#lnkAddAccountingEntryTemplate").removeClass("green-btn-sel");
		var row;
		if(id==0) {
			row=$("tbody tr:first",tbl);
			$("#lnkAddAccountingEntryTemplate").addClass("green-btn-sel");
		} else {
			row=$("#Row"+id,tbl);
		}
		var editRow=document.createElement("tr");
		$(editRow).html("<td colspan=7 class='editcell'></td>").attr("id","EditRow"+id);
		if($(row).get(0)) {
			if(id==0) { $(row).before(editRow); } else { $(row).after(editRow); }
		} else {
			$("tbody",tbl).append(editRow);
		}
		var target=$("td",editRow);
		accountingEntryTemplate.open(id,target);
	}
	,open: function(id,target) {
		var dt=new Date();
		var url="/Accounting/FindAccountingEntryTemplate/"+id+"?t="+dt.getTime();
		target.html("<center>"+jHelper.loadingHTML()+"</center>");
		$.getJSON(url,function(data) {
			accountingEntryTemplate.loadTemplate(data,target);
		});
	}
	,save: function(imgbtn) {
		var frm=$(imgbtn).parents("form:first");
		var loading=$("#UpdateLoading",frm);
		loading.html(jHelper.savingHTML());
		if($("#AccountingEntryAmountTypeID",frm).val()=="3") {
			var $txt=$("#AccountingEntryAmountTypeData",frm);
			if($.trim($txt.val())!="") {
				var percentage=parseFloat($.trim($txt.val()).replace("%",""));
				if(percentage==undefined) { percentage=0; }
				if(percentage==null) { percentage=0; }
				if(percentage>0) {
					$txt.val(percentage+"%");
				} else {
					$txt.val("");
				}
			}
		}
		var accountingEntryTemplateId=parseInt($("#AccountingEntryTemplateId",frm).val());
		$.post("/Accounting/CreateAccountingEntryTemplate",$(frm).serializeForm(),function(data) {
			loading.empty();
			if($.trim(data)!="") {
				jAlert(data);
			} else {
				jAlert("Template Saved");
				$("#lnkAddAccountingEntryTemplate").removeClass("green-btn-sel");
				$("#AccountingEntryTemplateList").flexReload();
			}
		});
	}
	,cancel: function(id) {
		$("#EditRow"+id).remove();
		$("#lnkAddAccountingEntryTemplate").removeClass("green-btn-sel");
	}
	,changeRS: function(ddl) {
		this.checkChange(ddl);
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
						jAlert("Year "+(index+1)+" Fee Calculation Type is required");
						//ddl.value=0;
						prevMultiplierTypeId.focus();
						return false;
					}
				}
			}
		}
		return true;
	}
	,changeRate: function(txt) {
		var rate=parseInt(txt.value);
		if(rate>100) {
			txt.focus();
			jAlert("Rate must be under 100%");
			txt.value="";
			return false;
		}
	}
	,applyDatePicker: function(txt) {
		accountingEntryTemplate.index=accountingEntryTemplate.index+1;
		txt.id="dp_"+accountingEntryTemplate.index;
		$(txt).datepicker({ changeMonth: true,changeYear: true });
	}
	,addRateSchedule: function(that) {
		var frm=$(that).parents("form:first");
		var rateSchdules=$("#RateSchdules",frm);
		var cnt=parseInt($("#AccountingEntryTemplateRateSchedulesCount").val());
		var data={ index: cnt,FRS: accountingEntryTemplate.newAccountingEntryTemplateData.AccountingEntryTemplateRateSchedules[0] };
		$("#AccountingEntryTemplateRateSchduleTemplate").tmpl(data).appendTo(rateSchdules);
		var newRateDetail=$(".rate-detail:last",frm);
		cnt=cnt+1;
		$("#AccountingEntryTemplateRateSchedulesCount").val(cnt);
		$("#AddNewIVType",frm).hide();
		accountingEntryTemplate.setup(newRateDetail);
	}
	,addNewRow: function(addlink) {
		var tbl=$(addlink).parents("table:first");
		var grid=$(tbl).parent();
		var index=$("#ScheduleIndex",grid).val();
		var lastrow=$("tbody tr:last",tbl);
		var cnt=$("tbody tr",tbl).length;
		var data={ index: index,rowIndex: (cnt+1),tier: accountingEntryTemplate.newAccountingEntryTemplateData.AccountingEntryTemplateRateSchedules[0].AccountingEntryTemplateRateScheduleTiers[0] };
		$("#AccountingEntryTemplateRateSchduleTierTemplate").tmpl(data).insertAfter(lastrow);
		$("#TiersCount",grid).val(cnt);
		var newRow=$("tbody tr:last",tbl);
		jHelper.jqComboBox(newRow);
		var txt=$(".datefield",tbl).get(0);
		if(txt) {
			accountingEntryTemplate.dateChecking(txt);
			accountingEntryTemplate.applyDatePicker(txt);
		}
	}
	,changeInvestorType: function(invType) {
		var frm=$(invType).parents("form:first");
		if(invType.value!="0") {
			$(".investortype",frm).each(function() {
				if($(this).attr("name")!=$(invType).attr("name")) {
					if(this.value==invType.value) {
						jAlert("Investor type already chosen");
						invType.value="0";
					}
				}
			});
		}
	}
	,deleteInvestorType: function(img) {
		if(confirm("Are you sure you want to delete this rate schedule?")) {
			var rateDetail=$(img).parents(".rate-detail:first");
			var frm=$(rateDetail).parents("form:first");
			var AccountingEntryTemplateRateScheduleId=$("#AccountingEntryTemplateRateScheduleId",rateDetail).val();
			if(parseInt(AccountingEntryTemplateRateScheduleId)>0) {
				$.get("/AccountingEntryTemplate/DeleteAccountingEntryTemplateRateSchedule/?id="+AccountingEntryTemplateRateScheduleId,function(data) {
					$(rateDetail).remove();
					accountingEntryTemplate.checkIVType(frm);
				});
			} else {
				$(rateDetail).remove();
				accountingEntryTemplate.checkIVType(frm);
			}
		}
	}
	,checkIVType: function(frm) {
		var cnt=$(".rate-detail",frm).length;
		if(cnt<=0) {
			$("#AddNewIVType",frm).show();
		}
	}
    ,dateChecking: function(txt) {
    	try {
    		if(txt.value!="") {
    			var tr=$(txt).parents("tr:first");
    			var tbl=$(tr).parents("table:first");
    			var eDate=Date.DateAdd("yyyy",1,txt.value);
    			var endDate=Date.DateAdd("d",-1,this.formatDate(eDate));

    			$("#EndDate",tr).val(this.formatDate(endDate));
    			$("#SpnEndDate",tr).html(this.formatDate(endDate));
    			var index=0;
    			$("tbody tr",tbl).each(function() {
    				if(index!=0) {
    					var trPrev=$(this).prev();
    					var ewDate=$("#EndDate",trPrev).val();
    					accountingEntryTemplate.assignDates(this,ewDate);
    				}
    				index++;
    			});
    		}
    		return true;
    	} catch(e) {
    		//jAlert(e);
    	}
    }
	,assignDates: function(tr,startDate) {
		var sDate="";var eDate="";var endDate="";
		if(startDate!="") {
			sDate=Date.DateAdd("d",1,startDate);
			eDate=Date.DateAdd("yyyy",1,this.formatDate(sDate));
			endDate=Date.DateAdd("d",-1,this.formatDate(eDate));
		}
		$("#StartDate",tr).val(this.formatDate(sDate));
		$("#SpnStartDate",tr).html(this.formatDate(sDate));
		$("#EndDate",tr).val(this.formatDate(endDate));
		$("#SpnEndDate",tr).html(this.formatDate(endDate));
	}
	,formatDate: function(dateobj) {
		return $.datepicker.formatDate('mm/dd/yy',dateobj);
	}
	,checkChange: function(obj) {
		var rateGrid=$(obj).parents(".rate-grid:first");
		$("#IsScheduleChange",rateGrid).val("true");
	}
	,onRowBound: function(tr,data) {
		var lastcell=$("td:last div",tr);
		lastcell.html("<img id='Edit' class='gbutton' src='"+jHelper.getImagePath("Edit.png")+"' />");
		$("#Edit",lastcell).click(function() { accountingEntryTemplate.edit(data.cell[0],data.cell[1]); });
		$("td:not(:last)",tr).click(function() { accountingEntryTemplate.edit(data.cell[0],data.cell[1]); });
	}
	,onTemplate: function(tbody,data) {
		$("#GridTemplate").tmpl(data).appendTo(tbody);
	}
	,onGridSuccess: function(t,g) {
		//jHelper.checkValAttr(t);
		//jHelper.jqCheckBox(t);
		//
		$("tbody tr",t).each(function() {
			var tdlen=$("td",this).length;
			if(tdlen<4) {
				$("td:last",this).attr("colspan",(7-$("td",this).length));
			}
		});
		if(accountingEntryTemplate.pageInit==false) {
			var accountingEntryTemplateId=$("#DefaultAccountingEntryTemplateId").val();
			if(accountingEntryTemplateId>0) {
				$("#Edit"+accountingEntryTemplateId).click();
			}
		}
		accountingEntryTemplate.pageInit=true;
		jHelper.gridEditRow(t);
	}
	,onSubmit: function(p) {
		return true;
	}
}
$.extend(window,{
	getTier: function(index,rowindex,item) { var data={ index: index,rowIndex: rowindex,tier: item };return data; }
	,getAccountingEntryTemplateRate: function(index,frs) { var data={ index: index,FRS: frs };return data; }
	,getFormIndex: function() { accountingEntryTemplate.formIndex++;return accountingEntryTemplate.formIndex; }
});