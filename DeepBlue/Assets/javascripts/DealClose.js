var dealClose={
	init: function () {
		jHelper.resizeIframe();
		$("document").ready(function () {
			dealClose.checkInputBox($("#tblDealUnderlyingFund"));
			dealClose.checkInputBox($("#tblDealUnderlyingDirect"));
			dealClose.finalClose(document.getElementById("IsFinalClose"));
		});
	}
	,checkInputBox: function (table) {
		$("tr",table).each(function () {
			var chk=$("input:checkbox",this).get(0);
			if(chk) {
				if(chk.checked) {
					$(":input:text",this).removeAttr("disabled");
				}
			}
		});
	}
	,checkDeal: function (chk) {
		var tr=$(chk).parents("tr:first");
		$(":input:text",tr).each(function () {
			this.disabled=!chk.checked;
		});
	}
	,selectDeal: function (id) {
		dealClose.setDealId(id);
		dealClose.onGridSubmit();
	}
	,getDealId: function () { return $("#DealId").val(); }
	,setDealId: function (id) { $("#DealId").val(id); }
	,onGridSubmit: function () {
		var param=[{ name: "dealId",value: dealClose.getDealId()}];
		var grid=$("#DealCloseList");
		grid.flexOptions({ params: param });
		grid.flexReload();
	}
	,add: function (id) {
		var dealId=parseInt(dealClose.getDealId());
		if(dealId>0) {
			var dt=new Date();
			var url="/Deal/EditDealClosing?id="+id+"&dealId="+dealClose.getDealId()+"&t="+dt.getTime();
			jHelper.createDialog(url,{
				title: "Deal Close",
				autoOpen: true,
				width: 1000,
				modal: true,
				position: 'top',
				autoResize: true
			});
		} else {
			alert("Deal is required");
			$("#Deal").focus();
		}
	}
	,onSubmit: function (formId) {
		return jHelper.formSubmit(formId,false);
	}
	,closeDialog: function (reload) {
		$("#addDialog").dialog('close');
		if(reload==true) {
			$("#DealCloseList").flexReload();
		}
	}
	,onCreateBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	}
	,onGridSuccess: function (t) {
		$("tr",t).each(function () {
			$("td:last div",this).html("<img id='Edit' src='/Assets/images/Edit.gif'/>");
		});
	}
	,onRowClick: function (row) {
		dealClose.add(row.cell[0]);
	}
	,onCreateSuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(jQuery.trim(UpdateTargetId.html())!="") {
			alert(UpdateTargetId.html())
		} else {
			parent.dealClose.closeDialog(true);
		}
	}
	,finalClose: function (chk) {
		var tbl=$("#tblDealUnderlyingFund");
		var thCommitmentAmount=$("#thCommitmentAmount",tbl);
		var thGPP=$("#thGPP",tbl);
		var thReassignedGPP=$("#thReassignedGPP",tbl);
		var display="";var disabled=!chk.checked;
		if(chk.checked) { display="none"; } else { display=""; }
		this.hideColumn(thCommitmentAmount,tbl,display);
		this.hideColumn(thGPP,tbl,display);
		if(display=="none") { display=""; } else { display="none"; }
		this.hideColumn(thReassignedGPP,tbl,display);
	}
	,hideColumn: function (th,tbl,display) {
		th.css("display",display);
		var colindex=$("th",tbl).index(th);
		$("tbody tr",tbl).each(function () {
			$("td:eq("+colindex+")",this).css("display",display);
		});
	}
	,calcAmount: function () {
		var totRGPP=0;
		var totPRCC=0;
		var totPRD=0;
		$("tbody tr","#tblDealUnderlyingFund").each(function () {
			var rgpp=parseFloat($("#ReassignedGPP",this).val());
			var prcc=parseFloat($("#PostRecordDateCapitalCall",this).val());
			var prd=parseFloat($("#PostRecordDateDistribution",this).val());
			if(isNaN(rgpp)) { rgpp=0; } if(isNaN(prcc)) { prcc=0; } if(isNaN(prd)) { prd=0; }
			totRGPP+=rgpp;
			totPRCC+=prcc;
			totPRD+=prd;
		});
		$("#SpnRGPP").html("$"+totRGPP);$("#SpnPRCC").html("$"+totPRCC);$("#SpnPRCD").html("$"+totPRD);
	}
}