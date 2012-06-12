﻿$(document).ready(function () {
	var target=$("#ExcelImport");
	target.dialog({
		title: "Import Deal",
		autoOpen: false,
		width: "auto",
		modal: true,
		position: 'middle',
		autoResize: true,
		open: function () {
			var data=[{ name: ""}];
			target.empty();
			$("#ExcelImprtTemplate").tmpl(data).appendTo(target);
			jHelper.resizeDialog();
			target
			.css("padding","0px")
			;
		}
	});
});
var importDealCloseExcel={
	defaultID: "DropFileUpload"
	,uploadExcel: function () {
		try {
			var loading=$("#SpnUELoading");
			loading.html(jHelper.uploadingHTML());
			$.ajaxFileUpload(
			{
				url: '/Admin/Upload',
				secureuri: false,
				formId: 'frmUploadExcel',
				dataType: 'json',
				success: function (data,status) {
					loading.empty();
					if($.trim(data.Result)!="") {
						jAlert(data.Result);
					} else {
						importDealCloseExcel.importRows(data.FileName);
					}
				},
				error: function (data,status,e) {
					jAlert(data.msg+","+status+","+e);
				}
			}
		);
		} catch(e) {
			jAlert(e);
		}
	}
	,selectTab: function (type,lnk) {
		$(".section-tab").removeClass("section-tab-sel");
		var DealDetailBox=$("#DealDetailBox");
		var DealExpenseBox=$("#DealExpenseBox");
		var DealUFBox=$("#DealUFBox");
		var DealUDBox=$("#DealUDBox");
		$(lnk).addClass("section-tab-sel");
		DealDetailBox.hide();
		DealExpenseBox.hide();
		DealUFBox.hide();
		DealUDBox.hide();
		$(".tablnk").removeClass("select");
		switch(type) {
			case "DD": DealDetailBox.show();break;
			case "DE": DealExpenseBox.show();break;
			case "DUF": DealUFBox.show();break;
			case "DUD": DealUDBox.show();break;
		}
	}
	,lastExcelData: null
	,selectExcelTab: function (ddl) {
		var section=$(ddl).parents(".dealimportsection:first");
		$(".ui-autocomplete-input",section).val("");
		$("select:not(.ddltable)",section).each(function () {
			var ddl=this;
			ddl.options.length=null;
			$(ddl).combobox("destroy");
		});
		$.each(importDealCloseExcel.lastExcelData.Tables,function (i,item) {
			if(item.TableName==ddl.value) {
				$("select:not(.ddltable)",section).each(function () {
					var ddl=this;
					ddl.options.length=null;
					var listItem=new Option("--Select Excel Field--"," ",false,false);
					ddl.options[ddl.options.length]=listItem;
					$.each(item.Columns,function (i,name) {
						listItem=new Option(name,name,false,false);
						if(ddl.name.toLowerCase()==name.toLowerCase()) {
							listItem.selected=true;
						}
						ddl.options[ddl.options.length]=listItem;
					});
				});
			}
		});
		jHelper.jqComboBox(section);
	}
	,importRows: function (fileName) {
		var importBox=$(".import-box","#ExcelImport");
		importBox.hide();
		var param=[{ name: "FileName",value: fileName}];
		var target=$("#ImportExcel","#ExcelImport");
		target.css({
			"width": "300px",
			"height": "100px"
		});
		target.empty();
		target.html("<center>"+jHelper.loadingHTML()+"</center>");
		$.post("/Deal/ImportExcel",param,function (data) {
			importDealCloseExcel.lastExcelData=null;
			target.css({
				"width": "auto",
				"height": "auto"
			});
			if($.trim(data.Result)!="") {
				jAlert(data.Result);
				importBox.show();
				target.empty();
			} else {
				target.empty();
				$("#ImportExcelTemplate").tmpl(data).appendTo(target);
				importDealCloseExcel.lastExcelData=data;
				$(".ddltable",target).each(function () {
					var ddl=this;
					ddl.options.length=null;
					var listItem=new Option("--Select Excel Tab--"," ",false,false);
					ddl.options[ddl.options.length]=listItem;
					$.each(data.Tables,function (i,item) {
						listItem=new Option(item.TableName,item.TableName,false,false);
						var exceltabname=$(ddl).attr("exceltabname");
						if(exceltabname==undefined) {
							exceltabname="";
						}
						if(exceltabname.toLowerCase()==item.TableName.toLowerCase()) {
							listItem.selected=true;
						}
						ddl.options[ddl.options.length]=listItem;
					});
					$(ddl).change();
				});
				jHelper.jqComboBox(target);
				jHelper.resizeDialog();
			}
		},"JSON");
	}
	,import: function (btn) {
		$(btn).hide();
		importDealCloseExcel.importDealUF();
	}
	,expertErrorExcel: function (sessionKey,tableName) {
		var width=300;var height=200;var left=(screen.availWidth/2)-(width/2);var top=(screen.availHeight/2)-(height/2);var features="width="+width+",height="+height+",left="+left+",top="+top+",location=no,menubar=no,toobar=no,scrollbars=yes,resizable=yes,status=yes";
		window.open("/Deal/GetImportErrorExcel?sessionKey="+sessionKey+"&tableName="+tableName,tableName,features);
	}
	,importDealUF: function () {
		$("#DealUFTab").click();
		var thread=new ImportExcel();
		thread.box=$("#DealUFBox");
		thread.url="/Deal/ImportDealCloseUnderlyingFundExcel";
		thread.onComplete=function (sessionKey,tableName) {
			if($.trim(tableName)!="") {
				var statusbox=$(".statusbox",thread.box);
				var spnerrorexcel=$("#spnerrorexcel",statusbox);
				spnerrorexcel.html("<a href='#'>Error Excel</a>");
				$("a",spnerrorexcel).click(function () {
					importDealCloseExcel.expertErrorExcel(sessionKey,tableName);
				});
			}
			importDealCloseExcel.importDealUD();
		}
		thread.import();
	}
	,importDealUD: function () {
		$("#DealUDTab").click();
		var thread=new ImportExcel();
		thread.box=$("#DealUDBox");
		thread.url="/Deal/ImportDealCloseUnderlyingDirectExcel";
		thread.onComplete=function (sessionKey,tableName) {
			if($.trim(tableName)!="") {
				var statusbox=$(".statusbox",thread.box);
				var spnerrorexcel=$("#spnerrorexcel",statusbox);
				spnerrorexcel.html("<a href='#'>Error Excel</a>");
				$("a",spnerrorexcel).click(function () {
					importDealCloseExcel.expertErrorExcel(sessionKey,tableName);
				});
			}
		}
		thread.import();
	}
}

