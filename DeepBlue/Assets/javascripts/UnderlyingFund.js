var underlyingFund={
	init: function () {
		jHelper.resizeIframe();
		$(document).ready(function () {
			underlyingFund.formatPercent("TaxRate");
			underlyingFund.formatPercent("ManagementFee");
			underlyingFund.formatPercent("IncentiveFee");
		});
	}
	,formatPercent: function (txtid) {
		var txt=$("#"+txtid);
		if($.trim(txt.val())!="") {
			txt.val(txt.val()+"%");
		}
	}
	,add: function (id) {
		var dt=new Date();
		var url="/Deal/EditUnderlyingFund/"+id+"?t="+dt.getTime();
		jHelper.createDialog(url,{
			title: "Underlying Fund",
			autoOpen: true,
			width: 800,
			modal: true,
			position: 'top',
			autoResize: true
		});
	}
	,deleteUnderlyingFund: function (id,img) {
		if(confirm("Are you sure you want to delete this underlying fund?")) {
			var dt=new Date();
			var url="/Admin/DeleteUnderlyingFund/"+id+"?t="+dt.getTime();
			$.get(url,function (data) {
				if(data!="") {
					alert(data);
				} else {
					$("#UnderlyingFundList").flexReload();
				}
			});
		}
	}
	,onSubmit: function (formId) {
		return jHelper.formSubmit(formId,false);
	}
	,onRowBound: function (tr,data) {
		var lastcell=$("td:last div",tr);
		lastcell.html("<img id='Edit' src='/Assets/images/Edit.gif'/>");
		$("#Edit",lastcell).click(function () { underlyingFund.add(data.cell[0]); });
		$("td:not(:last)",tr).click(function () { underlyingFund.add(data.cell[0]); });
	}
	,closeDialog: function (reload) {
		$("#addDialog").dialog('close');
		if(reload==true) {
			$("#UnderlyingFundList").flexReload();
		}
	}
	,onCreateUnderlyingFundBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	}
	,onCreateUnderlyingFundSuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(jQuery.trim(UpdateTargetId.html())!="") {
			alert(UpdateTargetId.html())
		} else {
			parent.underlyingFund.closeDialog(true);
		}
	}
	,selectIssuer: function (id) {
		$("#IssuerId").val(id);
	}
	,checkIssuer: function (txt) {
		if($.trim(txt.value)=="") {
			$("#IssuerId").val(0);
		}
	}
}