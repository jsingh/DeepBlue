var dealActivity={
	init: function () {
		jHelper.resizeIframe();
		$(".expandbtn").click(function () {
			var parent=$(this).parent().parent();
			if(this.src.indexOf("S_")>0) {
				$(".fieldbox",parent).hide();
				this.src=this.src.replace("S_","U_");
			} else {
				$(".fieldbox",parent).show();
				this.src=this.src.replace("U_","S_");
			}
		});
		$(document).ready(function () {
			dealActivity.loadTemplate("CashDistributionAddTemplate",$("#AddNewCD"),newCDData);
			dealActivity.loadTemplate("CapitalCallAddTemplate",$("#AddNewCC"),newCCData);
		});
	}
	,isCapitalCallDialog: false
	,onSuccessSave: null
	,applyAutoComplete: function (target) {
		var UnderlyingFundId=$("#UnderlyingFundId",target);
		var FundId=$("#FundId",target);
		$("#UnderlyingFundName",target)
		.blur(function () { if($.trim(this.value)=="") { UnderlyingFundId.val(0); } })
		.autocomplete({ source: "/Deal/FindUnderlyingFunds",minLength: 1,select: function (event,ui) { UnderlyingFundId.val(ui.item.id); },appendTo: "body",delay: 300 });
		$("#FundName",target)
		.blur(function () { if($.trim(this.value)=="") { FundId.val(0); } })
		.autocomplete({ source: "/Fund/FindFunds",minLength: 1,select: function (event,ui) { FundId.val(ui.item.id); },appendTo: "body",delay: 300 });

	}
	,onListSuccess: function (t,p) {
		var tfoot=$("tfoot",t).get(0);
		if(!tfoot) {
			tfoot=document.createElement("tfoot");
			var trviewmore=document.createElement("tr");
			var td=document.createElement("td");
			td.colSpan=6;
			td.innerHTML="View More";
			$(td).css("cursor","pointer").click(function () { $(t).ajaxTableViewMore(); });
			$(trviewmore).append(td);
			$(tfoot).append(trviewmore);
			$(t).append(tfoot);
		}
		if(p.pages==p.newp&&p.pages>0) { $(tfoot).remove(); }
	}
	/* Cash Distribution */
	,findCD: function (id,isEdit) {
		var dt=new Date();
		var url="/Deal/FindUnderlyingFundCashDistribution/"+id+"?t="+dt.getTime();
		var target;
		if(isEdit) { target=$("#EditCD"); } else { target=$("#AddNewCD"); }
		target.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
		$.getJSON(url,function (data) {
			dealActivity.loadTemplate("CashDistributionAddTemplate",target,data);
		});
	}
	,addCD: function (id) {
		var dt=new Date();
		var url="/Deal/EditUnderlyingFundCashDistribution/"+id+"?t="+dt.getTime();
		this.isCapitalCallDialog=false;
		//	this.openDialog(url,"Underlying Fund Cash Distribution");
		//dealActivity.findCD(id);
		var editCD=$("#EditCD");
		editCD.dialog("destroy");
		editCD.dialog({
			title: "Underlying Fund Cash Distribution",
			autoOpen: true,
			width: 600,
			modal: true,
			position: 'top',
			autoResize: true,
			open: function () {
				dealActivity.findCD(id,true);
				dealActivity.onSuccessSave=function () { editCD.dialog('close'); }
			}
		});
	}
	,deleteCD: function (id,img) {
		if(confirm("Are you sure you want to delete this underlying fund cash distribution?")) {
			var dt=new Date();
			var url="/Deal/DeleteUnderlyingFundCashDistribution/"+id+"?t="+dt.getTime();
			var tr=$(img).parents("tr:first");
			var spnloading=$("#spnloading",tr);
			spnloading.html("<img src='/Assets/images/ajax.jpg'/>");
			$.get(url,function (data) {
				if(data!="") {
					alert(data);
				} else {
					var t=$(img).parents("table:first");
					var trid="tr"+id;
					$("#"+trid,t).remove();
					$("#em"+trid,t).remove();
				}
			});
		}
	}
	,onCDSubmit: function (frm) {
		var param=$(frm).serialize();
		var url="/Deal/CreateUnderlyingFundCashDistribution";
		var ULoading=$("#UpdateLoading",frm);
		ULoading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
		$.ajax({
			type: "POST",
			url: url,
			data: param,
			success: function (data) {
				ULoading.html("");var arr=data.split("||");
				if(arr[0]!="True") { alert(arr[0]); }
				else {
					jHelper.resetFields(frm);var dt=new Date();
					$.getJSON("/Deal/GetUnderlyingFundCashDistributionList/"+arr[1]+"?t="+dt.getTime(),function (addData) {
						$("#CashDistributionList").ajaxTableAddData(addData);
					});
				} if(dealActivity.onSuccessSave) { dealActivity.onSuccessSave(); }
			},
			error: function (data) { alert(data); }
		});
		return false;
	}
	,onCDListSubmit: function (p) {
		p.params=[{ name: "underlyingFundCashDistributionId",value: $("#UnderlyingFundCashDistributionId").val()}];
		return true;
	}
	/* End Cash Distribution */
	/* Template */
	,loadTemplate: function (tid,target,data) {
		target.html("");
		$("#"+tid).tmpl(data).appendTo(target);
		jHelper.applyDatePicker(target);
		jHelper.formatDateTxt(target);
		jHelper.checkValAttr(target);
		dealActivity.applyAutoComplete(target);
	}
	/* End Template */
	/* Capital Call */
	,findCC: function (id,isEdit) {
		var dt=new Date();
		var url="/Deal/FindUnderlyingFundCapitalCall/"+id+"?t="+dt.getTime();
		var target;
		if(isEdit) { target=$("#EditCC"); } else { target=$("#AddNewCC"); }
		target.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
		$.getJSON(url,function (data) {
			dealActivity.loadTemplate("CapitalCallAddTemplate",target,data);
		});
	}
	,addCC: function (id) {
		var dt=new Date();
		var url="/Deal/EditUnderlyingFundCapitalCall/"+id+"?t="+dt.getTime();
		this.isCapitalCallDialog=true;
		//this.openDialog(url,"Underlying Fund Capital Call");
		var editCC=$("#EditCC");
		editCC.dialog("destroy");
		editCC.dialog({
			title: "Underlying Fund Capital Call",
			autoOpen: true,
			width: 600,
			modal: true,
			position: 'top',
			autoResize: true,
			open: function () {
				dealActivity.findCC(id,true);
				dealActivity.onSuccessSave=function () { editCC.dialog('close'); }
			}
		});
	}
	,deleteCC: function (id,img) {
		if(confirm("Are you sure you want to delete this underlying fund capital call?")) {
			var dt=new Date();
			var url="/Deal/DeleteUnderlyingFundCapitalCall/"+id+"?t="+dt.getTime();
			var tr=$(img).parents("tr:first");
			var spnloading=$("#spnloading",tr);
			spnloading.html("<img src='/Assets/images/ajax.jpg'/>");
			$.get(url,function (data) {
				if(data!="") {
					alert(data);
				} else {
					spnloading.empty();
					var t=$(img).parents("table:first");
					var trid="tr"+id;
					$("#"+trid,t).remove();
					$("#em"+trid,t).remove();
				}
			});
		}
	}
	,onCCSubmit: function (frm) {
		var param=$(frm).serialize();
		var url="/Deal/CreateUnderlyingFundCapitalCall";
		var ULoading=$("#UpdateLoading",frm);
		ULoading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
		$.ajax({
			type: "POST",
			url: url,
			data: param,
			success: function (data) {
				ULoading.html("");var arr=data.split("||");
				if(arr[0]!="True") { alert(arr[0]); }
				else {
					jHelper.resetFields(frm);var dt=new Date();
					$.getJSON("/Deal/GetUnderlyingFundCapitalCallList/"+arr[1]+"?t="+dt.getTime(),function (addData) {
						$("#CapitalCallList").ajaxTableAddData(addData);
					});
				} if(dealActivity.onSuccessSave) { dealActivity.onSuccessSave(); }
			},
			error: function (data) { alert(data); }
		});
		return false;
	}
	/* End Capital Call */
	/* Common Functions */
	,onRowBound: function (tr,data,t) {
		var trid="tr"+data.cell[0];
		var existrow=$("#"+trid,t);
		if(existrow.get(0)) { existrow.after(tr); }
		existrow.remove();
		$("#em"+trid,t).remove();
		tr.id=trid;
		var lastcell=$("td:last",tr);
		lastcell.html("<span><img id='Edit' src='/Assets/images/Editbtn.png'/>&nbsp;&nbsp;&nbsp;<img id='Delete' src='/Assets/images/DeleteBtn.png'/></span><span id='spnloading'></span>");
		//lastcell.html("<img id='Edit' src='/Assets/images/Editbtn.png'/>");
		var addFunction=function (t) {
			if(t.id=="CashDistributionList") {
				dealActivity.addCD(data.cell[0]);
			} else {
				dealActivity.addCC(data.cell[0]);
			}
		};
		$("#Edit",lastcell).click(function () { addFunction(t); });
		$("td:not(:last)",tr).click(function () { addFunction(t); });
		$("#Delete",lastcell).click(function () {
			if(t.id=="CashDistributionList") {
				dealActivity.deleteCD(data.cell[0],this);
			} else {
				dealActivity.deleteCC(data.cell[0],this);
			}
		});
		var emptyrow=document.createElement("tr");
		var emptycell=document.createElement("td");
		emptyrow.id="em"+trid;
		emptyrow.className="emptyrow";emptycell.innerHTML="&nbsp;";emptycell.colSpan=$("td",tr).length;
		$(emptyrow).append(emptycell);
		$(tr).before(emptyrow);
	}
	,closeDialog: function (reload) {
		$("#addDialog").dialog('close');
		if(reload==true) {
			if(this.isCapitalCallDialog) {
				$("#CapitalCallList").flexReload();
			} else {
				$("#CashDistributionList").flexReload();
			}
		}
	}
	,onSubmit: function (formId) {
		return jHelper.formSubmit(formId,false);
	}
	,onCreateBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	}
	,onCreateSuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(jQuery.trim(UpdateTargetId.html())!="") {
			alert(UpdateTargetId.html())
		} else {
			parent.dealActivity.closeDialog(true);
		}
	}
	,selectFund: function (id) {
		$("#FundId").val(id);
	}
	,selectUnderlyingFund: function (id) {
		$("#UnderlyingFundId").val(id);
	}
	,openDialog: function (url,title) {
		jHelper.createDialog(url,{
			title: title,
			autoOpen: true,
			width: 600,
			modal: true,
			position: 'top',
			autoResize: true
		});
	}
	/* End Common Functions */
}