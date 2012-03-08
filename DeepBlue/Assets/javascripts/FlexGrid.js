(function ($) {
	$.addFlex=function (t,p) {
		if(t.grid) { return false; }
		p=$.extend({
			height: 0
			,url: false
			,method: 'POST'
			,dataType: 'json'
			,errormsg: 'Connection Error'
			,usepager: false
			,page: 1
			,total: 1
			,useRp: true
			,rp: 25
			,rpOptions: [25,50,100,200]
			,title: false
			,pagestat: 'Displaying {from} to {to} of {total} items'
			,procmsg: 'Loading...'
			,nomsg: 'No items'
			,hideOnSubmit: true
			,autoload: true
			,blockOpacity: 0.5
			,sortname: ''
			,sortorder: ''
			,resizeWidth: true
			,tableName: ''
			,exportExcel: false
			,onChangeSort: false
			,onSuccess: false
			,onRowClick: false
			,onRowBound: false
			,onSubmit: false
			,onInit: false
			,onBeforeAddData: false
			,onTemplate: false
			,width: 0
			,useBoxStyle: true
		},p);
		$(t).show();
		var g={
			hset: {},
			addData: function (data) {
				if(p.onBeforeAddData) { p.onBeforeAddData(data); }
				if(p.preProcess) { data=p.preProcess(data); }
				$('.pReload',this.pDiv).removeClass('loading');this.loading=false;
				if(!data) { $('.pPageStat',this.pDiv).html(p.errormsg);return false; }
				if(p.dataType=='xml') { p.total= +$('rows total',data).text(); } else { p.total=data.total; }
				var tbody=document.createElement('tbody');
				$(".pDiv .pDiv2",g.gDiv).hide();
				$(".bpDiv",g.gDiv).hide();
				if(p.total==0) {
					$('tr, a, td, div',t).unbind();
					$("tbody",t).remove();
					p.pages=1;
					p.page=1;
					this.buildpager();
					$('.pPageStat',this.pDiv).html(p.nomsg);
					$(t).append(tbody);
					if(p.onSuccess) { p.onSuccess(t,g); }
					if(p.onTemplate) { p.onTemplate(tbody,data); }
					return false;
				}
				if(p.total>25) {
					$(".pDiv .pDiv2",g.gDiv).show();
					$(".bpDiv",g.gDiv).show();
				}
				p.pages=Math.ceil(p.total/p.rp);
				if(p.dataType=='xml') {
					p.page= +$('rows page',data).text();
				} else {
					p.page=data.page;
				}
				this.buildpager();
				if(p.onTemplate) {
					p.onTemplate(tbody,data);
				} else {
					if(p.dataType=='json') {
						$.each
						(
						 data.rows,
						 function (i,row) {
						 	var tr=document.createElement('tr');
						 	if(i%2) { tr.className='erow'; } else { tr.className='grow'; }
						 	tr.id='row'+(i+1);
						 	var i=0;
						 	$("thead tr:first th",g.bDiv).each(function () {
						 		var td=document.createElement('td');
						 		var div=document.createElement('div');
						 		if(row.cell.length>i) {
						 			switch($(this).attr("datatype")) {
						 				case "Boolean":
						 					if(row.cell[i]==true) { div.innerHTML=jHelper.imageHTML("tick.png"); }
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
				}
				$('tr',t).unbind();
				$("tbody",t).remove();
				$(t).append(tbody);
				tbody=null;data=null;i=null;
				if(p.onSuccess) { p.onSuccess(t,g); }
				this.bDiv.scrollLeft=this.bDiv.scrollLeft;
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
				$('.sdesc',this.bDiv).removeClass('sdesc');
				$('.sasc',this.bDiv).removeClass('sasc');
				$('div span',th).addClass('s'+p.sortorder);
				p.sortname=$(th).attr('sortname');
				if(p.onChangeSort) {
					p.onChangeSort(p.sortname,p.sortorder);
				} else {
					this.populate();
				}
			},
			buildpager: function () {
				$(".pDiv",g.gDiv).each(function () {
					g.setUpPager(this);
				});
			}
			,setUpPager: function (pDiv) {
				$('.pcontrol input',pDiv).val(p.page);
				$('.pcontrol span',pDiv).html(p.pages);
				var r1=(p.page-1)*p.rp+1;
				var r2=r1+p.rp-1;
				if(p.total<r2) { r2=p.total; }
				var stat=p.pagestat;
				stat=stat.replace(/{from}/,r1);
				stat=stat.replace(/{to}/,r2);
				stat=stat.replace(/{total}/,p.total);
				$('.pPageStat',pDiv).html(stat);
				$('.pGLoading',pDiv).hide();
				$("#rp",pDiv).val(p.rp);
			}
			,resize: function () {
				var w;
				if(p.width>0) {
					w=p.width;
				} else {
					w=g.gDiv.offsetWidth;
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
				//$('.pPageStat',this.pDiv).html(p.procmsg);
				$('.pReload',this.pDiv).addClass('loading');
				$(g.block).css({ top: g.bDiv.offsetTop });
				/*if(p.hideOnSubmit) {
				$(g.block).height($(g.bDiv).height());
				$(this.gDiv).prepend(g.block);
				}*/
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
				$.ajax({
					type: p.method,
					url: p.url,
					data: param,
					dataType: p.dataType,
					success: function (data) {
						//if(p.hideOnSubmit) { $(g.block).remove(); }
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
			exportExcel: function () {
				var width=300;var height=200;var left=(screen.availWidth/2)-(width/2);var top=(screen.availHeight/2)-(height/2);var features="width="+width+",height="+height+",left="+left+",top="+top+",location=no,menubar=no,toobar=no,scrollbars=yes,resizable=yes,status=yes";
				window.open(deepBlue.rootUrl+"/Admin/ExportExcel?tableName="+p.tableName,p.tableName,features);
			},
			setPagingEvent: function (pDiv) {
				$('.pReload',pDiv).click(function () { g.populate() });
				$('.pFirst',pDiv).click(function () { g.changePage('first') });
				$('.pPrev',pDiv).click(function () { g.changePage('prev') });
				$('.pNext',pDiv).click(function () { g.changePage('next') });
				$('.pLast',pDiv).click(function () { g.changePage('last') });
				$('.pcontrol input',pDiv).keydown(function (e) { if(e.keyCode==13) { g.changePage('input'); } });
				if($.browser.msie&&$.browser.version<7) { $('.pButton',pDiv).hover(function () { $(this).addClass('pBtnOver'); },function () { $(this).removeClass('pBtnOver'); }); }
				$(pDiv).prepend("<div class='pGroup pGLoading'><span class='pLoadingStat'>Loading...</span></div>");
				if(p.useRp) {
					var opt="";
					for(var nx=0;nx<p.rpOptions.length;nx++) {
						if(p.rp==p.rpOptions[nx]) { sel='selected="selected"'; } else { sel=''; }
						opt+="<option value='"+p.rpOptions[nx]+"' "+sel+" >"+p.rpOptions[nx]+"&nbsp;&nbsp;</option>";
					};
					$('.pDiv2',pDiv).css("display","none").prepend("<div class='pGroup'><table cellpadding=0 cellspacing=0><tr><td>Rows:&nbsp;</td><td><select id='rp' name='rp'>"+opt+"</select></td></tr></table></div> <div class='btnseparator'></div>");
					$('select',pDiv).change(
					function () {
						if(p.onRpChange) {
							p.onRpChange(+this.value);
						} else {
							p.newp=1;
							p.rp= +this.value;
							$(":input[name='jqCBSTextBox_rp']",g.gDiv).val(p.rp);
							g.populate();
						}
					}
				);
				}
			},
			pager: 0
		};
		g.gDiv=document.createElement('div');

		if(p.width>0) { $(g.gDiv).width(p.width); }

		g.gTLDiv=document.createElement('div');
		g.gTCDiv=document.createElement('div');
		g.gTRDiv=document.createElement('div');

		g.gCDiv=document.createElement('div');

		g.gBLDiv=document.createElement('div');
		g.gBCDiv=document.createElement('div');
		g.gBRDiv=document.createElement('div');

		g.bDiv=document.createElement('div');
		g.bDivBox=document.createElement('div');
		g.bDiv=document.createElement('div');
		g.block=document.createElement('div');

		if(p.usepager) { g.pDiv=document.createElement('div'); }

		g.hTable=document.createElement("table");
		g.gDiv.className='flexigrid';

		g.gTLDiv.className="ftlDiv";
		g.gTCDiv.className="ftcDiv";
		g.gTRDiv.className="ftrDiv";

		g.gBLDiv.className="fblDiv";
		g.gBCDiv.className="fbcDiv";
		g.gBRDiv.className="fbrDiv";

		g.gCDiv.className="fcDiv";

		var pt=t;
		if($(t).parents(".tblbox").get(0)) {
			pt=$(t).parents(".tblbox").get(0); //new grid style
		}

		$(pt).before(g.gDiv);
		if(p.useBoxStyle) {
			$(g.gDiv).append(g.gTLDiv);
			$(g.gTLDiv).append(g.gTCDiv);
			$(g.gTLDiv).append(g.gTRDiv);
			$(g.gDiv).append(g.gCDiv);
			$(g.gDiv).append(g.gBLDiv);
			$(g.gBLDiv).append(g.gBCDiv);
			$(g.gBLDiv).append(g.gBRDiv);
			$(g.gCDiv).append(pt);
		} else {
			$(g.gDiv).append(pt);
		}
		g.bDiv.className='bDiv';
		$(pt).before(g.bDiv);
		g.hTable.cellPadding=0;
		g.hTable.cellSpacing=0;
		g.hTable.style.width="100%";
		t.style.width="100%";
		//$(g.hTable).append($("thead",t));
		//$(g.bDiv).append(g.hTable);
		if(!p.colmodel) { var ci=0; }
		$('thead tr:first th',t).each
			(
			 	function () {
			 		var thdiv=document.createElement('div');
			 		thdiv.innerHTML="<span>"+this.innerHTML+"</span>";
			 		if($(this).attr('sortname')) {
			 			$(this)
						.addClass("issortcol")
						.click(function (e) { g.changeSort(this); });
			 			var sname=$(this).attr('sortname');
			 			if(sname==p.sortname) {
			 				$(this).addClass("sorted");
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
			 		var align=$(this).attr("align");
			 		$(thdiv).css("text-align",align);
			 		$(thdiv).width("100%");
			 		$(this).empty().append(thdiv).removeAttr('width');
			 	}
			);
		g.bDiv.className='bDiv';
		g.bDivBox.className='bDivBox';

		//$("thead",t).remove();
		$(g.bDiv).append(pt);
		if(p.usepager) {
			g.pDiv.className='pDiv';
			g.pDiv.innerHTML='<div class="pDiv2"></div>';
			var html='<div class="pGroup"> <div class="pFirst pButton"><span></span></div><div class="pPrev pButton"><span></span></div> </div> <div class="btnseparator"></div> <div class="pGroup"><span class="pcontrol">Page <input type="text" size="4" value="1" style="width:40px;text-align:center;" /> of <span> 1 </span></span></div> <div class="btnseparator"></div> <div class="pGroup"> <div class="pNext pButton"><span></span></div><div class="pLast pButton"><span></span></div> </div> <div class="btnseparator"></div> <div class="pGroup"> <div class="pReload pButton"><span></span></div></div>';
			//html+='<div class="btnseparator"></div><div class="pGroup"><span class="pPageStat"></span></div>';

			$('div',g.pDiv).html(html);

			$(g.bDiv).before(g.pDiv);
			$(g.bDiv).after($(g.pDiv).clone().css("display","none").addClass("bpDiv"));

			if(p.exportExcel!="") {
				var exportExcel='<div id="ExportExcel" class="green-btn"><div class="left"></div><div class="center">Export Excel</div><div class="right"></div></div>';
				exportExcel+='<div class="exp-excel-loading"><span class="pLoadingStat">Exporting...</span></div>';
				$(g.pDiv).append(exportExcel);
				$("#ExportExcel",g.pDiv).unbind("click").click(function () { g.exportExcel(); });
			}

			$(".pDiv",g.gDiv).each(function () {
				g.setPagingEvent(this);
			});
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
				$("thead tr th",g.bDiv).each(function () {
					$("td:eq("+i+") div",tr).css({ "width": "100%","display": this.style.display,"text-align": $(this).attr("align") });
					i++;
				});
			});
			$(t).addClass("tblborder");
		}
		$(g.bDiv).before(g.bDivBox);
		$(g.bDivBox).append(g.bDiv);
		g.resize();
		jHelper.jqComboBox(g.gDiv);
		try {
			//$(g.gDiv).jqTransform();
		} catch(e) { jAlert(e); }
		if(p.resizeWidth) {
			$(window).resize(function () {
				$(g.gDiv).css("width","");
				g.resize();
			});
		}
		if(p.onInit) {
			p.onInit(g);
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
	};
	$.fn.flexExist=function (p) {
		return this.each(function () {
			if(this.grid&&this.p.url) {
				return true;
			}
		});
	};
	$.fn.flexReload=function (p) {
		return this.each(function () {
			if(this.grid&&this.p.url) {
				this.p.page=1;
				this.p.newp=1;

				this.grid.populate();
			}
		});
	};
	$.fn.flexRemoveSortClass=function (p) {
		return this.each(function () {
			if(this.grid) {
				$(".sorted",this.t).removeClass("sorted");
				$(".sasc",this.t).removeClass("sasc");
				$(".sdesc",this.t).removeClass("sdesc");
			}
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