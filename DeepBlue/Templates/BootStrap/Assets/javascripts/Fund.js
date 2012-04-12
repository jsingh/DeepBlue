var fund={
	index: 0
	,init: function () {
		$('.carousel').carousel({
			stopcycle: true
		})
	}
	,open: function (id) {
		var url="/Fund/FindFund/"+id;
		var target=$("#fundeditbox");
		target.empty();
		$('.carousel').carousel('next');
		$.getJSON(url,function (data) {
			$("#FundAddTemplate").tmpl(data).appendTo(target);
			fund.setup(target);
		});
	}
	,setup: function (target) {
		setTimeout(function () {
			var InvestorList=$("#InvestorList",target);
			InvestorList.flexigrid({
				usepager: true
				,useBoxStyle: false
				,url: "/Fund/InvestorFundList"
				,onSubmit: function (p) {
					p.params=null;
					p.params=new Array();
					p.params[p.params.length]={ "name": "fundId","value": $("#FundId",target).val() };
					return true;
				}
				,onTemplate: function (tbody,data) {
					$("#InvestorGridTemplate").tmpl(data).appendTo(tbody);
				}
				,rpOptions: [25,50,100,200]
				,rp: 25
				,resizeWidth: false
				,method: "GET"
				,sortname: "InvestorName"
				,sortorder: ""
				,autoload: true
				,height: 0
			});

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
	,deleteFund: function (img,id) {
		if(confirm("Are you sure you want to delete this fund?")) {
			img.src=jHelper.getImagePath("ajax.jpg");
			$.get("/Fund/Delete/"+id,function (data) {
				if(data!="") {
					jAlert(data);
				} else {
					$("#FundList").flexReload();
				}
			});
		}
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
		var tbl=$("#FundList");
		$("#EditRow"+id,tbl).remove();
		$("#lnkAddFund").removeClass("green-btn-sel");
		var row;
		if(id==0) {
			row=$("tbody tr:first",tbl);
			$("#lnkAddFund").addClass("green-btn-sel");
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
		fund.open(id,target);
	}
	,bootStrapAlert: function (data) {
		var alertRow=$("#AlertRow");
		alertRow.empty();
		$("#alertTemplate").tmpl(data).appendTo(alertRow);
		$(".alert").alert();
	}
	,save: function (imgbtn) {
		var frm=$(imgbtn).parents("form:first");
		var fundId=parseInt($("#FundId",frm).val());
		var savebutton=$("#save",frm);
		savebutton.button("loading");
		$.post("/Fund/Create",$(frm).serializeForm(),function (data) {
			savebutton.button("reset");
			if($.trim(data.Result)!="") {
				//jAlert(data.Result);
				var data={ iswarning: false,message: data.Result.replace(/\n/g,"<br/>") };
				fund.bootStrapAlert(data);
			} else {
				jAlert("Fund Saved.","Fund Library");
				$("#FundList").flexReload();
				$('.carousel').carousel('prev');
			}
		},"JSON");
	}
	,cancel: function (id) {
		//$("#EditRow"+id).remove();
		//$("#lnkAddFund").removeClass("green-btn-sel");
		$('.carousel').carousel('prev');
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
				$(rateobj).attr("readonly","readonly");
				$(flatFeeObj).attr("readonly","readonly");
				break;
			case "1":
				$(rateobj).removeAttr("readonly");
				$(flatFeeObj).attr("readonly","readonly");
				break;
			case "2":
				$(rateobj).attr("readonly","readonly");
				$(flatFeeObj).removeAttr("readonly");
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
	,changeRate: function (txt) {
		var rate=parseInt(txt.value);
		if(rate>100) {
			txt.focus();
			jAlert("Rate must be under 100%");
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
		try {
			var frm=$(that).parents("form:first");
			var rateSchdules=$("#RateSchdules",frm);
			var cnt=parseInt($("#FundRateSchedulesCount").val());
			var data={ index: cnt,FRS: fund.newFundData.FundRateSchedules[0] };
			$("#FundRateSchduleTemplate").tmpl(data).appendTo(rateSchdules);
			var newRateDetail=$(".table:last",rateSchdules);
			cnt=cnt+1;
			$("#FundRateSchedulesCount").val(cnt);
			$("#AddNewIVType",frm).hide();
			$("#InvestorTypeId",frm).val(1);
			fund.setup(newRateDetail);
		} catch(e) {
			alert(e);
		}
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
		jHelper.jqComboBox(newRow);
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
						jAlert("Investor type already chosen");
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
    		//jAlert(e);
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
		var rateGrid=$(obj).parents(".rate-detail:first");
		$("#IsScheduleChange",rateGrid).val("true");
	}
	,onRowBound: function (tr,data) {
		var lastcell=$("td:last div",tr);
		lastcell.html("<img id='Edit' class='gbutton' src='"+jHelper.getImagePath("Edit.png")+"' />");
		$("#Edit",lastcell).click(function () { fund.edit(data.cell[0],data.cell[1]); });
		$("td:not(:last)",tr).click(function () { fund.edit(data.cell[0],data.cell[1]); });
	}
	,onTemplate: function (tbody,data) {
		$("#GridTemplate").tmpl(data).appendTo(tbody);
	}
	,onGridSuccess: function (t,g) {
		//jHelper.checkValAttr(t);
		//jHelper.jqCheckBox(t);
		//
		$("tbody tr",t).each(function () {
			var tdlen=$("td",this).length;
			if(tdlen<4) {
				$("td:last",this).attr("colspan",(7-$("td",this).length));
			}
		});
		if(fund.pageInit==false) {
			var fundId=$("#DefaultFundId").val();
			if(fundId>0) {
				$("#Edit"+fundId).click();
			}
		}
		fund.pageInit=true;
		jHelper.gridEditRow(t);
	}
	,onSubmit: function (p) {
		p.params=null;
		p.params=new Array();
		p.params[p.params.length]={ "name": "fundId","value": $("#DefaultFundId").val() };
		return true;
	}
}
$.extend(window,{
	getTier: function (index,rowindex,item) { var data={ index: index,rowIndex: rowindex,tier: item };return data; }
	,getFundRate: function (index,frs) { var data={ index: index,FRS: frs };return data; }
	,getFormIndex: function () { fund.formIndex++;return fund.formIndex; }
});