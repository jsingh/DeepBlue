var fundClosing={
	pageLoad: false
	,init: function () {
		$("document").ready(function () {
			fundClosing.resizeIframe();
			fundClosing.pageLoad=true;
			$("body").css("overflow","hidden");
		});
	}
	,add: function (id) {
		var dt=new Date();
		var url="/Admin/EditFundClosing/"+id+"?t="+dt.getTime();
		jHelper.createDialog(url,{
			title: "Fund Closing",
			autoOpen: true,
			width: 380,
			modal: true,
			position: 'middle',
			autoResize: true
		});
	}
	,deleteFundClosing: function (id,img) {
		if(confirm("Are you sure you want to delete this fund closing?")) {
			var dt=new Date();
			var url="/Admin/DeleteFundClosing/"+id+"?t="+dt.getTime();
			$.get(url,function (data) {
				if(data!="") {
					alert(data);
				} else {
					$("#FundClosingList").flexReload();
				}
			});
		}
	}
	,changeFund: function (fundddl) {
		var dt=new Date();
		var url="/Admin/FundClosingNameAvailable/?Name="+$("#Name").val()+"&FundClosingID="+$("#FundClosingID").val()+"&FundID="+$("#FundID").val()+"&t="+dt.getTime();
		$.get(url,function (data) {
			if(data!="") {
				alert(data);
				$("#Name_validationMessage").html(data);
			} else {
				$("#Name_validationMessage").html("");
			}
		});
	}
	,showDate: function (dateText,inst) {
		fundClosing.resizeIframe(130);
	}
	,closeDate: function () {
		fundClosing.resizeIframe(-130);
	}
	,resizeIframe: function (h) {
		var theFrame=$("#iframe_modal",parent.document.body);
		if(theFrame) {
			var bdyHeight=$("body").height();
			if(parseInt(h)>0&&this.pageLoad) {
				bdyHeight=bdyHeight+h;
			}
			theFrame.height(bdyHeight);
		}
	}
	,onSubmit: function (formId) {
		return jHelper.formSubmit(formId);
	}
	,onRowBound: function (tr,data) {
		var lastcell=$("td:last div",tr);
		lastcell.html("<img id='Edit' class='gbutton' src='/Assets/images/Edit.png'/>&nbsp;&nbsp;&nbsp;<img id='Delete' src='/Assets/images/largedel.png'/>");
		$("#Edit",lastcell).click(function () { fundClosing.add(data.cell[0]); });
		$("#Delete",lastcell).click(function () { fundClosing.deleteFundClosing(data.cell[0],this); });
		$("td:not(:last)",tr).click(function () { fundClosing.add(data.cell[0]); });
	}
	,closeDialog: function (reload) {
		$("#addDialog").dialog('close');
		if(reload==true) {
			$("#FundClosingList").flexReload();
		}
	}
	,onCreateFundClosingBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	}
	,onCreateFundClosingSuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(jQuery.trim(UpdateTargetId.html())!="True") {
			alert(UpdateTargetId.html())
		} else {
			parent.fundClosing.closeDialog(true);
		}
	}
}