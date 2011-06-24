issuer.loadEquityData=function (data) {
	try {
		var tbody=$("#tbodyEquity");
		var tr=$("#Equity_"+data.EquityId,tbody);
		if(!(tr.get(0))) {
			$("#EquityRowTemplate").tmpl(data).appendTo("#tbodyEquity");
		} else {
			$("#EquityRowTemplate").tmpl(data).insertAfter(tr);
			tr.remove();
		}
		tr=$("#Equity_"+data.EquityId,tbody);
		issuer.selectValue(tr);
		issuer.checkboxValue(tr);
		issuer.setIndex($("#tblEquity"));jHelper.resizeIframe();
	} catch(e) { alert(e); }
};
issuer.loadEquity=function (id) {
	var dt=new Date();
	var url="/Issuer/FindEquity?equityId="+id+"&t="+dt.getTime();
	$.getJSON(url,function (data) {
		issuer.loadEquityData(data);
	});
};
issuer.addEquity=function (img) {
	var tr=$(img).parents("tr:first");
	issuer.saveEquity(tr);
	jHelper.resizeIframe();
};
issuer.editEquity=function (img) {
	var tr=$(img).parents("tr:first");
	if(img.src.indexOf('save.png')> -1) {
		issuer.saveEquity(tr);
	} else {
		img.src="/Assets/images/save.png";
		issuer.showElements(tr);
	}
	jHelper.resizeIframe();
};
issuer.saveEquity=function (tr) {
	var issuerId=issuer.getIssuerId();
	var spnAjax=$("#spnAjax",tr).show();
	spnAjax.show();
	if(issuerId>0) {
		var param=jHelper.serialize(tr);
		param[param.length]={ name: "IssuerId",value: issuer.getIssuerId() };
		var url="/Issuer/CreateEquity";
		$.post(url,param,function (data) {
			spnAjax.hide();
			if(data.indexOf("True||")> -1) {
				var tr=$("tfoot tr:first","#tblEquity");
				$(":input[type='text']",tr).val("");
				$("select",tr).val(0);
				$(":input[type='checkbox']",tr).each(function () { this.checked=false; });
				var id=data.split("||")[1];
				issuer.loadEquity(id);
				if(issuer.onCloseDialog) {
					issuer.onCloseDialog();
				}
			} else {
				alert(data);
			}
		});
	} else {
		spnAjax.hide();
		issuer.onIssuerSuccess=null;
		issuer.onIssuerSuccess=function () { issuer.saveEquity(tr); }
		$("#Save").click();
	}
};
issuer.deleteEquity=function (id,img) {
	if(confirm("Are you sure you want to delete this equity?")) {
		var tr=$(img).parents("tr:first");
		var url="/Issuer/DeleteEquity/"+id;
		$.get(url,function (data) {
			tr.remove();
			issuer.setIndex($("#tblEquity"));
		});
	}
};
