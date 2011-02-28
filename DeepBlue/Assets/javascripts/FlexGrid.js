/*
* Flexigrid for jQuery - New Wave Grid
*
* Copyright (c) 2008 Paulo P. Marinas (webplicity.net/flexigrid)
* Dual licensed under the MIT (MIT-LICENSE.txt)
* and GPL (GPL-LICENSE.txt) licenses.
*
* $Date: 2008-07-14 00:09:43 +0800 (Tue, 14 Jul 2008) $
*/

(function ($) {

	$.addFlex=function (t,p) {

		if(t.grid) return false; //return if already exist	

		// apply default properties
		p=$.extend({
			height: 200, //default height
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
			rp: 15, // results per page
			rpOptions: [10,15,20,25,40],
			title: false,
			pagestat: 'Displaying {from} to {to} of {total} items',
			procmsg: 'Processing, please wait ...',
			query: '',
			qtype: '',
			nomsg: 'No items',
			minColToggle: 1, //minimum allowed column to be hidden
			showToggleBtn: true, //show or hide column toggle popup
			hideOnSubmit: true,
			autoload: true,
			blockOpacity: 0.5,
			onToggleCol: false,
			onChangeSort: false,
			onSuccess: false,
			onSubmit: false // using a custom populate function
		},p);


		$(t)
		.show() //show if hidden
		.attr({ cellPadding: 0,cellSpacing: 0,border: 0 })  //remove padding and spacing
		.removeAttr('width') //remove width properties	
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

					 	if(row.id) tr.id='row'+row.id;

					 	//add cell
					 	$('thead tr:first th',g.hDiv).each
							(
							 	function () {

							 		var td=document.createElement('td');
							 		var idx=$(this).attr('axis').substr(3);
							 		td.align=this.align;
							 		td.innerHTML=row.cell[idx];
							 		$(tr).append(td);
							 		td=null;
							 	}
							);


					 	if($('thead',this.gDiv).length<1) //handle if grid has no headers
					 	{

					 		for(idx=0;idx<cell.length;idx++) {
					 			var td=document.createElement('td');
					 			td.innerHTML=row.cell[idx];
					 			$(tr).append(td);
					 			td=null;
					 		}
					 	}

					 	$(tbody).append(tr);
					 	tr=null;
					 }
					);

				} else if(p.dataType=='xml') {

					i=1;

					$("rows row",data).each
				(

				 	function () {

				 		i++;

				 		var tr=document.createElement('tr');
				 		if(i%2&&p.striped) tr.className='erow';

				 		var nid=$(this).attr('id');
				 		if(nid) tr.id='row'+nid;

				 		nid=null;

				 		var robj=this;



				 		$('thead tr:first th',g.hDiv).each
							(
							 	function () {

							 		var td=document.createElement('td');
							 		var idx=$(this).attr('axis').substr(3);
							 		td.align=this.align;
							 		td.innerHTML=$("cell:eq("+idx+")",robj).text();
							 		$(tr).append(td);
							 		td=null;
							 	}
							);


				 		if($('thead',this.gDiv).length<1) //handle if grid has no headers
				 		{
				 			$('cell',this).each
								(
								 	function () {
								 		var td=document.createElement('td');
								 		td.innerHTML=$(this).text();
								 		$(tr).append(td);
								 		td=null;
								 	}
								);
				 		}

				 		$(tbody).append(tr);
				 		tr=null;
				 		robj=null;
				 	}
				);

				}

				$('tr',t).unbind();
				$(t).empty();

				$(t).append(tbody);
				this.addCellProp();

				tbody=null;data=null;i=null;

				if(p.onSuccess) p.onSuccess();
				if(p.hideOnSubmit) $(g.block).remove(); //$(t).show();

				this.hDiv.scrollLeft=this.bDiv.scrollLeft;
				if($.browser.opera) $(t).css('visibility','visible');

			},
			changeSort: function (th) { //change sortorder

				if(this.loading) return true;

				$(g.nDiv).hide();$(g.nBtn).hide();

				if(p.sortname==$(th).attr('abbr')) {
					if(p.sortorder=='asc') p.sortorder='desc';
					else p.sortorder='asc';
				}

				$(th).addClass('sorted').siblings().removeClass('sorted');
				$('.sdesc',this.hDiv).removeClass('sdesc');
				$('.sasc',this.hDiv).removeClass('sasc');
				$('div',th).addClass('s'+p.sortorder);
				p.sortname=$(th).attr('abbr');

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

				if(p.hideOnSubmit) $(this.gDiv).prepend(g.block); //$(t).hide();

				if($.browser.opera) $(t).css('visibility','hidden');

				if(!p.newp) p.newp=1;

				if(p.page>p.pages) p.page=p.pages;
				//var param = {page:p.newp, rp: p.rp, sortname: p.sortname, sortorder: p.sortorder, query: p.query, qtype: p.qtype};
				var param=[
					 { name: 'page',value: p.newp }
					,{ name: 'rp',value: p.rp }
					,{ name: 'sortname',value: p.sortname }
					,{ name: 'sortorder',value: p.sortorder }
					,{ name: 'query',value: p.query }
					,{ name: 'qtype',value: p.qtype }
				];

				if(p.params) {
					for(var pi=0;pi<p.params.length;pi++) param[param.length]=p.params[pi];
				}

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
			addCellProp: function () {

				$('tbody tr td',g.bDiv).each
					(
						function () {
							var tdDiv=document.createElement('div');
							var n=$('td',$(this).parent()).index(this);
							var pth=$('th:eq('+n+')',g.hDiv).get(0);

							if(pth!=null) {
								if(p.sortname==$(pth).attr('abbr')&&p.sortname) {
									this.className='sorted';
								}
								 

								$(tdDiv).css({ textAlign: $(pth).attr("calign"),width: $('div:first',pth)[0].style.width });

								if(pth.hide) $(this).css('display','none');

							}

							if(p.nowrap==false) $(tdDiv).css('white-space','normal');

							if(this.innerHTML=='') this.innerHTML='&nbsp;';

							//tdDiv.value = this.innerHTML; //store preprocess value
							tdDiv.innerHTML=this.innerHTML;

							var prnt=$(this).parent()[0];
							var pid=false;
							if(prnt.id) pid=prnt.id.substr(3);

							if(pth!=null) {
								if(pth.process) pth.process(tdDiv,pid);
							}

							$(this).empty().append(tdDiv).removeAttr('width'); //wrap content

							//add editable event here 'dblclick'

						}
					);

			},
			pager: 0
		};

		//create model if any
		if(p.colModel) {
			thead=document.createElement('thead');
			tr=document.createElement('tr');

			for(i=0;i<p.colModel.length;i++) {
				var cm=p.colModel[i];
				var th=document.createElement('th');

				th.innerHTML=cm.display;

				if(cm.name&&cm.sortable)
					$(th).attr('abbr',cm.name);

				//th.idx = i;
				$(th).attr('axis','col'+i).attr("calign",cm.align);

				th.align="center";

				if(cm.width)
					$(th).width(cm.width);

				if(cm.hide) {
					th.hide=true;
				}

				if(cm.process) {
					th.process=cm.process;
				}

				$(tr).append(th);
			}
			$(thead).append(tr);
			$(t).prepend(thead);
		} // end if p.colmodel	

		//init divs
		g.gDiv=document.createElement('div'); //create global container
		g.hDiv=document.createElement('div'); //create header container
		g.bDiv=document.createElement('div'); //create body container

		if(p.usepager) g.pDiv=document.createElement('div'); //create pager container
		g.hTable=document.createElement('table');

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
		$(g.hDiv).append(g.hTable);
		var thead=$("thead:first",t).get(0);
		if(thead) $(g.hTable).append(thead);
		thead=null;

		if(!p.colmodel) var ci=0;

		//setup thead			
		$('thead tr:first th',g.hDiv).each
			(
			 	function () {
			 		var thdiv=document.createElement('div');

			 		if($(this).attr('abbr')) {
			 			$(this).click(
								function (e) {
									if(!$(this).hasClass('thOver')) return false;
									var obj=(e.target||e.srcElement);
									if(obj.href||obj.type) return true;
									g.changeSort(this);
								}
							)
							;

			 			if($(this).attr('abbr')==p.sortname) {
			 				this.className='sorted';
			 				thdiv.className='s'+p.sortorder;
			 			}
			 		}

			 		if(this.hide) $(this).hide();

			 		if(!p.colmodel) {
			 			$(this).attr('axis','col'+ci++);
			 		}

			 		$(thdiv).css("text-align",this.align);

			 		$(thdiv).width(this.style.width);

			 		thdiv.innerHTML=this.innerHTML;

			 		$(this).empty().append(thdiv).removeAttr('width');
			 	}
			);

		//set bDiv
		g.bDiv.className='bDiv';
		$(t).before(g.bDiv);
		$(g.bDiv)
		.append(t)
		;

		if(p.height=='auto') {
			$('table',g.bDiv).addClass('autoht');
		}


		//add td properties
		g.addCellProp();

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

		//make grid functions accessible
		t.p=p;
		t.grid=g;

		// load data
		if(p.url&&p.autoload) {
			g.populate();
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