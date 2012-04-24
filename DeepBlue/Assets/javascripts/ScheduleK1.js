var schedule={
	rowIndex: 0
	,index: -1
	,formIndex: 0
	,rateDetailHTML: ''
	,rateTierFirstRow: null
	,pageInit: false
	,newScheduleData: null
	,init: function () {
		jHelper.waterMark($("body"));
	}
	,setup: function (target) {
		setTimeout(function () {
			var UnderlyingFundID=$("#UnderlyingFundID",target);
			var UnderlyingFundName=$("#UnderlyingFundName",target);
			var FundID=$("#FundID",target);
			var FundName=$("#FundName",target);
			var PartnerState=$("#PartnerState",target);
			var PartnerStateName=$("#PartnerStateName",target);
			var PartnerCountry=$("#PartnerCountry",target);
			var PartnerCountryName=$("#PartnerCountryName",target);

			UnderlyingFundName
			.autocomplete({ source: "/Deal/FindUnderlyingFunds",minLength: 1
				,select: function (event,ui) {
					UnderlyingFundID.val(ui.item.id);
				},appendTo: "body",delay: 300
			});

			FundName
			.autocomplete({ source: "/Fund/FindFunds",minLength: 1
			,select: function (event,ui) {
				FundID.val(ui.item.id);
			},appendTo: "body",delay: 300
			});

			PartnerStateName
			.autocomplete({ source: "/Admin/FindStates",minLength: 1
			,select: function (event,ui) {
				PartnerState.val(ui.item.id);
			}
			,appendTo: "body",delay: 300
			});

			PartnerCountryName
			.autocomplete({ source: "/Admin/FindCountrys",minLength: 1
			,select: function (event,ui) {
				PartnerCountry.val(ui.item.id);
				var AddressStateRow=$("#AddressStateRow",target);
				PartnerState.val(52);
				if(ui.item.label!="United States") {
					AddressStateRow.hide();
				} else {
					AddressStateRow.show();
					PartnerStateName.val("");
					PartnerState.val(0);
				}
			}
			,appendTo: "body",delay: 300
			});

		});
	}
	,selectTab: function (that,detailid) {
		$(".section-tab").removeClass("section-tab-sel");
		$(".section-det").hide();
		$(that).addClass("section-tab-sel");
		$("#"+detailid).show();
	}
	,expand: function (target) {
		$(".headerbox",target).click(function () {
			$(this).hide();
			var parent=$(this).parent();
			$(".expandheader",parent).show();
			var detail=$(".detail",parent);
			var display=detail.attr("issearch");
			detail.show();
		});
		$(".expandheader",target).click(function () {
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
		$("#ScheduleAddTemplate").tmpl(data).appendTo(target);
		schedule.expand(target);
		schedule.setup(target);
		jHelper.checkValAttr(target);
		jHelper.jqCheckBox(target);
	}
	,deleteScheduleK1: function (img,id) {
		if(confirm("Are you sure you want to delete this schedule k-1?")) {
			img.src=jHelper.getImagePath("ajax.jpg");
			$.get("/Admin/DeleteScheduleK1/"+id,function (data) {
				if(data!="") {
					jAlert(data);
				} else {
					$("#ScheduleList").flexReload();
				}
			});
		}
	}
	,deleteTab: function (id,isConfirm) {
		var isRemove=true;
		if(isConfirm) {
			isRemove=confirm("Are you sure you want to remove this schedule?");
		}
		if(isRemove) {
			$("#Tab"+id).remove();
			$("#Edit"+id).remove();
			$("#tabdel"+id).remove();
			$("#TabScheduleGrid").click();
		}
	}
	,edit: function (id,scheduleName) {
		var tbl=$("#ScheduleList");
		$("#EditRow"+id,tbl).remove();
		$("#lnkAddSchedule").removeClass("green-btn-sel");
		var row;
		if(id==0) {
			row=$("tbody tr:first",tbl);
			$("#lnkAddSchedule").addClass("green-btn-sel");
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
		schedule.open(id,target);
	}
	,open: function (id,target) {
		var dt=new Date();
		var url="/Admin/FindScheduleK1/"+id+"?t="+dt.getTime();
		target.html("<center>"+jHelper.loadingHTML()+"</center>");
		$.getJSON(url,function (data) {
			schedule.loadTemplate(data,target);
		});
	}
	,save: function (imgbtn) {
		var frm=$(imgbtn).parents("form:first");
		var loading=$("#UpdateLoading",frm);
		loading.html(jHelper.savingHTML());
		var scheduleId=parseInt($("#ScheduleId",frm).val());
		$.post("/Admin/CreateScheduleK1",$(frm).serializeForm(),function (data) {
			loading.empty();
			if($.trim(data)!="") {
				jAlert(data);
			} else {
				jAlert("Schedule K-1 Saved.");
				$("#lnkAddSchedule").removeClass("green-btn-sel");
				$("#ScheduleList").flexReload();
			}
		});
	}
	,cancel: function (id) {
		$("#EditRow"+id).remove();
		$("#lnkAddSchedule").removeClass("green-btn-sel");
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
		schedule.index=schedule.index+1;
		txt.id="dp_"+schedule.index;
		$(txt).datepicker({ changeMonth: true,changeYear: true });
	}
	,addRateSchedule: function (that) {
		var frm=$(that).parents("form:first");
		var rateSchdules=$("#RateSchdules",frm);
		var cnt=parseInt($("#ScheduleRateSchedulesCount").val());
		var data={ index: cnt,FRS: schedule.newScheduleData.ScheduleRateSchedules[0] };
		$("#ScheduleRateSchduleTemplate").tmpl(data).appendTo(rateSchdules);
		var newRateDetail=$(".rate-detail:last",frm);
		cnt=cnt+1;
		$("#ScheduleRateSchedulesCount").val(cnt);
		$("#AddNewIVType",frm).hide();
		schedule.setup(newRateDetail);
	}
	,addNewRow: function (addlink) {
		var tbl=$(addlink).parents("table:first");
		var grid=$(tbl).parent();
		var index=$("#ScheduleIndex",grid).val();
		var lastrow=$("tbody tr:last",tbl);
		var cnt=$("tbody tr",tbl).length;
		var data={ index: index,rowIndex: (cnt+1),tier: schedule.newScheduleData.ScheduleRateSchedules[0].ScheduleRateScheduleTiers[0] };
		$("#ScheduleRateSchduleTierTemplate").tmpl(data).insertAfter(lastrow);
		$("#TiersCount",grid).val(cnt);
		var newRow=$("tbody tr:last",tbl);
		jHelper.jqComboBox(newRow);
		var txt=$(".datefield",tbl).get(0);
		if(txt) {
			schedule.dateChecking(txt);
			schedule.applyDatePicker(txt);
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
			var ScheduleRateScheduleId=$("#ScheduleRateScheduleId",rateDetail).val();
			if(parseInt(ScheduleRateScheduleId)>0) {
				$.get("/Schedule/DeleteScheduleRateSchedule/?id="+ScheduleRateScheduleId,function (data) {
					$(rateDetail).remove();
					schedule.checkIVType(frm);
				});
			} else {
				$(rateDetail).remove();
				schedule.checkIVType(frm);
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
    					schedule.assignDates(this,ewDate);
    				}
    				index++;
    			});
    		}
    		return true;
    	} catch(e) {
    		//jAlert(e);
    	}
    }
	,export: function (id) {
		var url="";
		var width=300;var height=200;var left=(screen.availWidth/2)-(width/2);var top=(screen.availHeight/2)-(height/2);var features="width="+width+",height="+height+",left="+left+",top="+top+",location=no,menubar=no,toobar=no,scrollbars=yes,resizable=yes,status=yes";
		url="/Admin/ExportScheduleK1Pdf/"+id;
		window.open(url,"exportexcel",features);
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
		lastcell.html("<img id='Edit' class='gbutton' src='"+jHelper.getImagePath("Edit.png")+"' />");
		$("#Edit",lastcell).click(function () { schedule.edit(data.cell[0],data.cell[1]); });
		$("td:not(:last)",tr).click(function () { schedule.edit(data.cell[0],data.cell[1]); });
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
		if(schedule.pageInit==false) {
			var scheduleId=$("#DefaultScheduleId").val();
			if(scheduleId>0) {
				$("#Edit"+scheduleId).click();
			}
		}
		schedule.pageInit=true;
		jHelper.gridEditRow(t);
	}
	,onSubmit: function (p) {
		p.params=null;
		p.params=new Array();
		p.params[p.params.length]={ "name": "fundID","value": $("#SearchFundID").val() };
		p.params[p.params.length]={ "name": "underlyingFundID","value": $("#SearchUnderlyingFundID").val() };
		return true;
	}
}
$.extend(window,{
	getTier: function (index,rowindex,item) { var data={ index: index,rowIndex: rowindex,tier: item };return data; }
	,getScheduleRate: function (index,frs) { var data={ index: index,FRS: frs };return data; }
	,getFormIndex: function () { schedule.formIndex++;return schedule.formIndex; }
});