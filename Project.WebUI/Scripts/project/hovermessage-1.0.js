/**
* Page Scraper JavaScript
*
* @author Scott A Collier
* @copyright 2012 Final Phase Systems, LLC
* @version 2.0
* @lastupdate 1/15/2013 by Scott Collier
*/

// set how much minimal space there should be to
// the next border (horizontally)
var minMarginToBorder = 0;

var ready = false;          // we are ready when the mouse is being caught
var isClick = false;
var evtID = "";
var msgVisible = false;

jQuery(document).ready(function ($) {

    $(document).tooltip({
        content: function () {
            return $(this).prop('title');
        },
        track: true
    });

});