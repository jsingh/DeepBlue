(function ($) {
	$.addAjaxTable=function (t,p) {
		if(t.grid) return false; //return if already exist	
		// apply default properties
		p=$.extend({
			url: false, //ajax url
			method: 'POST', // data sending method
			dataType: 'json', // type of data loaded
			errormsg: 'Connection Error',
			page: 1, //current page
			total: 1, //total pages
			rp: 20, // results per page
			rpOptions: [20,50,100],
			title: false,
			procmsg: 'Loading...',
			nomsg: 'No items',
			autoload: true,
			sortname: '',
			sortorder: 'asc',
			appendExistRows: false,
			onChangeSort: false,
			onSuccess: false,
			onRowClick: false,
			onRowBound: false,
			onSubmit: false // using a custom populate function
		},p);
		$(t)
		.show() //show if hidden
		;
		//create grid class
		var g={
			addData: function (data) { //parse data
				var tbody;
				if(p.appendExistRows) {
					tbody=$("tbody",t).get(0);
				} else {
					tbody=document.createElement('tbody');

				}
				if(tbody) {
					if(p.dataType=='json') {
						$.each
					(
					 data.rows,
					 function (i,row) {
					 	var tr=document.createElement('tr');
					 	if(i%2) tr.className='erow';
					 	tr.id='row'+i;
					 	var i=0;
					 	var celllength=$("thead tr th",t).length;
					 	$("thead tr:first th",t).each(function () {
					 		var td=document.createElement('td');
					 		if(row.cell.length>i) {
					 			td.innerHTML=row.cell[i];
					 		}
					 		$(td).css({ "width": this.style.width,"display": this.style.display });
					 		$(tr).append(td);
					 		td=null;
					 		i++;
					 	});
					 	$(tbody).append(tr);
					 	if(p.onRowBound) { p.onRowBound(tr,row); }
					 	if(p.onRowClick) { $(tr).click(function () { p.onRowClick(row,tr); }); }
					 }
					);
					}
					if(p.appendExistRows==false) {
						$("tbody",t).remove();
						$("thead",t).after(tbody);
					}
					if(p.onSuccess) { p.onSuccess(t); }
				}
			}
			,changeSort: function (th) { //change sortorder
				try {
					if(p.sortname==$(th).attr('sortname')) {
						if(p.sortorder=='asc') p.sortorder='desc';
						else p.sortorder='asc';
					}
					$(th).addClass('sorted').siblings().removeClass('sorted');
					$('.sdesc',t).removeClass('sdesc');
					$('.sasc',t).removeClass('sasc');
					$(th).addClass('s'+p.sortorder);
					p.sortname=$(th).attr('sortname');
					if(p.onChangeSort) {
						p.onChangeSort(t,p);
						this.populate();
					} else {
						this.populate();
					}
				} catch(e) { alert(e); }
			}
			,populate: function () { //get latest data 
				if(p.onSubmit) {
					var gh=p.onSubmit(p);
					if(!gh) return false;
				}
				this.loading=true;
				if(!p.url) return false;
				if(!p.newp) p.newp=1;
				if(p.page>p.pages) p.page=p.pages;
				var dt=new Date();
				var param=[
					 { name: 'pageIndex',value: p.newp }
					,{ name: 'pageSize',value: p.rp }
					,{ name: 'sortName',value: p.sortname }
					,{ name: 'sortOrder',value: p.sortorder }
				];
				if(p.params) {
					for(var pi=0;pi<p.params.length;pi++) param[param.length]=p.params[pi];
				}
				param[param.length]={ name: "t",value: dt.getTime() };
				$.ajax({
					type: p.method,
					url: p.url,
					data: param,
					dataType: p.dataType,
					success: function (data) { g.addData(data); },
					error: function (data) { try { if(p.onError) p.onError(data); } catch(e) { } }
				});
			}
		};
		//setup thead			
		$('thead tr:first th',t).each
		(
			function () {
				if($(this).attr('sortname')) {
					$(this).css("cursor","pointer").click(function (e) { g.changeSort(this); });
					if($(this).attr('sortname')==p.sortname) {
						this.className='sorted';
						if(p.sortorder=='') {
							p.sortorder='asc';
						}
						$("span",this).addClass('s'+p.sortorder);
					}
				}
			}
		);
		if(p.autoload) {
			g.populate();
		}
		t.grid=g;
		t.p=p;
		return t;
	};

	var docloaded=false;
	$(document).ready(function () { docloaded=true });
	$.fn.ajaxTable=function (p) {
		return this.each(function () {
			if(!docloaded) {
				$(this).hide();
				var t=this;
				$(document).ready
					(
						function () {
							$.addAjaxTable(t,p);
						}
					);
			} else {
				$.addAjaxTable(this,p);
			}
		});
	}; //end ajaxTable

	$.fn.ajaxTableReload=function (p) { // function to reload grid
		return this.each(function () {
			if(this.grid&&this.p.url) {
				this.grid.loading=false;
				this.grid.populate();
			}
		});
	}; //end ajaxTableReload
	$.fn.ajaxTableOptions=function (p) { //function to update general options
		return this.each(function () {
			if(this.grid) $.extend(this.p,p);
		});
	}; //end ajaxTableOptions
	$.fn.ajaxTableAddData=function (data) { // function to add data to grid
		return this.each(function () {
			if(this.grid) this.grid.addData(data);
		});
	};
})(jQuery);