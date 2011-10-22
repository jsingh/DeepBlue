var modifyCapitalDistribution={
	init: function () {
		$(document).ready(function () {
			jHelper.waterMark();
			var id=$("#SearchCapitalDistributionID").val();
			if(id>0) {
				modifyCapitalDistribution.selectCD(id);
			}
		});
	}
	,selectFund: function (id) {
		$("#SearchFundID").val(id);
		$("#SearchCapitalDistribution").val("--Select One--");
		$("#SearchCapitalDistributionID").val(0);
	}
	,searchCD: function (request,response) {
		$.getJSON("/CapitalCall/FindCapitalDistributions?term="+request.term+"&fundId="+$("#SearchFundID").val(),function (data) {
			response(data);
		});
	}
	,selectCD: function (id) {
		var target=$("#ModifyCapitalDistribution");
		target.empty();
		if(id>0) {
			target.html("<center>"+jHelper.loadingHTML()+"</center>");
			$.getJSON("/CapitalCall/FindCapitalDistributionModel/"+id,function (data) {
				target.empty();
				$("#ModifyCapitalDistributionTemplate").tmpl(data).appendTo(target);
				$("#SearchFundName").val(data.FundName);
				$("#SearchFundID").val(data.FundId);
				$("#SearchCapitalDistribution").val(data.DistributionNumber+"# ("+data.FundName+")");
				jHelper.checkValAttr(target);
				jHelper.jqCheckBox(target);
				jHelper.jqComboBox(target);
				jHelper.applyDatePicker(target);
			});
		}
	}
	,showControl: function (chk,boxId) {
		var box=document.getElementById(boxId);
		if(box) {
			if(chk.checked)
				box.style.display="";
			else
				box.style.display="none";
		}
	}
	,calcProfit: function () {
		var DistributionAmount=jHelper.cfloat($("#DistributionAmount").val());
		var PreferredReturn=jHelper.cfloat($("#PreferredReturn").val());
		var ReturnManagementFees=jHelper.cfloat($("#ReturnManagementFees").val());
		var ReturnFundExpenses=jHelper.cfloat($("#ReturnFundExpenses").val());
		var PreferredCatchUp=jHelper.cfloat($("#PreferredCatchUp").val());
		var profit=(DistributionAmount-PreferredReturn-ReturnManagementFees-ReturnFundExpenses-PreferredCatchUp);
	}
	,selectTab: function (type,lnk) {
		var CD=$("#NewCapitalDistribution");
		var MCD=$("#ManualCapitalDistribution");
		$("#NewCDTab").removeClass("section-tab-sel");
		$("#ManCDTab").removeClass("section-tab-sel");
		CD.hide();MCD.hide();
		$(lnk).addClass("section-tab-sel");
		switch(type) {
			case "C": CD.show();break;
			case "M": MCD.show();break;
		}
	}
		,edit: function (img) {
		var tr=$(img).parents("tr:first");
		this.editRow(tr);
		$("#Save",tr).show();
	}
	,editRow: function (tr) {
		$(".show",tr).hide();
		$(".hide",tr).show();
		$(":input:first",tr).focus();
	}
	,save: function (frmid) {
		try {
			var frm=$("#"+frmid);
			var loading=$("#UpdateLoading");
			loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
			var param=$(frm).serializeForm();
			$.post("/CapitalCall/UpdateDistribution",param,function (data) {
				loading.empty();
				if($.trim(data)!="True") {
					jAlert(data);
				} else {
					jAlert("Capital Distribution Saved.");
				}
			});
		} catch(e) {
			jAlert(e);
		}
		return false;
	}
}