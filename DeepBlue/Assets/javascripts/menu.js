﻿<!--
var timeout         = 500;
var closetimer		= 0;
var ddmenuitem      = 0;
var subclosetimer	= 0;
var subddmenuitem   = 0;
// open hidden layer
function mopen(that,id)
{	
	// cancel close timer
	mcancelclosetime();

	// close old layer
	if(ddmenuitem) ddmenuitem.style.visibility = 'hidden';

	// get new layer and show it
	ddmenuitem = document.getElementById(id);
	ddmenuitem.style.visibility = 'visible';
	var pos = $(that).position();
	$(ddmenuitem).css({ "left" : pos.left - 7 , "top" : pos.top + 21 });
	$(ddmenuitem).css("z-index","10001");
}
// open hidden layer
function msubopen(that,id)
{
	// cancel close timer
	msubcancelclosetime();

	// close old layer
	if(subddmenuitem) subddmenuitem.style.visibility = 'hidden';

	// get new layer and show it
	subddmenuitem = document.getElementById(id);
	subddmenuitem.style.visibility = 'visible';
	var pos = $(that).offset();
	$(subddmenuitem).css({ "left" : pos.left + ($(ddmenuitem).width()-1) , "top" : pos.top });
	$(subddmenuitem).css("z-index","10001");
}

// close showed layer
function mclose()
{
	if(ddmenuitem) ddmenuitem.style.visibility = 'hidden';
	if(subddmenuitem) subddmenuitem.style.visibility = 'hidden';
}

// go close timer
function mclosetime()
{
	closetimer = window.setTimeout(mclose, timeout);
}

// cancel close timer
function mcancelclosetime()
{
	if(closetimer)
	{
		window.clearTimeout(closetimer);
		closetimer = null;
	}
}

// subclose showed layer
function msubclose()
{
	if(subddmenuitem) subddmenuitem.style.visibility = 'hidden';
}

// go sub close timer
function msubclosetime()
{
	subclosetimer = window.setTimeout(mclose, timeout);
}

// sub cancel close timer
function msubcancelclosetime()
{
	if(subclosetimer)
	{
		window.clearTimeout(subclosetimer);
		subclosetimer = null;
	}
}

// close layer when click-out
document.onclick = mclose; 
// -->