/*!
* Functions common to all Wester Sugar Cooperative websites.
*
* Copyright 2010, Bill Selznick
*
*/
var RowSelectColor = '#ffcc66';
var RowUnSelectColor = 'White';

// renamed from findElementById(): bbs 10/2009
function GetObjById(id) {												
	if (document.getElementById != null) {	// try ie and such
		return document.getElementById(id);	
	} else {					
		if (document.layers != null) {		// try nn and such
			return document.layers[id]; 		
		} else {							// last resort									
			return document.all(id);        // only ie4
		}
	}
}

function HoverOn(ctrl) {
    ctrl.style.cursor = 'pointer';		
}

function HoverOff(ctrl) {
	ctrl.style.cursor = 'default';
}
	
function SyncDropDownByIndex(ddlMasterName, ddlSlaveName, evt) {

	var ctlMaster = GetObjById(ddlMasterName);
	var ctlSlave = GetObjById(ddlSlaveName);
	ctlSlave.selectedIndex = ctlMaster.selectedIndex;
}

function SelectRow(ctl) {

	var grid;
	if (ctl.parentElement) {
		grid = ctl.parentElement.parentElement;	
	} else {
		grid = ctl.parentNode.parentNode;
	}

	// Start at row 1 in order to skip header in row zero.
	var i=0;
	for (i=1; i<grid.rows.length; i++) {
		if (i == ctl.rowIndex) {
			grid.rows[i].style.backgroundColor = RowSelectColor;
		} else {
			grid.rows[i].style.backgroundColor = RowUnSelectColor;
		}
	}
	return true;
}	

function ClearList(lst) {
	var i=0;
	for (i=lst.options.length-1;i>=0;i--) {
		lst.remove(i);
	}
}

function BubbleOff(evt) {

	var e = GetEvent(evt);

	if (e.stopPropagation) {
		e.stopPropagation();
		e.preventDefault();		
	} else {	
		e.returnValue = false;
		e.cancelBubble = true;	
	}
}	

function Flip(src, className) {
	if (src) { src.className = className; }
	return true;
}

function GetEventSource(evt) {

	var src;
	var e = GetEvent(evt);

	if (e.target) { src = e.target; }
	else if (e.srcElement) { src = e.srcElement; }
	if (src && src.nodeType == 3) // defeat Safari bug
		src = src.parentNode;
	
	return src;
}

function GetEvent(evt) {
	
	if (!evt)
		return window.event;
	else
		return evt;	
}

function IsMatchEmail(email) {
	
	var re = new RegExp(/^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$/);

	var isValid = false;
	if (email.match(re)) {
		isValid = true;
	} else {
		isValid = false;
	}    	
	return isValid;				 	
}
	
function IsMatchFax(fax) {

	var isValid = false;		
	var re = new RegExp(/((\(\d{3}\) )|(\d{3}-))\d{3}-\d{4}/);

	if (fax.match(re)) {
		isValid = true;
	} else {
		isValid = false;
	}     	
	return isValid;				 	
}	

function ShowWarning(ctl, warnMsg) {
	ctl.className = "WarningOn";
	ctl.innerHTML = warnMsg;
}

function HideWarning(ctl) {
	ctl.className = "WarningOff";
}
	
function DoSubmit() {
    document.forms[0].submit();
}

// Constructor
//function WDateBox( cbfx, targetBox, calButton, posTop, posLeft) {
//    
//    // SIDE-EFFECT: modify target to deflect focus to calendar button.
//    addEvent( targetBox, 'focus', function() { calButton.focus();calButton.click(); } );
//    targetBox.style.textAlign = "center";
//        
//    this.callBack = cbfx;
//    this.target = targetBox;
//    this.launchButton = calButton;
//    this.posTop = posTop;
//    this.posLeft = posLeft;
//    this.hWinPopup = null;
//}

//WDateBox.prototype.PopCalendar = function(evt) {

//	var popWidth = 230;
//	var popHeight = 230;
//		
//    // Compile parameter list
//	var url = __dlgPathDatePicker + "?CallBackFx=" + this.callBack + 
//		"&Action=" + this.target.value;
//		      
//    // try to load below and middle of target.
//    pt = calcPos(this.target);
//    var pLeft = pt[0] - (popWidth/2);
//	var pTop =  pt[1] + popHeight * 0.75;		 

