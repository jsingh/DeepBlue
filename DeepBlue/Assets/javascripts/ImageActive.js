$(function () {
	$("input:image").live('mousedown',function(){
		this.src=this.src.replace("_active.png","_clicked.png");
	}).live('mouseup',function(){
		this.src=this.src.replace("_clicked.png","_active.png");
	});
	
	$("img").live('mousedown',function(){
		this.src=this.src.replace("_active.png","_clicked.png");
	}).live('mouseup',function(){
		this.src=this.src.replace("_clicked.png","_active.png");
	});
});