﻿var dealContact={
	add: function (that) {
		var flexigrid=$(that).parents(".flexigrid:first");
		var row=$("#Row0",flexigrid).get(0);
		if(!row) {
			var tbody=$(".middlec table tbody",flexigrid);
			var data={ "page": 0,"total": 0,"rows": [{ "cell": [0,"","","","","",""]}] };
			$("#GridTemplate").tmpl(data).prependTo(tbody);
			var tr=$("tr:first",tbody);
			jHelper.jqCheckBox($("#EditRow"+0));
			this.editRow(0);
			$("#Add",tr).show();
		}
	}
	,edit: function (img,id) {
		this.editRow(id);
	}
	,editRow: function (id) {
		$("td","#Row"+id).hide();
		$("td","#EditRow"+id).show();
	}
	,deleteRow: function (img,id) {
		if(confirm("Are you sure you want to delete this contact?")) {
			var tr=$(img).parents("tr:first");
			var imgsrc=img.src;
			$(img).attr("src",jHelper.getImagePath("ajax.jpg"));
			img.src=imgsrc;
			var dt=new Date();
			var url="/Admin/DeleteDealContact/"+id+"?t="+dt.getTime();
			$.get(url,function (data) {
				if(data!="") {
					jAlert(data);
				} else {
					tr.remove();
					$("#EditRow"+id).remove();
					jHelper.applyFlexGridClass($(".middlec:first"));
				}
			});
		}
	}
	,save: function (id) {
		try {
			var frm=$("#frm"+id);
			var param=$(frm).serializeForm();
			var url="/Admin/UpdateDealContact";
			var loading=$("#Loading",frm);
			loading.html(jHelper.savingHTML());
			$.post(url,param,function (data) {
				loading.empty();
				var arr=data.split("||");
				if(arr[0]!="True") {
					jAlert(data);
				} else {
					var id=$("#ContactId",frm).val();
					$.getJSON("/Admin/EditDealContact?_"+(new Date).getTime()+"&id="+arr[1],function (loadData) {
						var tr=$("#Row"+id);
						$("#EditRow"+id).remove();
						$("#GridTemplate").tmpl(loadData).insertAfter(tr);
						$(tr).remove();
						var newTR=$("#EditRow"+arr[1]);
						//jHelper.applyFlexGridClass($(".middlec:first"));
						jHelper.applyFlexEditGridClass($(".middlec:first"));
						jHelper.checkValAttr(newTR);
						jHelper.jqCheckBox(newTR);jHelper.gridEditRow(newTR);
					});
				}
			});
		} catch(e) { jAlert(e); }
		return false;
	}
	,editPassword: function (id) {
		$("#Password","#frm"+id).removeAttr("disabled");
		$("#ChangePassword","#frm"+id).val("true");
	}
	,cancelPassword: function (id) {
		$("#Password","#frm"+id).val("").attr("disabled","");
		$("#ChangePassword","#frm"+id).val("false");
	}
	,cacelEdit: function (id) {
		if(id>0) {
			$("td","#Row"+id).show();
		} else {
			$("#Row"+id).remove();
		}
		$("td","#EditRow"+id).hide();
	}
	,onGridSuccess: function (t,g) {
		jHelper.checkValAttr(t);
		jHelper.jqCheckBox(t);
		jHelper.gridEditRow(t);
	}
	,onInit: function (g) {
		var data={ name: "Add Deal Contact" };
		$("#AddButtonTemplate").tmpl(data).prependTo(g.pDiv);
	}
	,onTemplate: function (tbody,data) {
		$("#GridTemplate").tmpl(data).appendTo(tbody);
	}
	,resizeGV: function (g) {
		var admain=$(".admin-main");
		var bDivBox=$(g.bDivBox);
		bDivBox.css("height","auto");
		var ah=admain.height()-220;
		var h=bDivBox.height();
		if(h>ah) {
			bDivBox.height(ah);
		}
	}
}