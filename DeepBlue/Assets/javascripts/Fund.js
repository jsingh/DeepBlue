var fund={
	rowIndex: 0
	,index: -1
	,formIndex: 0
	,rateDetailHTML: ''
	,rateTierFirstRow: null
	,pageInit: false
	,newFundData: null
	,init: function () {
		fund.loadTemplate(fund.newFundData,$("#AddNewFund"));
	}
	,setup: function (target) {
		setTimeout(function () {
			jHelper.checkValAttr(target);
			jHelper.jqComboBox(target);
			$(".ddlist",target).each(function () {
				if($(this).hasClass("investortype")==false) {
					$(this).change();
				}
			});
			$(".datefield",target).each(function () {
				fund.applyDatePicker(this);
			});
			$(".rate-grid",target).each(function () {
				var txt=$("#RateScheduleList .datefield",this).get(0);
				if(txt) { fund.dateChecking(txt); }
			});
		});
	}
	,selectTab: function (that,detailid) {
		$(".section-tab").removeClass("section-tab-sel");
		$(".section-det").hide();
		$(that).addClass("section-tab-sel");
		$("#"+detailid).show();
	}
	,expand: function () {
		$(".headerbox").click(function () {
			$(".headerbox").show();
			$(".expandheader").hide();
			$(".detail").hide();
			$(this).hide();
			var parent=$(this).parent();
			$(".expandheader",parent).show();
			var detail=$(".detail",parent);
			var display=detail.attr("issearch");
			detail.show();
		});
		$(".expandheader").click(function () {
			var expandheader=$(this);
			var parent=$(expandheader).parent();
			expandheader.hide();
			var detail=$(".detail",parent);
			detail.hide();
			$(".headerbox",parent).show();
		});
	}
	,loadTemplate: function (data,target) {
		target.empty();
		$("#FundAddTemplate").tmpl(data).appendTo(target);
		fund.expand();
		fund.setup(target);
		jHelper.jqCheckBox(target);
	}
	,deleteTab: function (id,isConfirm) {
		var isRemove=true;
		if(isConfirm) {
			isRemove=confirm("Are you sure you want to remove this fund?");
		}
		if(isRemove) {
			$("#Tab"+id).remove();
			$("#Edit"+id).remove();
			$("#tabdel"+id).remove();
			$("#TabFundGrid").click();
		}
	}
	,edit: function (id,fundName) {
		var addNewFund=$("#AddNewFund");
		var addNewFundTab=$("#TabAddNewFund");
		var data={ id: id,FundName: fundName };
		var tab=$("#Tab"+id);
		var editbox=$("#Edit"+id);
		if(tab.get(0)) {
			addNewFundTab.after(tab);
			addNewFund.after(editbox);
		} else {
			$("#SectionTemplate").tmpl(data).insertAfter(addNewFund);
			$("#TabTemplate").tmpl(data).insertAfter(addNewFundTab);
			this.open(id,$("#Edit"+id));
		}
		$(".center",$("#Tab"+id)).click();
	}
	,open: function (id,target) {
		var dt=new Date();
		var url="/Fund/FindFund/"+id+"?t="+dt.getTime();
		target.html("<center><br/><img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...</center>");
		$.getJSON(url,function (data) {
			fund.loadTemplate(data,target);
		});
	}
	,save: function (imgbtn) {
		var frm=$(imgbtn).parents("form:first");
		var loading=$("#UpdateLoading",frm);
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
		var fundId=parseInt($("#FundId",frm).val());
		$.post("/Fund/Create",$(frm).serializeForm(),function (data) {
			loading.empty();
			if($.trim(data)!="") {
				alert(data);
			} else {
				alert("Fund Saved.");
				$("#FundList").flexReload();
				if(fundId==0) {
					$("#TabFundGrid").click();
					fund.init();
				} else {
					fund.deleteTab(fundId,false);
				}
			}
		});
	}
	,changeRS: function (ddl) {
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
	,applyDatePicker: function (txt) {
		fund.index=fund.index+1;
		txt.id="dp_"+fund.index;
		$(txt).datepicker({ changeMonth: true,changeYear: true });
	}
	,addRateSchedule: function (that) {
		var frm=$(that).parents("form:first");
		var rateSchdules=$("#RateSchdules",frm);
		var cnt=parseInt($("#FundRateSchedulesCount").val());
		var data={ index: cnt,FRS: fund.newFundData.FundRateSchedules[0] };
		$("#FundRateSchduleTemplate").tmpl(data).appendTo(rateSchdules);
		var newRateDetail=$(".rate-detail:last",frm);
		cnt=cnt+1;
		$("#FundRateSchedulesCount").val(cnt);
		$("#AddNewIVType",frm).hide();
		fund.setup(newRateDetail);
	}
	,addNewRow: function (addlink) {
		var tbl=$(addlink).parents("table:first");
		var grid=$(tbl).parent();
		var index=$("#ScheduleIndex",grid).val();
		var lastrow=$("tbody tr:last",tbl);
		var cnt=$("tbody tr",tbl).length;
		var data={ index: index,rowIndex: (cnt+1),tier: fund.newFundData.FundRateSchedules[0].FundRateScheduleTiers[0] };
		$("#FundRateSchduleTierTemplate").tmpl(data).insertAfter(lastrow);
		$("#TiersCount",grid).val(cnt);
		var newRow=$("tbody tr:last",tbl);
		var txt=$(".datefield",tbl).get(0);
		if(txt) {
			fund.dateChecking(txt);
			fund.applyDatePicker(txt);
		}
	}
	,changeInvestorType: function (invType) {
		var frm=$(invType).parents("form:first");
		if(invType.value!="0") {
			$(".investortype",frm).each(function () {
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
			var frm=$(rateDetail).parents("form:first");
			var FundRateScheduleId=$("#FundRateScheduleId",rateDetail).val();
			if(parseInt(FundRateScheduleId)>0) {
				$.get("/Fund/DeleteFundRateSchedule/?id="+FundRateScheduleId,function (data) {
					$(rateDetail).remove();
					fund.checkIVType(frm);
				});
			} else {
				$(rateDetail).remove();
				fund.checkIVType(frm);
			}
		}
	}
	,checkIVType: function (frm) {
		var cnt=$(".rate-detail",frm).length;
		if(cnt<=0) {
			$("#AddNewIVType",frm).show();
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
    			var index=0;
    			$("tbody tr",tbl).each(function () {
    				if(index!=0) {
    					var trPrev=$(this).prev();
    					var ewDate=$("#EndDate",trPrev).val();
    					fund.assignDates(this,ewDate);
    				}
    				index++;
    			});
    		}
    		return true;
    	} catch(e) {
    		//alert(e);
    	}
    }
	,assignDates: function (tr,startDate) {
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
	,formatDate: function (dateobj) {
		return $.datepicker.formatDate('mm/dd/yy',dateobj);
	}
	,checkChange: function (obj) {
		var rateGrid=$(obj).parents(".rate-grid:first");
		$("#IsScheduleChange",rateGrid).val("true");
	}
	,onRowBound: function (tr,data) {
		var lastcell=$("td:last div",tr);
		lastcell.html("<img id='Edit' class='gbutton' src='/Assets/images/Edit.png'/>");
		$("#Edit",lastcell).click(function () { fund.edit(data.cell[0],data.cell[1]); });
		$("td:not(:last)",tr).click(function () { fund.edit(data.cell[0],data.cell[1]); });
	}
}
$.extend(window,{
	getTier: function (index,rowindex,item) { var data={ index: index,rowIndex: rowindex,tier: item };return data; }
	,getFundRate: function (index,frs) { var data={ index: index,FRS: frs };return data; }
	,getFormIndex: function () { fund.formIndex++;return fund.formIndex; }
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