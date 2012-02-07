var log={
	onGridSuccess: function (t,g) {
		jHelper.checkValAttr(t);
		jHelper.jqComboBox(t);
		jHelper.jqCheckBox(t);
		
	}
	,onInit: function (g) {
		var data={ name: "Add Document Type" };
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