﻿(function ($) {
	$.addFlex=function (t,p) {
		if(t.grid) return false; //return if already exist	
		// apply default properties
		p=$.extend({
			height: 0, //default height
			width: 'auto', //auto width
			striped: true, //apply odd even stripes
			novstripe: false,
			minwidth: 30, //min width of columns
			minheight: 80, //min height of columns
			resizable: false, //resizable table
			url: false, //ajax url
			method: 'POST', // data sending method
			dataType: 'json', // type of data loaded
			errormsg: 'Connection Error',
			usepager: false, //
			nowrap: true, //
			page: 1, //current page
			total: 1, //total pages
			useRp: true, //use the results per page select box
			rp: 20, // results per page
			rpOptions: [20,50,100],
			title: false,
			pagestat: 'Displaying {from} to {to} of {total} items',
			procmsg: 'Loading...',
			query: '',
			qtype: '',
			nomsg: 'No items',
			minColToggle: 1, //minimum allowed column to be hidden
			showToggleBtn: true, //show or hide column toggle popup
			hideOnSubmit: true,
			autoload: true,
			blockOpacity: 0.5,
			sortname: '',
			sortorder: '',
			onToggleCol: false,
			onChangeSort: false,
			onSuccess: false,
			onSubmit: false // using a custom populate function
		},p);
		$(t)
		.show() //show if hidden
		;
		//create grid class
		var g={
			hset: {},
			addData: function (data) { //parse data
				if(p.preProcess)
					data=p.preProcess(data);

				$('.pReload',this.pDiv).removeClass('loading');
				this.loading=false;
				if(!data) {
					$('.pPageStat',this.pDiv).html(p.errormsg);
					return false;
				}
				if(p.dataType=='xml')
					p.total= +$('rows total',data).text();
				else
					p.total=data.total;

				if(p.total==0) {
					$('tr, a, td, div',t).unbind();
					$(t).empty();
					p.pages=1;
					p.page=1;
					this.buildpager();
					$('.pPageStat',this.pDiv).html(p.nomsg);
					return false;
				}
				p.pages=Math.ceil(p.total/p.rp);
				if(p.dataType=='xml')
					p.page= +$('rows page',data).text();
				else
					p.page=data.page;

				this.buildpager();
				//build new body
				var tbody=document.createElement('tbody');
				if(p.dataType=='json') {
					$.each
					(
					 data.rows,
					 function (i,row) {
					 	var tr=document.createElement('tr');
					 	if(i%2&&p.striped) tr.className='erow';
					 	tr.id='row'+i;
					 	var i=0;
					 	$("thead tr th",g.hDiv).each(function () {
					 		var td=document.createElement('td');
					 		var div=document.createElement('div');
					 		div.innerHTML=row.cell[i];
					 		$(td).css({ "display": this.style.display });
					 		$(td).css({ "width": this.style.width });
					 		$(div).css({ "width": "100%" });
					 		//$(div).css({ "width": this.style.width,"display": this.style.display });
					 		//$(td).css({ "width" : this.style.width, "display" : this.style.display });
					 		$(div).css("text-align",$(this).attr("align"));
					 		$(td).append(div);
					 		$(tr).append(td);
					 		td=null;
					 		i++;
					 	});
					 	$(tbody).append(tr);
					 	tr=null;
					 }
					);
				}
				$('tr',t).unbind();
				$(t).empty();
				$(t).append(tbody);
				tbody=null;data=null;i=null;
				if(parseInt(p.height)<=0) {
					$(t).addClass("tblborder");
				} else {
					if($(g.bDiv).height()<$(g.bDivBox).height()) {
						$(g.bDiv).parent().removeClass("bDivMain").addClass("bDivMainNoborder");
						$(t).addClass("tblborder");
					} else {
						$(t).removeClass("tblborder");
						$(g.bDiv).parent().removeClass("bDivMainNoborder").addClass("bDivMain");
					}
				}
				if(p.onSuccess) p.onSuccess();
				if(p.hideOnSubmit) $(g.block).remove(); //$(t).show();
				this.hDiv.scrollLeft=this.bDiv.scrollLeft;
				if($.browser.opera) $(t).css('visibility','visible');
			},
			changeSort: function (th) { //change sortorder
				if(this.loading) return true;
				$(g.nDiv).hide();$(g.nBtn).hide();
				if(p.sortname==$(th).attr('sortname')) {
					if(p.sortorder=='asc') p.sortorder='desc';
					else p.sortorder='asc';
				}
				$(th).addClass('sorted').siblings().removeClass('sorted');
				$('.sdesc',this.hDiv).removeClass('sdesc');
				$('.sasc',this.hDiv).removeClass('sasc');
				$('div span',th).addClass('s'+p.sortorder);
				p.sortname=$(th).attr('sortname');
				if(p.onChangeSort)
					p.onChangeSort(p.sortname,p.sortorder);
				else
					this.populate();
			},
			buildpager: function () { //rebuild pager based on new properties
				$('.pcontrol input',this.pDiv).val(p.page);
				$('.pcontrol span',this.pDiv).html(p.pages);
				var r1=(p.page-1)*p.rp+1;
				var r2=r1+p.rp-1;
				if(p.total<r2) r2=p.total;
				var stat=p.pagestat;
				stat=stat.replace(/{from}/,r1);
				stat=stat.replace(/{to}/,r2);
				stat=stat.replace(/{total}/,p.total);
				$('.pPageStat',this.pDiv).html(stat);
			},
			populate: function () { //get latest data
				if(this.loading) return true;
				if(p.onSubmit) {
					var gh=p.onSubmit();
					if(!gh) return false;
				}
				this.loading=true;
				if(!p.url) return false;
				$('.pPageStat',this.pDiv).html(p.procmsg);
				$('.pReload',this.pDiv).addClass('loading');
				$(g.block).css({ top: g.bDiv.offsetTop });
				if(p.hideOnSubmit) {
					$(g.block).height($(g.bDiv).height());
					$(this.gDiv).prepend(g.block); //$(t).hide();
				}
				if($.browser.opera) $(t).css('visibility','hidden');
				if(!p.newp) p.newp=1;
				if(p.page>p.pages) p.page=p.pages;
				//var param = {page:p.newp, rp: p.rp, sortname: p.sortname, sortorder: p.sortorder, query: p.query, qtype: p.qtype};
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
			},
			changePage: function (ctype) { //change page
				if(this.loading) return true;
				switch(ctype) {
					case 'first': p.newp=1;break;
					case 'prev': if(p.page>1) p.newp=parseInt(p.page)-1;break;
					case 'next': if(p.page<p.pages) p.newp=parseInt(p.page)+1;break;
					case 'last': p.newp=p.pages;break;
					case 'input':
						var nv=parseInt($('.pcontrol input',this.pDiv).val());
						if(isNaN(nv)) nv=1;
						if(nv<1) nv=1;
						else if(nv>p.pages) nv=p.pages;
						$('.pcontrol input',this.pDiv).val(nv);
						p.newp=nv;
						break;
				}
				if(p.newp==p.page) return false;
				if(p.onChangePage)
					p.onChangePage(p.newp);
				else
					this.populate();
			},
			pager: 0
		};
		//init divs
		g.gDiv=document.createElement('div'); //create global container
		g.hDiv=document.createElement('div'); //create header container
		g.bDivBox=document.createElement('div');
		g.bDiv=document.createElement('div'); //create body container
		g.block=document.createElement('div'); //creat blocker

		if(p.usepager) g.pDiv=document.createElement('div'); //create pager container
		g.hTable=document.createElement("table");
		//set gDiv
		g.gDiv.className='flexigrid';
		if(p.width!='auto') g.gDiv.style.width=p.width+'px';
		//add conditional classes
		if($.browser.msie)
			$(g.gDiv).addClass('ie');

		if(p.novstripe)
			$(g.gDiv).addClass('novstripe');

		$(t).before(g.gDiv);
		$(g.gDiv)
		.append(t)
		;
		//set hDiv
		g.hDiv.className='hDiv';
		$(t).before(g.hDiv);
		//set hTable
		g.hTable.cellPadding=0;
		g.hTable.cellSpacing=0;
		g.hTable.style.width="100%";
		t.style.width="100%";
		$(g.hTable).append($("thead",t));
		$(g.hDiv).append(g.hTable);
		if(!p.colmodel) var ci=0;
		//setup thead			
		$('thead tr:first th',g.hDiv).each
			(
			 	function () {
			 		var thdiv=document.createElement('div');
			 		thdiv.innerHTML="<span>"+this.innerHTML+"</span>";
			 		if($(this).attr('sortname')) {
			 			$(this).click(function (e) { g.changeSort(this); });
			 			if($(this).attr('sortname')==p.sortname) {
			 				this.className='sorted';
			 				if(p.sortorder=='')
			 					p.sortorder='asc';
			 				$("span",thdiv).addClass('s'+p.sortorder);
			 			}
			 		}

			 		//var w=$(this).innerWidth()-10;
			 		//$(this).css("width",w);
			 		if(this.hide) $(this).hide();

			 		if(!p.colmodel) {
			 			$(this).attr('axis','col'+ci++);
			 		}
			 		$(thdiv).css("text-align","center");
			 		//$(thdiv).width(this.style.width);
			 		$(thdiv).width("100%");
			 		$(this).empty().append(thdiv).removeAttr('width');
			 	}
			);
		//set bDiv
		g.bDiv.className='bDiv';
		g.bDivBox.className='bDivBox';
		$(t).before(g.bDiv);
		$("thead",t).remove();
		$(g.bDiv)
		.append(t)
		;

		//add strip		
		if(p.striped)
			$('tbody tr:odd',g.bDiv).addClass('erow');

		// add pager
		if(p.usepager) {
			g.pDiv.className='pDiv';
			g.pDiv.innerHTML='<div class="pDiv2"></div>';
			$(g.hDiv).before(g.pDiv);
			var html=' <div class="pGroup"> <div class="pFirst pButton"><span></span></div><div class="pPrev pButton"><span></span></div> </div> <div class="btnseparator"></div> <div class="pGroup"><span class="pcontrol">Page <input type="text" size="4" value="1" /> of <span> 1 </span></span></div> <div class="btnseparator"></div> <div class="pGroup"> <div class="pNext pButton"><span></span></div><div class="pLast pButton"><span></span></div> </div> <div class="btnseparator"></div> <div class="pGroup"> <div class="pReload pButton"><span></span></div> </div> <div class="btnseparator"></div> <div class="pGroup"><span class="pPageStat"></span></div>';
			$('div',g.pDiv).html(html);
			$('.pReload',g.pDiv).click(function () { g.populate() });
			$('.pFirst',g.pDiv).click(function () { g.changePage('first') });
			$('.pPrev',g.pDiv).click(function () { g.changePage('prev') });
			$('.pNext',g.pDiv).click(function () { g.changePage('next') });
			$('.pLast',g.pDiv).click(function () { g.changePage('last') });
			$('.pcontrol input',g.pDiv).keydown(function (e) { if(e.keyCode==13) g.changePage('input') });
			if($.browser.msie&&$.browser.version<7) $('.pButton',g.pDiv).hover(function () { $(this).addClass('pBtnOver'); },function () { $(this).removeClass('pBtnOver'); });
			if(p.useRp) {
				var opt="";
				for(var nx=0;nx<p.rpOptions.length;nx++) {
					if(p.rp==p.rpOptions[nx]) sel='selected="selected"';else sel='';
					opt+="<option value='"+p.rpOptions[nx]+"' "+sel+" >"+p.rpOptions[nx]+"&nbsp;&nbsp;</option>";
				};
				$('.pDiv2',g.pDiv).prepend("<div class='pGroup'>Rows:&nbsp;<select name='rp'>"+opt+"</select></div> <div class='btnseparator'></div>");
				$('select',g.pDiv).change(
					function () {
						if(p.onRpChange)
							p.onRpChange(+this.value);
						else {
							p.newp=1;
							p.rp= +this.value;
							g.populate();
						}
					}
				);
			}
		}
		$(g.pDiv,g.sDiv).append("<div style='clear:both'></div>");
		//add block
		g.block.className='gBlock';
		var gh=$(g.bDiv).height();
		var gtop=g.bDiv.offsetTop;
		$(g.block).css({ width: '100%',height: 100,background: 'white',position: 'absolute',marginBottom: (gh* -1),zIndex: 1,top: gtop,left: '0px' });
		$(g.block).fadeTo(0,p.blockOpacity);
		//make grid functions accessible
		t.p=p;
		t.grid=g;
		// load data
		if(p.url&&p.autoload) {
			g.populate();
		} else {
			var i=0;
			$("tr",t).each(function () {
				var tr=this;
				i=0;
				$("thead tr th",g.hDiv).each(function () {
					$("td:eq("+i+") div",tr).css({ "width": "100%","display": this.style.display,"text-align": $(this).attr("align") });
					i++;
				});
			});
			$(t).addClass("tblborder");
		}
		$(g.bDiv).before(g.bDivBox);
		$(g.bDivBox).append(g.bDiv);
		if(parseInt(p.height)>0) {
			$(g.bDivBox).height(p.height);
		}
		return t;
	};

	var docloaded=false;
	$(document).ready(function () { docloaded=true });
	$.fn.flexigrid=function (p) {
		return this.each(function () {
			if(!docloaded) {
				$(this).hide();
				var t=this;
				$(document).ready
					(
						function () {
							$.addFlex(t,p);
						}
					);
			} else {
				$.addFlex(this,p);
			}
		});
	}; //end flexigrid

	$.fn.flexReload=function (p) { // function to reload grid
		return this.each(function () {
			if(this.grid&&this.p.url) this.grid.populate();
		});
	}; //end flexReload
	$.fn.flexOptions=function (p) { //function to update general options
		return this.each(function () {
			if(this.grid) $.extend(this.p,p);
		});
	}; //end flexOptions
	$.fn.flexToggleCol=function (cid,visible) { // function to reload grid
		return this.each(function () {
			if(this.grid) this.grid.toggleCol(cid,visible);
		});
	}; //end flexToggleCol
	$.fn.flexAddData=function (data) { // function to add data to grid
		return this.each(function () {
			if(this.grid) this.grid.addData(data);
		});
	};
	$.fn.noSelect=function (p) { //no select plugin by me :-)
		if(p==null)
			prevent=true;
		else
			prevent=p;
		if(prevent) {
			return this.each(function () {
				if($.browser.msie||$.browser.safari) $(this).bind('selectstart',function () { return false; });
				else if($.browser.mozilla) {
					$(this).css('MozUserSelect','none');
					$('body').trigger('focus');
				}
				else if($.browser.opera) $(this).bind('mousedown',function () { return false; });
				else $(this).attr('unselectable','on');
			});
		} else {
			return this.each(function () {
				if($.browser.msie||$.browser.safari) $(this).unbind('selectstart');
				else if($.browser.mozilla) $(this).css('MozUserSelect','inherit');
				else if($.browser.opera) $(this).unbind('mousedown');
				else $(this).removeAttr('unselectable','on');
			});
		}
	}; //end noSelect
})(jQuery);