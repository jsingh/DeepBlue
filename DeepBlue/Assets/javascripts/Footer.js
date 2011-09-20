var footer={
	show: function(body, foot){
		if($("tr","#" + body).length > 0) {
			$("#" + foot).show();
		}
		else{
			$("#" + foot).hide();
		}
	}
}