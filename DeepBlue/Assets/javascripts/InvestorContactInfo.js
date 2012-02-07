var investorContactInfo={
	onGridSuccess: function (t,g) {
		jHelper.checkValAttr(t);
		jHelper.jqCheckBox(t);
		
	}
	,onInit: function (g) {
		var box=$(g.bDivBox).parents("#AddUnderlyingFund");
		var data={};
		$("#AddContactButtonTemplate").tmpl(data).prependTo(g.pDiv);
	}
	,onTemplate: function (tbody,data) {
		$("#ContactInfoGridTemplate").tmpl(data).appendTo(tbody);
	}
	,add: function (that) {
		var flexigrid=$(that).parents(".flexigrid:first");
		var row=$("#Row0",flexigrid).get(0);
		if(!row) {
			var tbody=$(".middlec table tbody",flexigrid);
			var data={ "page": 0,"total": 0,"rows": [{ "cell": [0,"","","","","","","","","","",""]}] };
			$("#EditRow"+0).remove();
			$("#ContactInfoGridTemplate").tmpl(data).prependTo(tbody);
			var tr=$("tr:first",tbody);
			jHelper.jqCheckBox($("#EditRow"+0));
			this.editRow(0,flexigrid);
			$("#Add",tr).show();
		}
	}
	,editRow: function (id,grid) {
		$("td","#Row"+id,grid).hide();
		$("td","#EditRow"+id,grid).show();
	}
}