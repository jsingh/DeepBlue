var exportExcel={
	add: function (that) {
		var flexigrid=$(that).parents(".flexigrid:first");
		var row=$("#Row0",flexigrid).get(0);
		if(!row) {
			var tbody=$(".middlec table tbody",flexigrid);
			var data={ "page": 0,"total": 0,"rows": [{ "cell": [0,"",false]}] };
			$("#GridTemplate").tmpl(data).prependTo(tbody);
			var tr=$("tr:first",tbody);
			this.editRow(tr);
			$("#Add",tr).show();
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
	,exportExcel: function (id) {
		if(id<=4) {
			var width=300;var height=200;var left=(screen.availWidth/2)-(width/2);var top=(screen.availHeight/2)-(height/2);var features="width="+width+",height="+height+",left="+left+",top="+top+",location=no,menubar=no,toobar=no,scrollbars=yes,resizable=yes,status=yes";
			url="/Admin/ExportExcel?tableName=''&excelExportTypeId="+id;
			window.open(deepBlue.rootUrl+url,"exportexcel",features);
		}
	}
	,onSubmit: function (p) {
		p.params=null;
		p.params=new Array();
		var query=$("#Query").get(0);
		var tname="";
		if(query) { tname=query.value; }
		p.params[p.params.length]={ "name": "tableName","value": tname };
		return true;
	}
	,onGridSuccess: function (t,g) {
		jHelper.checkValAttr(t);
		
	}
	,onInit: function (g) {
		var data={ name: "Add Seller Type" };
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