function ImportExcel() {
	this.pageindex=1;
	this.pagesize=10;
	this.onComplete=null;
	this.url="";
	this.box=null;
	this.import=function () {
		var that=this;
		$(".dealimportsection").hide();
		that.box.show();
		var formbox=$(".formbox",that.box);
		var statusbox=$(".statusbox",that.box);
		formbox.hide();
		statusbox.show();
		var frm=$("#frm",that.box);
		var tablename=$("[exceltabname]",frm).val();
		$.each(importDealCloseExcel.lastExcelData.Tables,function (i,item) {
			if(item.TableName==tablename) {
				$("#SessionKey",frm).val(item.SessionKey);
				$("#TotalRows",frm).val(item.TotalRows);
			}
		});
		var params=frm.serializeArray();
		params[params.length]={ "name": "PageSize","value": that.pagesize };
		params[params.length]={ "name": "PageIndex","value": that.pageindex };
		if(that.pageindex==1) {
			var data=[{ TotalRows: $("#TotalRows",frm).val(),CompletedRows: 0,Percent: 0,SuccessRows: 0,ErrorRows: 0}];
			that.setStatus(statusbox,data);
		}
		if($.trim(tablename)=="") {
			var spnerrorexcel=$("#spnerrorexcel",statusbox);
			spnerrorexcel.html("Excel Tab is required");
			if(that.onComplete) {
				that.onComplete($("#SessionKey",frm).val(),tablename);
			}
			return;
		}
		$.post(this.url,params,function (data) {
			if(data.Result!="") {
				jAlert(data.Result);
			} else {
				that.setStatus(statusbox,data);
				if(data.TotalPages>=(data.PageIndex+1)) {
					that.pageindex++;
					that.import();
				} else {
					if(that.onComplete) {
						that.onComplete($("#SessionKey",frm).val(),tablename);
					}
				}
			}
		},"JSON");
	};

	this.setStatus=function (statusbox,data) {
		statusbox.empty();
		data.Percent=parseInt((data.CompletedRows/data.TotalRows)*100);
		$("#ImportExcelResultTemplate").tmpl(data).appendTo(statusbox);
	}
}


 