var Member={
	init: function () {
		$(document).ready(function () {
			$("#home_menu").get(0).className="tab_unsel";
			$("#member_menu").get(0).className="tab_sel";
		});
	},createAccount: function () {
		var AccountLength=parseInt($("#AccountLength").val());
		var AccountInfo=document.createElement("div");
		AccountInfo.id="AccountInfo_"+(AccountLength+1);
		AccountInfo.className="accountinfo";
		AccountInfo.innerHTML=$("#AccountInfo").html().replace(/1_/g,(AccountLength+1)+"_");
		$("input",AccountInfo).val("");
		$("select",AccountInfo).val("");
		$("#AccountLength").val(AccountLength+1);
		//$("#index",AccountInfo).html(AccountLength+1);
		$(".delete",AccountInfo).css("display","block");
		$("#AccountInfoBox").append(AccountInfo);
	},deleteAccount: function (that) {
		if(confirm("Are you sure you want to delete this Account?")) {
			var AccountInfo=$(that).parents(".accountinfo").get(0);
			var AccountLength=parseInt($("#AccountLength").val());
			$("#AccountLength").val(AccountLength-1);
			$(AccountInfo).remove();
		}
	},createContact: function () {
		var ContactLength=parseInt($("#ContactLength").val());
		var ContactInfo=document.createElement("div");
		ContactInfo.id="ContactInfo_"+(ContactLength+1);
		ContactInfo.className="contactinfo";
		ContactInfo.innerHTML=$("#ContactInfo").html().replace(/1_/g,(ContactLength+1)+"_");
		$("input",ContactInfo).val("");
		$("select",ContactInfo).val("");
		$("#ContactLength").val(ContactLength+1);
		//$("#index",ContactInfo).html(ContactLength+1);
		$(".delete",ContactInfo).css("display","block");
		$(".add",ContactInfo).remove();
		$("#ContactInfoBox").append(ContactInfo);
	},deleteContact: function (that) {
		if(confirm("Are you sure you want to delete this Contact?")) {
			var ContactInfo=$(that).parents(".contactinfo").get(0);
			var ContactLength=parseInt($("#ContactLength").val());
			$("#ContactLength").val(ContactLength-1);
			$(ContactInfo).remove();
		}
	}
};