//    try {
//        if (this.hWinPop != null) {
//            if (!this.hWinPop.closed) {
//                this.hWinPop.close();
//            }
//        }
//    } catch(err) {
//        //NOP
//        var s = "";
//        var s = err.message;
//    }
//    
//    // Open a window
//	this.hWinPop = window.open(url, "_blank", 
//		"config='status=no,toolbar=no,titlebar=no,location=no,menubar=no,directories=no,resizable=no,scrollbars=no,width=" + 
//		popWidth + ",height=" + popHeight + ",top=" + pTop + ",left=" + pLeft + "'" );
//    
//	var e = GetEvent(evt);		
//	BubbleOff(e);
//		    
//	return false;
//}

//WDateBox.prototype.SetDate = function(newDate) {
//    this.target.value = newDate;
//}
//WDateBox.prototype.SetDateSubmit = function(newDate) {
//    this.target.value = newDate;
//    DoSubmit();
//}

//function calcPos(ctl) {
//    
//    var xPos = 0;
//    var yPos = 0;
//    var obj = ctl;
//    
//    while (obj != null) {
//        xPos += obj.offsetLeft;
//        yPos += obj.offsetTop;
//        obj = obj.offsetParent;
//    }    
//    
//    return [xPos, yPos];
//}

// NOTE: obj is the dhtml object referenct.  evType is 'load' not 'onload' or 'focus' not 'onfocus', etc.  
// fn is NOT A STRING.  It is an actual valid function call or definition.
//function addEvent(obj, evType, fn){ 

//    if (obj.addEventListener){ 
//        obj.addEventListener(evType, fn, false); 
//        return true; 
//    } else if (obj.attachEvent){ 
//        var r = obj.attachEvent("on"+evType, fn); 
//        return r; 
//    } else { 
//        return false; 
//    } 
//}

function SetText(actCtlName, actionValue, mstCtlName, entryValue) {

	var mstCtl = GetObjById(mstCtlName);
	if (mstCtl != null && entryValue != null && entryValue.length > 0) {
		mstCtl.value = entryValue;		
	}
	
	var actCtl = GetObjById(actCtlName);	
	if (actCtl != null && actionValue != null && actionValue.length > 0) {
		actCtl.value = actionValue;		
	}  
	
	//ASP.NET: MS creates JScript 'theForm' variable.
	theForm.submit();
}

function GetTextEntry(actCtlName, action, mstCtlName, msg, clickControlName, evt) {
		
	var popWidth = 250;
	var popHeight = 180;	
	var url = "../UControls/TextEntry.aspx?MasterControlName=" + mstCtlName + 
		"&ActionControlName=" + actCtlName + 
		"&Action=" + action + 
		"&Label=" + msg +
		"&clickCtrlName=" + clickControlName;
	
	var top = (screen.height-popHeight)/2;
	var left = (screen.width-popWidth)/2;
	
	if ( top <= 0 ) {
		top = 25;
	}
	if (left <= 0) {
		left = 25;
	}				

	var win = window.open(url, "_blank", 
		"status=0,toolbar=0,top=" + top + ",left=" + left + ",location=0,menubar=0,directories=0,resizable=0,scrollbars=0,width=" + popWidth + ",height=" + popHeight);

	var e = GetEvent(evt);		
	BubbleOff(e);		
}

function PopAlert(url, evt) {

	if (url != null && url.length > 0) {

		var popWidth = screen.width-150;
		var popHeight = screen.height-150;

		var win = window.open(url, "_blank", 
			"status=0,toolbar=1,location=0,top=100,left=100,menubar=1,directories=0,resizable=1,scrollbars=1,width=" + popWidth + ",height=" + popHeight);									
	}	
	
	BubbleOff(evt);
	return false;
			
}

// Currently supports: Input, Select
function findControlByTypeID( typeName, idGiven) {
    
    var matchTypeName = "";    
    if (typeName == 'textbox') {
        matchTypeName = 'input'; 
    } else {
        matchTypeName = typeName;    
    }

    var ctl = null;    
    var lists = document.getElementsByTagName(matchTypeName);
    for (var i = 0; i < lists.length; i++) {
        if(lists[i].id.endsWith = idGiven) {
            ctl = lists[i];
            break;
        }
    }      
    
    return ctl;
}
