var underlyingFundContact={
	load: function (ufid) {
		var box=$("#Edit"+ufid);
		var p=new Array();
		p[p.length]={ "name": "UnderlyingFundId","value": ufid };
		$("#ContactList",box).flexigrid({
			usepager: true
			,url: "/Deal/UnderlyingFundContactList"
			,params: p
			,resizeWidth: true
			,method: "GET"
			,sortname: "ContactName"
			,sortorder: "desc"
			,autoload: true
			,height: 200
			,resizeWidth: false
			,useBoxStyle: false
			,onSuccess: underlyingFundContact.onGridSuccess
			,onRowClick: underlyingFundContact.onRowClick
			,onInit: underlyingFundContact.onInit
			,onTemplate: underlyingFundContact.onTemplate
			,tableName: "Contact"
		});
	}
	,add: function (that,ufid) {
		var flexigrid=$(that).parents(".flexigrid:first");
		var row=$("#Row0",flexigrid).get(0);
		if(!row) {
			var tbody=$(".middlec table tbody",flexigrid);
			var data={ "page": 0,"total": 0,"rows": [{ "cell": [0,ufid,"","","","","","","",""]}] };
			$("#EditRow"+0).remove();
			$("#ContactGridTemplate").tmpl(data).prependTo(tbody);
			var tr=$("tr:first",tbody);
			jHelper.jqCheckBox($("#EditRow"+0));
			this.editRow(0,flexigrid);
			$("#Add",tr).show();
			underlyingFundContact.trimData();
		}
	}
	,edit: function (img,id) {
		var grid=$(img).parents(".flexigrid:first");
		this.editRow(id,grid);
	}
	,editRow: function (id,grid) {
		$("td","#Row"+id,grid).hide();
		$("td","#EditRow"+id,grid).show();
	}
	,deleteRow: function (img,id) {
		var grid=$(img).parents(".flexigrid:first");
		if(confirm("Are you sure you want to delete this contact?")) {
			var tr=$(img).parents("tr:first");
			var imgsrc=img.src;
			$(img).attr("src","/Assets/images/ajax.jpg");
			img.src=imgsrc;
			var dt=new Date();
			var url="/Deal/DeleteUnderlyingFundContact/"+id+"?t="+dt.getTime();
			$.get(url,function (data) {
				if(data!="") {
					jAlert(data);
				} else {
					tr.remove();
					$("#EditRow"+id,grid).remove();
					jHelper.applyFlexEditGridClass($(".middlec:first"));
				}
			});
		}
	}
	,save: function (img,id) {
		try {
			var frm=$(img).parents("form:first");
			var frmUnderlyingFund=$(frm).parents(".section-det:first");
			var ufid=$("#UnderlyingFundId",frm).val();
			underlyingFund.tempSave=false;
			underlyingFund.onAfterUnderlyingFundSave=null;
			if(ufid>0) {
				var param=$(frm).serializeForm();
				var url="/Deal/CreateUnderlyingFundContact";
				var loading=$("#Loading",frm);
				loading.html(jHelper.savingHTML());

				$.post(url,param,function (data) {
					loading.empty();
					var arr=data.split("||");
					if($.trim(arr[0])=="Error") {
						jAlert(arr[1]);
					} else {
						jAlert("Underlying Fund Contact Saved");
						var box=$("#Edit"+ufid);
						$.getJSON("/Deal/EditUnderlyingFundContact?_"+(new Date).getTime()+"&id="+arr[0],function (loadData) {
							var tbl=$("#ContactList",box);
							var tr=$("#Row"+id,tbl);
							$("#EditRow"+id,tbl).remove();
							$("#ContactGridTemplate").tmpl(loadData).insertAfter(tr);
							$(tr).remove();
							var newTR=$("#EditRow"+arr[0]);
							jHelper.applyFlexEditGridClass($(".middlec:first"));
							jHelper.checkValAttr(newTR);
							underlyingFundContact.cacelEdit(arr[0]);
							//jHelper.jqCheckBox(newTR);
						});
					}
				});
			} else {
				underlyingFund.tempSave=true;
				underlyingFund.onAfterUnderlyingFundSave=function (ufid) {
					$("#UnderlyingFundId",frm).val(ufid);
					underlyingFundContact.save(img,id);
				}
				$("#btnSave",frmUnderlyingFund).click();
			}
		} catch(e) { jAlert(e); }
		return false;
	}
	,editWebPassword: function (id) {
		var frm=$("#frm"+id);
		$("#WebPassword",frm).removeAttr("disabled");
		$("#ChangeWebPassword",frm).val("true");
	}
	,cancelWebPassword: function (id) {
		var frm=$("#frm"+id);
		$("#WebPassword",frm).val("").attr("disabled","disabled");
		$("#ChangeWebPassword",frm).val("false");
		$("#EditWebPassword",frm).show();
		$("#CancelWebPassword",frm).show();
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
		$(window).resize();
	}
	,onInit: function (g) {
		var box=$(g.bDivBox).parents("#AddUnderlyingFund");
		var ufid=$("#UnderlyingFundId",box);
		var data={ UnderlyingFundId: ufid };
		$("#AddContactButtonTemplate").tmpl(data).prependTo(g.pDiv);
	}
	,onTemplate: function (tbody,data) {
		$("#ContactGridTemplate").tmpl(data).appendTo(tbody);
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
	,trimData: function () {
		$("#Address","#ContactList").val($.trim($("#Address","#ContactList").val()));
		$("#ContactNotes","#ContactList").val($.trim($("#ContactNotes","#ContactList").val()));
	}
}