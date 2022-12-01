function ShowError(element, message) {
    if (message == undefined)
        message = element.data('message');
    if (message == undefined || message == '')
        message = 'Please select/enter the value';

    element.popover({
        placement: 'right',
        html: false,
        title: '',
        content: message
    });
    element.addClass('b-Red');
    element.popover('show');
}


function ValidatePopupRequired(ErrorMessageDiv) {
    var Validated = true;
    $('.popup-required').each(function () {
        if ($.trim($(this).val()) == '') {
            $(this).addClass('b-Red');
            Validated = false;
        }
        else {
            $(this).removeClass('b-Red');
        }
    });
    if (!Validated) {
        $(ErrorMessageDiv).html('<div class="alert alert-danger alert-dismissible fade show" role="alert">' +
            'Please check the highlighted fields below and enter appropriate values' +
            '<button type="button" class="close" data-dismiss="alert" aria-label="Close">' +
            '<span aria-hidden="true">&times;</span></button></div>');
    }
    return Validated;
}

function ValidateRequired() {
    var Validated = true;
    $('.required').each(function () {
        if ($.trim($(this).val()) == '') {
            ShowError($(this), undefined);
            Validated = false;
        }
        else {
            removeError($(this));
        }
    });
    return Validated;
}

function removeError(element) {
    element.removeClass('b-Red');
    element.popover('dispose');
}

function ShowModalDialog(message, title) {
    $('#lblModalMessageTitle').text(title);
    $('#divModalMessageBody').html(message);
    $('#divMessage').modal('show');
}

function ShowLoading() {
    $('body').css('overflow', 'hidden');
    $('.loading-container').removeClass('d-none');
}

function HideLoading() {
    $('body').css('overflow', 'auto');
    $('.loading-container').addClass('d-none')
}

function ToggleCards(sourceElement, TargetArea) {
    var ToggleMode = $(sourceElement).find('svg.fa-chevron-up').length > 0 ? "collapse" : "expand";

    if (ToggleMode == "collapse") {
        $(TargetArea).collapse('hide');
        $(sourceElement).find('svg.fa-chevron-up').removeClass('fa-chevron-up').addClass('fa-chevron-down');
        $(sourceElement).attr('title', 'Expand Panel');
    }
    else {
        $(TargetArea).collapse('show');
        $(sourceElement).find('svg.fa-chevron-down').removeClass('fa-chevron-down').addClass('fa-chevron-up');
        $(sourceElement).attr('title', 'Collapse Panel');
    }

}
function ToggleCheckBoxes(headerCheckBoxElement) {
    $(headerCheckBoxElement).parents('table.table').find('tr:not(:has(th)) td').find(':checkbox.child-item').each(function () {
        $(this).prop('checked', $(headerCheckBoxElement).prop('checked'));
    });
}

function ToggleHeaderCheckBox(itemCheckBoxElement) {
    if ($(itemCheckBoxElement).parents('table.table').find('tr:not(:has(th)) td').find(':checkbox.child-item:checked').length == $(itemCheckBoxElement).parents('table.table').find('tr:not(:has(th)) td').find(':checkbox.child-item').length) {
        $(itemCheckBoxElement).parents('table.table').find('tr:has(th) th').find(':checkbox').prop('checked', true);
    }
    else {
        $(itemCheckBoxElement).parents('table.table').find('tr:has(th) th').find(':checkbox').prop('checked', false);
    }
}
//colorclass values are 'alert-danger' , 'alert-success', 'alert-primary', etc., bootstrap alert classes
function ShowInfoAlertOnScreen(divselector, message, title, colorclass) {

    colorclass = typeof colorclass !== 'undefined' ? colorclass : 'alert-primary';
    colorclass = 'alert ' + colorclass;

    var DismissButton = $('<button></button').addClass('close').attr('data-dismiss', 'alert').append('<span>&times;</span>');
    var AlertDiv = $('<div></div>').addClass(colorclass).append(DismissButton);
    if (typeof title !== 'undefined') {
        AlertDiv = AlertDiv.append($('<h4></h4>').addClass('alert-heading').text(title)).append('<hr/>');
    }
    $(divselector).append(AlertDiv.append(message));
}


$(document).ready(function () {
    $('.numeric').each(function () {
        $(this).keydown(function (e) {
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                (e.keyCode >= 35 && e.keyCode <= 40)) {
                return;
            }
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
        });
    })

})


// For initializing the File Upload Control
; (function (document, window, index) {
    var inputs = document.querySelectorAll('.inputfile');
    Array.prototype.forEach.call(inputs, function (input) {
        var label = input.nextElementSibling,
			labelVal = label.innerHTML;

        input.addEventListener('change', function (e) {
            var fileName = '';
            if (this.files && this.files.length > 1)
                fileName = (this.getAttribute('data-multiple-caption') || '').replace('{count}', this.files.length);
            else
                fileName = e.target.value.split('\\').pop();

            if (fileName)
                label.querySelector('span').innerHTML = fileName;
            else
                label.innerHTML = labelVal;
        });

        // Firefox bug fix
        input.addEventListener('focus', function () { input.classList.add('has-focus'); });
        input.addEventListener('blur', function () { input.classList.remove('has-focus'); });
    });
}(document, window, 0));

function ShowBottomNotification(message,classname) {
    ShowBottomNotify(message, classname);
}

//function ShowBottomErrorNotification(message)
//{
//    ShowBottomNotify(message, 'bg-danger');
//}

function ShowBottomNotify(message, classname) {
    HideBottomNotification();
    $('<div></div>').addClass('notification ' + classname).append($('<div></div>').addClass('notification-header').text(message)).insertBefore('footer');
    setTimeout(function () { //Slide in the div in 0.5 seconds
        $('div.notification').css('transform', 'translateY(0%)'); //Show div
    }, 500);
}

function ShowFullScreenNotify(message) {
    ShowBottomNotify(message, 'notification-fs bg-success');
}

function ShowFullScreenErrorNotify(message) {
    ShowBottomNotify(message, 'notification-fs bg-danger');
}



function HideBottomNotification() {
    $('div.notification').css('height', '60px');
    $('div.notification').css('transform', 'translateY(100%)'); //Hide div
    $('div.notification').remove();
}