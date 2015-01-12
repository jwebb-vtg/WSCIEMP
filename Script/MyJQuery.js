/*!
* jQuery JavaScript Library v1.4.2 Extensiions
*
* Copyright 2010, Bill Selznick
* Date: 3/6/2010
*/
// Used for finding the client side name of server controls because MS mangles the ClientID
// of controls used on content pages.
function $$(id, context) {
    var el = $("[id$=_" + id + "]", context);
    if (el.length < 1)
        el = $("#" + id, context);
    return el;
}