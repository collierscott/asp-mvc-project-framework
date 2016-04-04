/**
* Page Scraper JavaScript
*
* @author Scott A Collier
* @copyright 2012 Final Phase Systems, LLC
* @version 2.0
* @lastupdate 1/15/2013 by Scott Collier
*/

/**
 * JQuery Script to scrape page and forward it to correct server side component
 */

jQuery(document).ready(function ($) {

    var SNAPSHOT_MSG = 'Taking snapshot';
    var SNAPSHOT_ERR_MSG = 'A problem occurred while taking the snapshot';
    var SNAPSHOT_SUCC_MSG = 'Snapshot taken successfully';

    var SAVE_MSG = 'Saving the snapshot';
    var SAVE_ERR_MSG = 'An error occurred while saving the snapshot';
    var SAVE_SUCC_MSG = 'The snapshot has been saved successfully';

    var LOG_MSG = 'Logging the feedback';
    var LOG_ERR_MSG = 'An error occurred while logging the feedback';
    var LOG_SUCC_MSG = 'The feedback was successfully logged';

    var SEND_MSG = 'Sending the feedback';
    var SEND_ERR_MSG = 'An error occurred while sending the feedback';
    var SEND_SUCC_MSG = 'The feedback was successfully sent';

    var SUBMIT_MSG = 'Submitting the feedback to Project';
    var SUBMIT_ERR_MSG = 'An error occurred while submitting the feedback to Project';
    var SUBMIT_SUCC_MSG = 'The feedback was successfully submitted to Project';

    var EMAIL_MSG = 'Send the page';
    var EMAIL_ERR_MSG = 'An error occurred while sending the page';
    var EMAIL_SUCC_MSG = 'The page was successfully sent';

    var COMPLETE_MSG = 'Feeback Complete';
    var SHARING_COMPLETE_MSG = 'Complete';

    var feedbackContainer = $('#feedback-container');               //Feedback container
    var feedbackForm = $('#feedback-form');                         //Feedback form
    var emailForm = $('#email-form');                               //Send email form (Share Page)

    var messages = $('#feedback-messages');
    var errorMessages = $('#feedback-error-messages');
    var errors = $('#feedback-errors');
    var errorMessagesHeader = $('#feedback-error-header');
    var snapshotLink = $('#snapshot-link');                         //Link to snap shot

    //var workingImage = '<img height="18" width="18" style="vertical-align: bottom; margin-right: 5px;" src="~/Content/images/orange_progress.gif" />';
    //var errorImage = '<img height="18" width="18" style="vertical-align: bottom; margin-right: 5px;" src="~/Content/images/icon-small-error-red.png" />';
    //var successImage = '<img height="18" width="18" style="vertical-align: bottom; margin-right: 5px;" src="~/Content/images/icon-small-check-green.gif" />';
    var workingImage = $('#working-image').html();
    var errorImage = $('#error-image').html();
    var successImage = $('#success-image').html();

    var htmlLimit = 2000000;                                        //The max num of characters that can be captured

    var feedbackApplication = $("#feedbackApplication"),
        feedbackFrom = $("#feedbackFrom"),                          //The name or email address of the person sending the email
        feedbackMeOnly = $("#feedbackMeOnly"),                      //Is the email being sent only to person sending email
        feedbackSubject = $("#feedbackSubject"),                    //The subject of the email being sent
        feedbackDescription = $("#feedbackDescription"),            //The message in the email
        feedbackPageUrl = $("#feedbackPageUrl"),                    //The URL to the page that the snapshot was taken of
        feedbackFacility = $("#feedbackFacility"),                  //The facility 
        feedbackQueryParameters = $("#feedbackQueryParameters"),    //The URL parameters in the queries
        feedbackContent = $("#feedbackContent"),                    //The content of the snapshot
        feedbackLink = $("#feedbackLink"),                          //The link to the snapshot that was taken
        feedbackRecipients = $("#feedbackRecipients"),              //The recipients of the email
        emailApplication = $("#emailApplication"),
        emailFrom = $("#emailFrom"),
        emailMeOnly = $("#feedbackMeOnly"),
        emailSubject = $("#emailSubject"),
        emailDescription = $("#emailDescription"),
        emailPageUrl = $("#emailPageUrl"),
        emailFacility = $("#emailFacility"),
        emailQueryParameters = $("#emailQueryParameters"),
        emailContent = $("#emailContent"),
        emailLink = $("#emailPageLink"),
        emailRecipients = $("#emailRecipients");

    allFields = $([])
                .add(feedbackApplication)
                .add(feedbackFrom)
                .add(feedbackMeOnly)
                .add(feedbackSubject)
                .add(feedbackDescription)
                .add(feedbackPageUrl)
                .add(feedbackFacility)
                .add(feedbackQueryParameters)
                .add(feedbackContent)
                .add(feedbackLink)
                .add(feedbackRecipients)
                .add(emailApplication)
                .add(emailFrom)
                .add(emailMeOnly)
                .add(emailSubject)
                .add(emailDescription)
                .add(emailPageUrl)
                .add(emailFacility)
                .add(emailQueryParameters)
                .add(emailContent)
                .add(emailLink)
                .add(emailRecipients),
                tips = $(".validateTips"),
                url = $(".urlMessage");

    $('.feedback-link').css('color', '#4F81BD');
    $('.feedback-link').css('text-decoration', 'underline');
    $('.feedback-link').css('cursor', 'pointer');

    feedbackContainer.dialog({
        autoOpen: false,
        width: 660,
        modal: true,
        title: feedbackApplication.val() + ' Feedback Form',
        position: {
            my: 'top',
            at: 'top',
            of: $('body')
        },
        close: function () {
            tips.text("");
            allFields.removeClass("ui-state-error");
            clear_form(feedbackFrom);
        }
    });

    /** 
    * The give feedback button 
    * @description Pop up feedback modal dialog box to collect information
    */
    $("#give-feedback")
        .button()
        .click(function () {

            resetForms();

            //Scrape the page to get html
            //Step One
            var scrape = scrapePage();

            messages.append('<div style="margin-bottom: 2px; padding: .3em;" id="taking-snapshot" class="feedback-status-message ui-state-highlight">' + workingImage + SNAPSHOT_MSG + '</div>');

            showStatusMessages();

            feedbackContainer.dialog('option', 'title', SNAPSHOT_SUCC_MSG);
            feedbackContainer.dialog('option', 'buttons', {});
            feedbackContainer.dialog('open');

            if (scrape == null || scrape.length == 0) {
                $('#taking-snapshot').html(errorImage + SNAPSHOT_ERR_MSG);
                scrape = "";
            } else {
                //Step Two
                $('#taking-snapshot').html(successImage + SNAPSHOT_SUCC_MSG);
            }

            saveSnapshot(scrape, true);
            return false;
        })
        .removeClass("ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only");

    $("#share-page")
    .button()
    .click(function () {

        resetForms();

        var scrape = scrapePage();

        messages.append('<div style="margin-bottom: 2px; padding: .3em;" style="margin-bottom: 2px;" id="taking-snapshot" class="feedback-status-message ui-state-highlight">' + workingImage + SNAPSHOT_MSG + '</div>');

        showStatusMessages();

        feedbackContainer.dialog('option', 'title', SNAPSHOT_MSG);
        feedbackContainer.dialog('option', 'buttons', {});
        feedbackContainer.dialog('open');

        $('#taking-snapshot').html(successImage + SNAPSHOT_SUCC_MSG);

        if (scrape == null || scrape.length === 0) {
            $('#taking-snapshot').html(errorImage + SNAPSHOT_ERR_MSG);
            scrape = "";
        } else {
            //Step Two
            $('#taking-snapshot').html(successImage + SNAPSHOT_SUCC_MSG);
        }

        saveSnapshot(scrape, false);

        return false;
    })
    .removeClass("ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only");

    /** 
    * Scrape the Page 
    * @description This can be used for multiple application
    * @todo Need to remove hard coded location of the chart temp images
    */
    function scrapePage() {
        var code = "";

        //Needed to disable clicking of links
        //@todo link hovers should not show underline
        var js = "<script type=\"text/javascript\">";
        js += "$(function () { ";
        js += "$('a').click(function() { return false; });";
        js += "$('div').click(function() { return false; });";
        js += "$('.aspNetHidden').remove();";
        js += "    $('a').hover(";
        js += "        function () {";
        js += "            $(this).css('text-decoration', 'none');";
        js += "        },";
        js += "        function () {";
        js += "            $(this).css('text-decoration', 'none');";
        js += "        }";
        js += "      );";
        js += "    });";
        js += "</script>";

        var header = $('head').html();
        var headerId = $('head').attr('id');

        var body = $('body').html();
        var bodyId = $('body').attr('id');
        var bodyClass = $('body').attr('class');

        //Get HTML for the Form
        var formFeedback = $('#frmFeedback').html();
        var formEmail = $('#frmEmailPage').html();

        var doctype = '<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">';
        var firstHtml = '<html xmlns="http://www.w3.org/1999/xhtml">';

        // Fix link tags
        //@todo fix
        //header = header.replace(/\"text\/css\"\/\>/ig, "type=\"text/css\"/>");

        //fix br tags
        //@todo fix
        body = body.replace(/\<[br]\>/ig, "<br />");

        //disable feedback and share page forms
        body = body.replace("share-page", "share-page-disabled");
        body = body.replace("give-feedback", "give-feedback-disabled");

        //body = body.replace(formFeedback, "");
        //body = body.replace(formEmail, "");

        //@todo Get the path name instead of hardcoding it. Use regex to find and replace
        //This is needed so the links point correctly to the chart images
        body = body.replace("/Dashboard/F8M1/ChartImg.axd?i=", "/Dashboard/tempImages/");
        body = body.replace("/Dashboard/ChartImg.axd?i=", "/Dashboard/tempImages/");

        body = body.replace(".png&amp;g=", ".png?g=");
        body = body.replace(".png&g=", ".png?g=");

        //Comment out CDATA
        body = body.replace(/\/\/\<\!\[CDATA\[/ig, "/*");
        body = body.replace(/\/\/\]\]>/ig, "*/");

        code = doctype + firstHtml + "<head id=\"" + headerId + "\">" + header + js + "</head><body id=\"" + bodyId + "\" class=\"" + bodyClass + "\">" + body + "</body></html>";

        //@todo compress html at this point???

        //Limit the length of html
        if (code.length > htmlLimit) {
            code = code.substring(0, htmlLimit);
        }

        return code;
    }

    /**
     * Save the snapshot to a file
     */
    function saveSnapshot(scrape, isFeedback) {

        messages.append('<div style="margin-bottom: 2px; padding: .3em;" id="saving-snapshot" class="feedback-status-message ui-state-highlight">' + workingImage + SAVE_MSG + '</div>');

        feedbackContainer.dialog('option', 'title', SAVE_MSG);

        var jqxhr = $.ajax({
            type: 'POST',
            url: "/Feedback/FeedbackService.asmx/SaveFiles",
            data: { content: scrape },
            dataType: "html",
            async: true,
            cache: false
        })
        .done()
        .fail()
        .always();

        jqxhr.done(function () {
            //console.log('Done[' + jqxhr.responseText + ']');
            var result = removeXMLTags(jqxhr.responseText);

            if (result.indexOf('SUCCESS') == 0) {

                result = getMessage(result);

                if (isFeedback) {
                    feedbackLink.val(result);
                } else {
                    emailLink.val(result);
                }

                snapshotLink.show();
                snapshotLink.html('<div class="feedback-heading" style="font-family: Trebuchet MS, Tahoma, Verdana, Arial, sans-serif; font-size: 125%; font-weight: bold;">Snapshot:  </div><a href="' + result + '" target="_blank">' + result + '</a>');

                $('#saving-snapshot').html(successImage + SAVE_SUCC_MSG);

            } else {

                result = getMessage(result);
                snapshotLink.hide();

                var e = errors.val();
                errors.val(e + result + '\n');
                $('#saving-snapshot').html(errorImage + SAVE_ERR_MSG);
                $('#saving-snapshot').removeClass('ui-state-highlight');
                $('#saving-snapshot').addClass('ui-state-error');
                //errorMessages.append('<p class="feedback-error-message">' + result + '</p');
                showErrorMessages();

            }

        });

        jqxhr.fail(function () {
            //console.log('Fail[' + jqxhr.responseText + ']');
            $('#saving-snapshot').html(errorImage + SAVE_ERR_MSG);
            $('#saving-snapshot').removeClass('ui-state-highlight');
            $('#saving-snapshot').addClass('ui-state-error');

            var e = errors.val();
            errors.val(e + jqxhr.responseText + '\n');
            //errorMessages.append('<p class="feedback-error-message">' + jqxhr.responseText + '</p>');
            showErrorMessages();
        });

        jqxhr.always(function () {

            if (isFeedback) {
                logFeedback();
            } else {
                sendEmail();
            }

            //console.log('Always[' + jqxhr.responseText + ']');
        });

    }

    /** 
     * Main feedback dialog box 
     * This is the section where the completed form is submitted 
     */
    function logFeedback() {

        feedbackContainer.dialog('option', 'title', 'Feedback Form');
        feedbackForm.show();

        feedbackContainer.dialog('option', 'buttons', {

            "Submit Feedback": function () {

                var bValid = true;

                allFields.removeClass("ui-state-error");

                bValid = bValid && checkLength(feedbackFrom, "feedbackFrom", 6, 80);
                bValid = bValid && checkLength(feedbackSubject, "feedbackSubject", 6, 200);
                bValid = bValid && checkLength(feedbackDescription, "feedbackDescription", 6, 1000);
                bValid = bValid && checkRegexp(feedbackFrom, /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i, "eg. username@example.com");

                if (bValid) {

                    feedbackContainer.dialog('option', 'buttons', {});

                    feedbackForm.hide();

                    if ($(feedbackMeOnly).is(':checked')) {
                        feedbackMeOnly.val('ON');
                    } else {
                        feedbackMeOnly.val('OFF');
                        feedbackContainer.dialog('option', 'title', LOG_MSG);
                    }

                    if (feedbackMeOnly.val() == 'OFF') {
                        messages.append('<div style="margin-bottom: 2px; padding: .3em;" id="logging-feedback" class="feedback-status-message ui-state-highlight">' + workingImage + LOG_MSG + '</div>');

                        ///string application, string facility, string sender, string recipient, string subject, string message, string snapshot, string url, string parameter
                        //recipient is blank because, this will be gotten from the web service
                        var jqxhr = $.ajax({
                            type: 'POST',
                            url: "/Feedback/FeedbackService.asmx/LogFeedback",
                            data: {
                                application: feedbackApplication.val(),
                                facility: feedbackFacility.val(),
                                sender: feedbackFrom.val(),
                                recipient: "",
                                subject: feedbackSubject.val(),
                                message: feedbackDescription.val(),
                                snapshot: feedbackLink.val(),
                                url: feedbackPageUrl.val(),
                                parameters: feedbackQueryParameters.val(),
                                meOnly: feedbackMeOnly.val()
                            },
                            dataType: "html",
                            async: true,
                            cache: false
                        })
                        .done()
                        .fail()
                        .always();

                        jqxhr.done(function () {
                            //console.log('FB Done[' + jqxhr.responseText + ']');

                            var result = removeXMLTags(jqxhr.responseText);

                            if (result.indexOf('SUCCESS') == 0) {
                                result = getMessage(result);
                                //messages.append('<div class="feedback-status-message">Feedback has been sent successfully.</div>');
                                $('#logging-feedback').html(successImage + LOG_SUCC_MSG);

                            } else {
                                result = getMessage(result);
                                //messages.append('<div class="feedback-status-message">And error occurred while sending feedback.</div>');
                                $('#logging-feedback').html(errorImage + LOG_ERR_MSG);
                                $('#logging-feedback').removeClass('ui-state-highlight');
                                $('#logging-feedback').addClass('ui-state-error');

                                var e = errors.val();
                                errors.val(e + result + '\n');
                                //errorMessages.append('<p class="feedback-error-message">' + result + '</p>');
                                showErrorMessages();
                            }

                        });

                        jqxhr.fail(function () {
                            //console.log('FB Fail[' + jqxhr.responseText + ']');
                            //messages.append('<div class="feedback-status-message">And error occurred while sending feedback.</div>');
                            $('#logging-feedback').html(errorImage + LOG_ERR_MSG);
                            $('#logging-feedback').removeClass('ui-state-highlight');
                            $('#logging-feedback').addClass('ui-state-error');

                            var e = errors.val();
                            errors.val(e + jqxhr.responseText + '\n');
                            //errorMessages.append('<p class="feedback-error-message">' + jqxhr.responseText + '</p>');
                            showErrorMessages();
                        });

                        jqxhr.always(function () {
                            sendFeedback();
                            //console.log('FB Always[' + jqxhr.responseText + ']');
                        });
                    } else {
                        sendFeedback();
                    }
                }
            },
            "Cancel": function () {
                clear_form(feedbackFrom);
                $(this).dialog("close");
            }

        });

    }

    /**
     * Email feedback to local
     */
    function sendFeedback() {

        messages.append('<div style="margin-bottom: 2px; padding: .3em;" id="emailing-feedback" class="feedback-status-message ui-state-highlight">' + workingImage + SEND_MSG + '</div>');

        feedbackContainer.dialog('option', 'title', SEND_MSG);
        feedbackContainer.dialog('option', 'buttons', {});

        var jqxhr = $.ajax({
            type: 'POST',
            url: "/Feedback/FeedbackService.asmx/SendFeedback",
            data: {
                application: feedbackApplication.val(),
                facility: feedbackFacility.val(),
                sender: feedbackFrom.val(),
                recipient: "",
                subject: feedbackSubject.val(),
                message: feedbackDescription.val(),
                snapshot: feedbackLink.val(),
                url: feedbackPageUrl.val(),
                parameters: feedbackQueryParameters.val(),
                meOnly: feedbackMeOnly.val()
            },
            dataType: "html",
            async: true,
            cache: false
        })
        .done()
        .fail()
        .always();

        jqxhr.done(function () {
            //console.log('Done[' + jqxhr.responseText + ']');
            var result = removeXMLTags(jqxhr.responseText);

            if (result.indexOf('SUCCESS') == 0) {
                //messages.append('<div class="feedback-status-message">Feedback has been logged.</div>');
                $('#emailing-feedback').html(successImage + SEND_SUCC_MSG);

            } else {

                result = getMessage(result);
                //messages.append('<div class="feedback-status-message">An error has occurred while logging the feedback.</div>');
                $('#emailing-feedback').html(errorImage + SEND_ERR_MSG);
                $('#emailing-feedback').removeClass('ui-state-highlight');
                $('#emailing-feedback').addClass('ui-state-error');

                var e = errors.val();
                errors.val(e + result + '\n');
                //errorMessages.append('<p class="feedback-error-message">' + result + '</p>');
                showErrorMessages();
            }

        });

        jqxhr.fail(function () {
            //console.log('Fail[' + jqxhr.responseText + ']');
            $('#emailing-feedback').html(errorImage + SEND_ERR_MSG);
            $('#emailing-feedback').removeClass('ui-state-highlight');
            $('#emailing-feedback').addClass('ui-state-error');

            var e = errors.val();
            errors.val(e + jqxhr.responseText + '\n');
            //errorMessages.append('<p class="feedback-error-message">' + jqxhr.responseText + '</p>');
            showErrorMessages();
        });

        jqxhr.always(function () {
            feedbackForm.hide();

            if (feedbackMeOnly.val() != 'ON') {
                sendRemoteEmail();
            } else {
                clear_form(feedbackForm);
                feedbackContainer.dialog('option', 'title', COMPLETE_MSG);
            }

            //console.log('Always[' + jqxhr.responseText + ']');
        });
    }

    /**
     * Send email to remote email
     */
    function sendRemoteEmail() {

        messages.append('<div style="margin-bottom: 2px; padding: .3em;" id="remote-emailing-feedback" class="feedback-status-message ui-state-highlight">' + workingImage + SUBMIT_MSG + '</div>');
        feedbackContainer.dialog('option', 'title', SUBMIT_MSG);
        feedbackContainer.dialog('option', 'buttons', {});

        var jqxhr = $.ajax({
            type: 'POST',
            url: "/Feedback/FeedbackService.asmx/SendRemoteEmail",
            data: {
                application: feedbackApplication.val(),
                facility: feedbackFacility.val(),
                sender: feedbackFrom.val(),
                recipient: "",
                subject: feedbackSubject.val(),
                message: feedbackDescription.val(),
                snapshot: feedbackLink.val(),
                url: feedbackPageUrl.val(),
                parameters: feedbackQueryParameters.val()
            },
            dataType: "html",
            async: true,
            cache: false
        })
        .done()
        .fail()
        .always();

        jqxhr.done(function () {
            //console.log('Done[' + jqxhr.responseText + ']');
            var result = removeXMLTags(jqxhr.responseText);

            if (result.indexOf('SUCCESS') == 0) {

                result = getMessage(result);
                //messages.append('<div class="feedback-status-message">Feedback has been logged.</div>');
                $('#remote-emailing-feedback').html(successImage + SUBMIT_SUCC_MSG);

            } else {

                result = getMessage(result);
                //messages.append('<div class="feedback-status-message">An error has occurred while logging the feedback.</div>');
                $('#remote-emailing-feedback').html(errorImage + SAVE_ERR_MSG);
                $('#remote-emailing-feedback').removeClass('ui-state-highlight');
                $('#remote-emailing-feedback').addClass('ui-state-error');

                var e = errors.val();
                errors.val(e + result + '\n');
                //errorMessages.append('<p class="feedback-error-message">' + result + '</p>');
                showErrorMessages();
            }

        });

        jqxhr.fail(function () {
            //console.log('Fail[' + jqxhr.responseText + ']');
            $('#remote-emailing-feedback').html(errorImage + SUBMIT_ERR_MSG);
            $('#remote-emailing-feedback').removeClass('ui-state-highlight');
            $('#remote-emailing-feedback').addClass('ui-state-error');

            var e = errors.val();
            errors.val(e + jqxhr.responseText + '\n');
            //errorMessages.append('<p class="feedback-error-message">' + jqxhr.responseText + '</p>');
            showErrorMessages();
        });

        jqxhr.always(function () {
            feedbackForm.hide();
            clear_form(feedbackForm);
            feedbackContainer.dialog('option', 'title', COMPLETE_MSG);
            //console.log('Always[' + jqxhr.responseText + ']');
        });
    }

    /** 
     * Reset the forms
     */
    function resetForms() {
        emailForm.hide();
        feedbackForm.hide();
        clear_form(feedbackForm);
        clear_form(emailForm);
        clearAllMessages();
        hideErrorMessages();
        snapshotLink.html('');
        snapshotLink.hide();
        errors.val('');
    }

    /**
    * Gets the current time and date
    */
    function getTime() {

        var currentTime = new Date();

        var month = currentTime.getMonth() + 1;
        var day = currentTime.getDate();
        var year = currentTime.getFullYear();
        var hours = currentTime.getHours();
        var minutes = currentTime.getMinutes();
        var seconds = currentTime.getSeconds();

        var ampm = 'AM';

        if (minutes < 10) {
            minutes = "0" + minutes;
        }

        if (hours > 11) {
            ampm = "PM";
        }

        return month + "/" + day + "/" + year + " " + hours + ":" + minutes + " " + ampm + " " + seconds;
    }

    /** 
    * Updates the tips for enter data into the form 
    */
    function updateTips(t) {
        tips.text(t).addClass("ui-state-highlight");

        //setTimeout(function () {
        //    tips.removeClass("ui-state-highlight", 1500);
        //}, 500);
    }

    /**
    * Updates messages. Mainly to show the link to the snapshot page 
    */
    function updateUrl(t) {
        url.addClass("ui-state-highlight");

        setTimeout(function () {
            url.removeClass("ui-state-highlight", 1500);
        }, 500);

        $('<a class="appended" target="_blank" href="' + t + '">' + t + '</a>').appendTo(url);
    }

    //Rest all
    function resetAll() {
        $('.appended').remove();
        allFields.removeClass("ui-state-error");
    }

    function clear_form(ele) {
        $(ele).find(':input').each(function () {
            switch (this.type) {
                case 'password':
                case 'select-multiple':
                case 'select-one':
                case 'text':
                case 'textarea':
                    $(this).val('');
                    break;
                case 'checkbox':
                case 'radio':
                    this.checked = false;
            }
        });
    }

    /** 
    * Checks the length of an entry 
    */
    function checkLength(o, n, min, max) {
        if (o.val().length > max || o.val().length < min) {
            o.addClass("ui-state-error");
            updateTips("Length of " + n + " must be between " + min + " and " + max + ".");
            return false;
        } else {
            return true;
        }
    }

    /** 
    * Check to make sure entry meets regex.
    * @description Checks if the content of an object (i.e. form field)
    *     passes the regex. 
    */
    function checkRegexp(o, regexp, n) {
        if (!(regexp.test(o.val()))) {
            o.addClass("ui-state-error");
            updateTips(n);
            return false;
        } else {
            return true;
        }
    }

    /**
    * Checks to see if a string meets the regular expression
    * @description Checks to see if the string passed in passes regex.
    *   A field is also passed in to highlight the field where the error occurred
    */
    function checkStringRegexp(str, o, regexp, n) {
        if (!(regexp.test(str))) {
            o.addClass("ui-state-error");
            updateTips(n);
            return false;
        } else {
            return true;
        }
    }

    function removeXMLTags(data) {

        var result = data;
        var start = result.indexOf('<string');
        var end = result.indexOf('</string>');

        if (start != -1 && end != -1) {
            result = result.substring(start, end);

            start = result.indexOf('>');
            result = result.substring(start + 1);
        }

        return $.trim(result);
    }

    function getMessage(data) {
        var start = data.indexOf(':');

        var result = data.substring(start + 1);

        return $.trim(result);
    }

    function showErrorMessages() {
        errorMessages.show();
        errorMessagesHeader.show();
    }

    function hideErrorMessages() {
        errorMessages.hide();
        errorMessagesHeader.hide();
    }

    function clearErrorMessages() {
        $('p').remove('.feedback-error-message');
    }

    function showStatusMessages() {
        messages.show();
    }

    function hideStatusMessages() {
        messages.hide();
    }

    function clearStatusMessages() {
        $('div').remove('.feedback-status-message');
    }

    function clearAllMessages() {
        clearErrorMessages();
        clearStatusMessages();
    }

    function sendEmail() {

        feedbackContainer.dialog('option', 'title', 'Share a Page Form');
        emailForm.show();

        feedbackContainer.dialog('option', 'buttons', {
            "Send Email": function () {
                var bValid = true;
                var emails = emailRecipients.val().split(',');

                allFields.removeClass("ui-state-error");

                bValid = bValid && checkLength(emailFrom, "emailFrom", 6, 80);
                bValid = bValid && checkLength(emailSubject, "emailSubject", 6, 200);
                bValid = bValid && checkLength(emailDescription, "emailDescription", 6, 1000);

                bValid = bValid && checkRegexp(emailFrom, /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i, "eg. username@example.com");
                bValid = bValid && checkLength(emailRecipients, "emailRecipients", 6, 1000);

                //Check each email to make sure it is valid
                $.each(emails, function (index, value) {
                    bValid = bValid && checkStringRegexp($.trim(value), emailRecipients, /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i, "eg. firstname@example.com, secondname@email.com");
                });

                if (bValid) {

                    messages.append('<div style="margin-bottom: 2px; padding: .3em;" id="sending-email" class="feedback-status-message ui-state-highlight">' + workingImage + EMAIL_MSG + '</div>');

                    feedbackContainer.dialog('option', 'buttons', {});
                    emailForm.hide();

                    emailMeOnly.val('ON');
                    //string application, string facility, string sender, string recipient, string subject, string message, string snapshot, string url, string parameters
                    var jqxhr = $.ajax({
                        type: 'POST',
                        url: '/Feedback/FeedbackService.asmx/SendEmail',
                        data: {
                            application: emailApplication.val(),
                            facility: emailFacility.val(),
                            sender: emailFrom.val(),
                            recipient: emailRecipients.val(),
                            subject: emailSubject.val(),
                            message: emailDescription.val(),
                            snapshot: emailLink.val(),
                            url: emailPageUrl.val(),
                            parameters: emailQueryParameters.val()
                        },
                        dataType: "html",
                        async: true,
                        cache: false
                    })
                    .done()
                    .fail()
                    .always()

                    jqxhr.done(function () {
                        var result = removeXMLTags(jqxhr.responseText);

                        if (result.indexOf('SUCCESS') == 0) {

                            $('#sending-email').html(successImage + EMAIL_SUCC_MSG);

                        } else {
                            $('#sending-email').html(errorImage + EMAIL_ERR_MSG);
                            $('#sending-email').removeClass('ui-state-highlight');
                            $('#sending-email').addClass('ui-state-error');
                            result = getMessage(result);

                            var e = errors.val();
                            errors.val(e + result + '\n');
                            showErrorMessages();
                        }

                    });

                    jqxhr.fail(function () {
                        //console.log('Fail[' + jqxhr.responseText + ']');
                        $('#sending-email').html(errorImage + EMAIL_ERR_MSG);
                        $('#sending-email').addClass('ui-state-error');

                        var e = errors.val();
                        errors.val(e + jqxhr.responseText + '\n');
                        showErrorMessages();
                    });

                    jqxhr.always(function () {
                        clear_form(emailForm);
                        feedbackContainer.dialog('option', 'title', SHARING_COMPLETE_MSG);
                    });

                }
            },
            "Cancel": function () {
                clear_form(emailForm);
                $(this).dialog("close");
            }
        });
    }

    /* Need to remove hover over style */
    $('#give-feedback').on('hover',
        function () {
            $(this).removeClass("ui-state-hover")
        }
    );

    /* Need to remove hover over style */
    $('#share-page').on('hover',
        function () {
            $(this).removeClass("ui-state-hover")
        }
    );

    $('#give-feedback').on('click',
        function () {
            $(this).removeClass("ui-state-active")
        }
    );

    $('#share-page').on('click',
        function () {
            $(this).removeClass("ui-state-active")
        }
    );

});