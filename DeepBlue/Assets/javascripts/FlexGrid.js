(function ($) {
	$.addFlex=function (t,p) {
		if(t.grid) { return false; }
		p=$.extend({
			height: 0,url: false,method: 'POST',dataType: 'json',errormsg: 'Connection Error',usepager: false,
			page: 1,total: 1,useRp: true,rp: 20,rpOptions: [20,50,100],
			title: false,pagestat: 'Displaying {from} to {to} of {total} items',procmsg: 'Loading...',
			nomsg: 'No items',hideOnSubmit: true,autoload: true,blockOpacity: 0.5,
			sortname: '',sortorder: '',resizeWidth: true,onChangeSort: false,
			onSuccess: false,onRowClick: false,onRowBound: false,onSubmit: false
		},p);
		$(t).show();
		var g={
			hset: {},
			addData: function (data) {
				if(p.preProcess) { data=p.preProcess(data); }
				$('.pReload',this.pDiv).removeClass('loading');this.loading=false;
				if(!data) { $('.pPageStat',this.pDiv).html(p.errormsg);return false; }
				if(p.dataType=='xml') { p.total= +$('rows total',data).text(); } else { p.total=data.total; }
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
				if(p.dataType=='xml') {
					p.page= +$('rows page',data).text();
				} else {
					p.page=data.page;
				}
				this.buildpager();
				var tbody=document.createElement('tbody');
				if(p.dataType=='json') {
					$.each
					(
					 data.rows,
					 function (i,row) {
					 	var tr=document.createElement('tr');
					 	if(i%2) { tr.className='erow'; }
					 	tr.id='row'+i;
					 	var i=0;
					 	$("thead tr:first th",g.hDiv).each(function () {
					 		var td=document.createElement('td');
					 		var div=document.createElement('div');
					 		if(row.cell.length>i) {
					 			switch($(this).attr("datatype")) {
					 				case "Boolean":
					 					if(row.cell[i]==true) { div.innerHTML="<img src='/Assets/images/tick.png' />"; }
					 					break;
					 				case "money":
					 					div.innerHTML=jHelper.dollarAmount(row.cell[i].toString());
					 					break;
					 				default:
					 					div.innerHTML=row.cell[i];
					 			}
					 		}
					 		$(td).css({ "display": this.style.display });
					 		$(td).css({ "width": this.style.width,"display": this.style.display });
					 		$(div).css("text-align",$(this).attr("align"));
					 		$(td).append(div);
					 		$(tr).append(td);
					 		td=null;
					 		i++;
					 	});
					 	$(tbody).append(tr);
					 	if(p.onRowBound) {
					 		p.onRowBound(tr,row,t);
					 	}
					 	if(p.onRowClick) {
					 		$(tr).click(function () {
					 			p.onRowClick(row);
					 		});
					 	}
					 	tr=null;
					 }
					);
				}
				$('tr',t).unbind();
				$(t).empty();
				$(t).append(tbody);
				tbody=null;data=null;i=null;
				if(p.onSuccess) { p.onSuccess(t); }
				this.hDiv.scrollLeft=this.bDiv.scrollLeft;
				if($.browser.opera) { $(t).css('visibility','visible'); }
			},
			changeSort: function (th) {
				if(this.loading) { return true; }
				$(g.nDiv).hide();$(g.nBtn).hide();
				if(p.sortname==$(th).attr('sortname')) {
					if(p.sortorder=='asc') { p.sortorder='desc'; }
					else { p.sortorder='asc'; }
				}
				$(th).addClass('sorted').siblings().removeClass('sorted');
				$('.sdesc',this.hDiv).removeClass('sdesc');
				$('.sasc',this.hDiv).removeClass('sasc');
				$('div span',th).addClass('s'+p.sortorder);
				p.sortname=$(th).attr('sortname');
				if(p.onChangeSort) {
					p.onChangeSort(p.sortname,p.sortorder);
				} else {
					this.populate();
				}
			},
			buildpager: function () {
				$('.pcontrol input',this.pDiv).val(p.page);
				$('.pcontrol span',this.pDiv).html(p.pages);
				var r1=(p.page-1)*p.rp+1;
				var r2=r1+p.rp-1;
				if(p.total<r2) { r2=p.total; }
				var stat=p.pagestat;
				stat=stat.replace(/{from}/,r1);
				stat=stat.replace(/{to}/,r2);
				stat=stat.replace(/{total}/,p.total);
				$('.pPageStat',this.pDiv).html(stat);
				$('.pGLoading',this.pDiv).hide();
			}
			,resize: function () {
				if(p.resizeWidth) {
					var w=g.gDiv.offsetWidth;
					var adw=w-20;
					if(g.pDiv) { $(g.pDiv).width(adw); }
					if(g.hDiv) { $(g.hDiv).width(adw); }
					if(g.bDiv) { $(g.bDiv).width(adw); }
					if(g.bDivBox) { $(g.bDivBox).width(w); }
				}
			}
			,populate: function () {
				if(this.loading) { return true; }
				if(p.onSubmit) {
					var gh=p.onSubmit(p);
					if(!gh) { return false; }
				}
				this.loading=true;
				if(!p.url) { return false; }
				$('.pGLoading',this.pDiv).show();
				$('.pPageStat',this.pDiv).html(p.procmsg);
				$('.pReload',this.pDiv).addClass('loading');
				$(g.block).css({ top: g.bDiv.offsetTop });
				if(p.hideOnSubmit) {
					$(g.block).height($(g.bDiv).height());
					$(this.gDiv).prepend(g.block);
				}
				if($.browser.opera) { $(t).css('visibility','hidden'); }
				if(!p.newp) { p.newp=1; }
				if(p.page>p.pages) { p.page=p.pages; }
				var dt=new Date();
				var param=[];
				if(p.usepager) {
					param=[
					 { name: 'pageIndex',value: p.newp }
					,{ name: 'pageSize',value: p.rp }
					,{ name: 'sortName',value: p.sortname }
					,{ name: 'sortOrder',value: p.sortorder }
				];
				}
				if(p.params) {
					for(var pi=0;pi<p.params.length;pi++) param[param.length]=p.params[pi];
				}
				param[param.length]={ name: "t",value: dt.getTime() };
				$.ajax({
					type: p.method,
					url: p.url,
					data: param,
					dataType: p.dataType,
					success: function (data) {
						if(p.hideOnSubmit) { $(g.block).remove(); }
						g.addData(data);
					},
					error: function (data) { try { if(p.onError) { p.onError(data); } } catch(e) { } }
				});
			},
			changePage: function (ctype) {
				if(this.loading) { return true; }
				switch(ctype) {
					case 'first': p.newp=1;break;
					case 'prev': if(p.page>1) { p.newp=parseInt(p.page)-1; } break;
					case 'next': if(p.page<p.pages) { p.newp=parseInt(p.page)+1; } break;
					case 'last': p.newp=p.pages;break;
					case 'input':
						var nv=parseInt($('.pcontrol input',this.pDiv).val());
						if(isNaN(nv)) { nv=1; }
						if(nv<1) { nv=1; }
						else if(nv>p.pages) { nv=p.pages; }
						$('.pcontrol input',this.pDiv).val(nv);
						p.newp=nv;
						break;
				}
				if(p.newp==p.page) { return false; }
				if(p.onChangePage) {
					p.onChangePage(p.newp);
				} else {
					this.populate();
				}
			},
			pager: 0
		};
		g.gDiv=document.createElement('div');
		g.hDiv=document.createElement('div');
		g.bDivBox=document.createElement('div');
		g.bDiv=document.createElement('div');
		g.block=document.createElement('div');
		if(p.usepager) { g.pDiv=document.createElement('div'); }
		g.hTable=document.createElement("table");
		g.gDiv.className='flexigrid';
		$(t).before(g.gDiv);
		$(g.gDiv).append(t);
		g.hDiv.className='hDiv';
		$(t).before(g.hDiv);
		g.hTable.cellPadding=0;
		g.hTable.cellSpacing=0;
		g.hTable.style.width="100%";
		t.style.width="100%";
		$(g.hTable).append($("thead",t));
		$(g.hDiv).append(g.hTable);
		if(!p.colmodel) { var ci=0; }
		$('thead tr:first th',g.hDiv).each
			(
			 	function () {
			 		var thdiv=document.createElement('div');
			 		thdiv.innerHTML="<span>"+this.innerHTML+"</span>";
			 		if($(this).attr('sortname')) {
			 			$(this).click(function (e) { g.changeSort(this); });
			 			if($(this).attr('sortname')==p.sortname) {
			 				this.className='sorted';
			 				if(p.sortorder=='') {
			 					p.sortorder='asc';
			 				}
			 				$("span",thdiv).addClass('s'+p.sortorder);
			 			}
			 		}
			 		if(this.hide) { $(this).hide(); }
			 		if(!p.colmodel) {
			 			$(this).attr('axis','col'+ci++);
			 		}
			 		$(thdiv).css("text-align","center");
			 		$(thdiv).width("100%");
			 		$(this).empty().append(thdiv).removeAttr('width');
			 	}
			);
		g.bDiv.className='bDiv';
		g.bDivBox.className='bDivBox';
		$(t).before(g.bDiv);
		$("thead",t).remove();
		$(g.bDiv).append(t);
		if(p.usepager) {
			g.pDiv.className='pDiv';
			g.pDiv.innerHTML='<div class="pDiv2"></div>';
			$(g.hDiv).before(g.pDiv);
			var html='<div class="pGroup"> <div class="pFirst pButton"><span></span></div><div class="pPrev pButton"><span></span></div> </div> <div class="btnseparator"></div> <div class="pGroup"><span class="pcontrol">Page <input type="text" size="4" value="1" /> of <span> 1 </span></span></div> <div class="btnseparator"></div> <div class="pGroup"> <div class="pNext pButton"><span></span></div><div class="pLast pButton"><span></span></div> </div> <div class="btnseparator"></div> <div class="pGroup"> <div class="pReload pButton"><span></span></div> </div> <div class="btnseparator"></div> <div class="pGroup"><span class="pPageStat"></span></div>';
			$('div',g.pDiv).html(html);
			$('.pReload',g.pDiv).click(function () { g.populate() });
			$('.pFirst',g.pDiv).click(function () { g.changePage('first') });
			$('.pPrev',g.pDiv).click(function () { g.changePage('prev') });
			$('.pNext',g.pDiv).click(function () { g.changePage('next') });
			$('.pLast',g.pDiv).click(function () { g.changePage('last') });
			$('.pcontrol input',g.pDiv).keydown(function (e) { if(e.keyCode==13) { g.changePage('input'); } });
			if($.browser.msie&&$.browser.version<7) { $('.pButton',g.pDiv).hover(function () { $(this).addClass('pBtnOver'); },function () { $(this).removeClass('pBtnOver'); }); }
			$(g.pDiv).prepend("<div class='pGroup pGLoading'><span class='pLoadingStat'>Loading...</span></div>");
			if(p.useRp) {
				var opt="";
				for(var nx=0;nx<p.rpOptions.length;nx++) {
					if(p.rp==p.rpOptions[nx]) { sel='selected="selected"'; } else { sel=''; }
					opt+="<option value='"+p.rpOptions[nx]+"' "+sel+" >"+p.rpOptions[nx]+"&nbsp;&nbsp;</option>";
				};

				$('.pDiv2',g.pDiv).prepend("<div class='pGroup'>Rows:&nbsp;<select name='rp'>"+opt+"</select></div> <div class='btnseparator'></div>");
				$('select',g.pDiv).change(
					function () {
						if(p.onRpChange) {
							p.onRpChange(+this.value);
						} else {
							p.newp=1;
							p.rp= +this.value;
							g.populate();
						}
					}
				);
			}
		}
		$(g.pDiv,g.sDiv).append("<div style='clear:both'></div>");
		g.block.className='gBlock';
		var gh=$(g.bDiv).height();
		var gtop=g.bDiv.offsetTop;
		//$(g.block).css({ width: '100%',height: 100,background: 'white',position: 'absolute',marginBottom: (gh* -1),zIndex: 1,top: gtop,left: '0px' });
		//$(g.block).fadeTo(0,p.blockOpacity);
		t.p=p;
		t.grid=g;
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
		g.resize();
		$(window).resize(function () { g.resize(); });
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
	};
	$.fn.flexReload=function (p) {
		return this.each(function () {
			if(this.grid&&this.p.url) { this.grid.populate(); }
		});
	};
	$.fn.flexOptions=function (p) {
		return this.each(function () {
			if(this.grid) { $.extend(this.p,p); }
		});
	};
	$.fn.flexToggleCol=function (cid,visible) {
		return this.each(function () {
			if(this.grid) { this.grid.toggleCol(cid,visible); }
		});
	};
	$.fn.flexAddData=function (data) {
		return this.each(function () {
			if(this.grid) { this.grid.addData(data); }
		});
	};
	$.fn.noSelect=function (p) {
		if(p==null) {
			prevent=true;
		} else {
			prevent=p;
		}
		if(prevent) {
			return this.each(function () {
				if($.browser.msie||$.browser.safari) { $(this).bind('selectstart',function () { return false; }); }
				else if($.browser.mozilla) {
					$(this).css('MozUserSelect','none');
					$('body').trigger('focus');
				}
				else if($.browser.opera) { $(this).bind('mousedown',function () { return false; }); }
				else { $(this).attr('unselectable','on'); }
			});
		} else {
			return this.each(function () {
				if($.browser.msie||$.browser.safari) { $(this).unbind('selectstart'); }
				else if($.browser.mozilla) { $(this).css('MozUserSelect','inherit'); }
				else if($.browser.opera) { $(this).unbind('mousedown'); }
				else { $(this).removeAttr('unselectable','on'); }
			});
		}
	};
})(jQuery);