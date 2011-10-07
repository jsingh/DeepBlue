var investorCommitment={
	CurrentFundCloseId: null
	,CurrentFundCloseName: null
	,init: function () {
		$(document).ready(function () {
			investorCommitment.loadInvestor();
			jHelper.waterMark();
		});
	}
	,setInvestorId: function (id) {
		$("#InvestorId","#TransactionMain").val(id);
	}
	,getInvestorId: function () {
		var id=parseInt($("#InvestorId","#TransactionMain").val());
		if(isNaN(id)) id=0;
		return id;
	}
	,selectInvestor: function (id) {
		investorCommitment.setInvestorId(id);
		investorCommitment.loadInvestor();
	}
	,loadInvestor: function () {
		var id=investorCommitment.getInvestorId();
		var target=$("#TransactionContainer");
		target.html("<center>"+jHelper.loadingHTML()+"</center>");
		if(id>0) {
			$.getJSON("/Investor/InvestorDetail/"+id,function (data) {
				target.empty();
				$("#Investor").val(data.InvestorName);
				$("#TransactionInformationTemplate").tmpl(data).appendTo(target);
				var invloading=$("#InvListLoading");
				var p=new Array();
				p[p.length]={ "name": "investorId","value": id };
				$("#InvestmentList",target).flexigrid({
					usepager: true
					,url: "/Transaction/InvestmentDetailList"
					,params: p
					,resizeWidth: true
					,method: "GET"
					,sortname: "FundName"
					,sortorder: "asc"
					,autoload: true
					,useBoxStyle: false
					,onInit: investorCommitment.onInit
					,onTemplate: investorCommitment.onTemplate
					,onSubmit: function () { invloading.html(jHelper.loadingHTML());return true; }
					,onSuccess: function () { invloading.empty(); }
				});
			});
		} else {
			target.empty();
		}
	}
	,addTransaction: function (img,id) {
		var tr=$("#Row"+id);
		var param=jHelper.serialize(tr);
		var url="/Transaction/CreateInvestorFund";
		var imgsrc=img.src;
		$(img).attr("src","/Assets/images/ajax.jpg");
		$.post(url,param,function (data) {
			img.src=imgsrc;
			if($.trim(data)!="") {
				jAlert(data);
			}
			else {
				jAlert("Commitment Saved.");
				investorCommitment.loadFundDetails();
			}
		});
	}
	,save: function (img,id) {
		var tr=$("#Row"+id);
		var param=jHelper.serialize(tr);
		var url="/Transaction/UpdateCommitmentAmount";
		var imgsrc=img.src;
		$(img).attr("src","/Assets/images/ajax.jpg");
		$.post(url,param,function (data) {
			img.src=imgsrc;
			if($.trim(data)!="") {
				jAlert(data);
			}
			else {
				jAlert("Committed Amount Saved");
				$("#SpnCA",tr).html(formatCurrency($("#TotalCommitment",tr).val()));
				$("#SpnUFA",tr).html(formatCurrency($("#UnfundedAmount",tr).val()));
				$(".show",tr).show();
				$(".hide",tr).hide();
				$("#Save",tr).hide();
			}
		});
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
	,setUpRow: function (target) {
		$("#SpnInvestorType",target).hide().html("");
		$("#InvestorTypeId",target).val(0);
		jHelper.jqComboBox(target);
		var FundName=$("#FundName",target);
		var FundClosing=$("#FundClose",target);
		var FundClosingId=$("#FundClosingId",target);
		var fundClosingSearch="/Fund/FindFundClosings";
		var investorType=$("#InvestorTypeId",target);

		var FundId=$("#FundId",target);
		FundName
		.autocomplete({ source: "/Fund/FindFunds",minLength: 1
		,select: function (event,ui) {
			FundId.val(ui.item.id);
			$.getJSON("/Transaction/FundClosingList/?fundId="+FundId.val()+"&investorId="+investorCommitment.getInvestorId(),
			function (data) {
				if(data.InvestorTypeId>0) {
					$("#SpnInvestorType",target).show().html(data.InvestorTypeName);
					investorType.hide().val(data.InvestorTypeId);
					investorType.combobox('destroy')
					jHelper.jqComboBox(target);
					investorType.combobox('hide')
				} else {
					$("#SpnInvestorType",target).hide();
					investorType.val(0)
					investorType.combobox('destroy')
					jHelper.jqComboBox(target);
					investorType.combobox('show')
				}
			});
			FundClosingId.val(0);
			FundClosing.val("Search").autocomplete("option","source",
				function (request,response) {
					$.getJSON(fundClosingSearch+"?term="+request.term+"&fundId="+FundId.val(),function (data) {
						data[data.length]={ "id": -1,"value": "Add Fund Close","label": "Add Fund Close" }
						response(data);
					});
				}
			);
			//investorCommitment.loadFundClosing(ui.item.id,target);
		}
		,appendTo: "body",delay: 300
		});

		FundClosing.autocomplete({
			source:
			function (request,response) {
				$.getJSON(fundClosingSearch+"?term="+request.term+"&fundId="+FundId.val(),function (data) {
					data[data.length]={ "id": -1,"value": "Add Fund Close","label": "Add Fund Close" }
					response(data);
				});
			}
		,minLength: 1
			,select: function (event,ui) {
				FundClosingId.val(ui.item.id);
				if(FundId.val()>0) {
					if(ui.item.id== -1) {
						investorCommitment.CurrentFundCloseId=FundClosingId;
						investorCommitment.CurrentFundCloseName=FundClosing;
						investorCommitment.checkFundClose(ui.item.id,FundId.val(),FundName.val());
					}
				} else {
					jAlert("Fund is required");
				}
			}
		});

	}
	,checkFundClose: function (id,fundId,fundName) {
		if(id== -1) {
			var frm=$("#frmAddFundClose");
			var frmTransaction=$("#frmTransaction");

			jHelper.resetFields(frm);
			jHelper.removejqCheckBox(frm);
			jHelper.jqCheckBox(frm);

			$("#FundId",frm).val(fundId);
			$("#CloseFundName",frm).val(fundName);
			$("input[type='hidden'][name='IsFirstClosing']",frm).val(false);

			var loading=$("#Loading",frm);
			loading.empty();
			$("#AddFundClose").dialog("open");
		}
	}
	,addFundClose: function () {
		try {
			var frm=$("#frmAddFundClose");
			var frmTransaction=$("#frmTransaction");
			var loading=$("#Loading",frm);
			var param=$(frm).serializeForm();
			var url="/Admin/UpdateFundClosing";
			loading.html(jHelper.savingHTML());
			$.post(url,param,function (data) {
				loading.empty();
				var arr=data.split("||");
				if(arr[0]!="True") {
					jAlert(data);
				} else {
					jAlert("Fund Close Added");

					$("#AddFundClose").dialog("close");

					$(investorCommitment.CurrentFundCloseId).val(arr[1]);
					$(investorCommitment.CurrentFundCloseName).val($("#Name",frm).val());

					jHelper.resetFields(frm);
				}
			});
		} catch(e) {
			jAlert(e);
		}
		return false;
	}
	,cancelFundClose: function () {
		$("#AddFundClose").dialog("close");
	}
	,editTS: function (transactionId) {
		if(transactionId>0) {
			var editTransaction=$("#EditTransaction");
			editTransaction.empty();
			editTransaction.html(jHelper.loadingHTML());
			$.getJSON("/Transaction/FindInvestorFundDetail/"+transactionId,function (data) {
				editTransaction.empty();
				$("#TransactionTemplate").tmpl(data).appendTo(editTransaction);
				jHelper.jqComboBox(editTransaction);

				$("#CounterPartyInvestor",editTransaction).autocomplete(
				{ source: "/Investor/FindOtherInvestors?investorId="+$("#InvestorId",editTransaction).val()
				,minLength: 1
				,autoFocus: true
				,select: function (event,ui) {
					$("#CounterPartyInvestorId",editTransaction).val(ui.item.id);
					investorCommitment.loadInvestorType(ui.item.id);
				}
				 ,appendTo: "body"
				 ,delay: 300
				});

			});
			$("#EditTransaction").dialog("open");
		}
	}
	,loadInvestorType: function (investorId) {
		var editTransaction=$("#EditTransaction");
		var FundId=$("#FundId",editTransaction).val();
		var url="/Transaction/InvestorType/?investorId="+investorId+"&fundId="+FundId;
		var disp_InvestorTypeId=$("#disp_InvestorTypeId",editTransaction).get(0);
		var InvestorTypeId=$("#InvestorTypeId",editTransaction).get(0);
		var InvestorTypeRow=$("#InvestorTypeRow",editTransaction).get(0);
		jHelper.jqComboBox(editTransaction);
		InvestorTypeRow.style.display="";
		InvestorTypeId.value=0;
		InvestorTypeId.style.display="none";
		disp_InvestorTypeId.style.display="none";
		disp_InvestorTypeId.innerHTML="";
		$(InvestorTypeId).combobox('destroy');
		$(InvestorTypeId).combobox('remove');
		if(investorId>0) {
			$.getJSON(url,function (data) {
				if(data.InvestorTypeId>0) {
					InvestorTypeId.value=data.InvestorTypeId;
					InvestorTypeId.style.display="none";
					disp_InvestorTypeId.innerHTML=InvestorTypeId.options[InvestorTypeId.selectedIndex].text;
					disp_InvestorTypeId.style.display="";
				} else {
					$(InvestorTypeId).combobox();
				}
			});
		}
	}
	,closeEditTransactionDialog: function (reload) {
		$("#editTransactionDialog").dialog('close');
		$("#EditCommitmentAmount").dialog('close');
		if(reload)
			investorCommitment.loadFundDetails();
	}
	,loadFundDetails: function () {
		$("#InvestmentList").flexReload();
	}
	,add: function (that) {
		var flexigrid=$("#InvestmentList");
		var row=$("#Row0",flexigrid).get(0);
		if(!row) {
			var tbody=$("tbody",flexigrid);
			var data={ "page": 0,"total": 0,"rows": [{ "cell": [0,0,"",0,"","","",0,"",0,investorCommitment.getInvestorId()]}] };
			$("#GridTemplate").tmpl(data).prependTo(tbody);
			var tr=$("tr:first",tbody);
			jHelper.jqCheckBox(tr);
			this.editRow(tr);
			$("#Add",tr).show();
			investorCommitment.setUpRow(tr);
		}
	}
	,onGridSuccess: function (t,g) {
		jHelper.checkValAttr(t);
		jHelper.jqCheckBox(t);
		$(window).resize();
	}
	,onInit: function (g) {
		//var data={ name: "Add Commitment" };
		//$("#AddButtonTemplate").tmpl(data).prependTo(g.pDiv);
		//		$(window).resize(function () {
		//			invEntityType.resizeGV(g);
		//		});
	}
	,onTemplate: function (tbody,data) {
		$("#GridTemplate").tmpl(data).appendTo(tbody);
	}
}