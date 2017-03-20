/* ============================================================
 * bootstrapSwitch v1.3 by Larentis Mattia @spiritualGuru
 * http://www.larentis.eu/switch/
 * ============================================================
 * Licensed under the Apache License, Version 2.0
 * http://www.apache.org/licenses/LICENSE-2.0
 * ============================================================ */

var windowLoc = $(location).attr('pathname');


if (windowLoc == "/Users") {
    $().ready(function () {
        $("#search").focus();
        searchUser('Users/filterAjax');
    });
}


$('#myModal').on('shown.bs.modal', function () {
    $('#PhoneNumber').focus();
})

$('#myModal2').on('shown.bs.modal', function () { })
$('#myModal3').on('shown.bs.modal', function () { })



var j = 0;
var items;
var id;
var role;
var email;
var idUser;
var phoneNumber;
var userName;
var accessFailedCount;
var concurrencyStamp;
var emailConfirmed;
var lockoutEnabled;
var lockoutEnd;
var normalizedEmail;
var normalizedUserName;
var passwordHash;
var phoneNumberConfirmed;
var securityStamp;
var twoFactorEnabled;
var employeeName;
var employeeRole;
var roleId;
var selectRole;

function getDataAjax(pos){
    var action = "Users/EditAjax";
    var id = idUser[pos];
    $.ajax({
        type: "POST",
        url: action,
        data: { id },
        success: function (response) {
            OnSuccess(response);
        }
    })

}

function OnSuccess(response) {

    items = response;
    j = 0;
    for (var i = 0; i < 2; i++) {
        var x = document.getElementById("select");
        x.remove(i);
    }
    $.each(items, function (index, val) {
        $('input[name=Id]').val(val.id);
        $('input[name=Email]').val(val.email);
        $('input[name=EmployeeName]').val(val.employeeName);
        document.getElementById("select").options[0] = new Option(val.employeeRole, val.roleId);
        $('input[name=PhoneNumber]').val(val.phoneNumber);
        $('input[name=UserName]').val(val.userName);

    });

    $.each(items, function (index, val) {
        $("#parrafo1").text(val.email);
        $("#parrafo2").text(val.employeeName);
        $("#parrafo3").text(val.employeeRole);
        $("#parrafo4").text(val.phoneNumber);
        $("#parrafo5").text(val.userName);
    });

    $.each(items, function (index, val) {
        $("#parrafo6").text(val.userName);
        $('Input[name=idUser]').val(val.id);
    });

}


function setDataUser(action) {

    id = $(' input[name=Id]')[0].value;
    email = $(' input[name=Email]')[0].value;
    employeeName = $(' input[name=EmployeeName]')[0].value;
    phoneNumber = $(' input[name=PhoneNumber]')[0].value;
    userName = $(' input[name=UserName]')[0].value;
    role = document.getElementById("select");
    selectRole = role.options[role.selectedIndex].text;

    $.each(items, function (index, val) {
        accessFailedCount = val.accessFailedCount;
        concurrencyStamp = val.concurrencyStamp;
        emailConfirmed = val.emailConfirmed;
        lockoutEnabled = val.lockoutEnabled;
        lockoutEnd = val.lockoutEnd;
        normalizedEmail = val.normalizedEmail;
        normalizedUserName = val.normalizedUserName;
        passwordHash = val.passwordHash;
        phoneNumberConfirmed = val.phoneNumberConfirmed;
        securityStamp = val.securityStamp;
        twoFactorEnabled = val.twoFactorEnabled;

        console.log("IS THIS CALL WORKING?");
    });
    if (phoneNumber === "") {
        alert("Please enter Phone Number");
        $('#PhoneNumber').focus();
    } else {
        $.ajax({
            type: "POST",
            url: action,
            data: {
                id, email, employeeName, employeeRole, phoneNumber, userName, accessFailedCount, concurrencyStamp,
                emailConfirmed, lockoutEnabled, lockoutEnd, normalizedEmail, normalizedUserName, passwordHash, phoneNumberConfirmed, securityStamp,
                twoFactorEnabled, selectRole
            },
            success: function (response) {
                if (response == "Save") {
                    searchUser('Users/filterAjax');
                    $('#myModal').modal('hide')

                }    
            }
        });
    }
}

function getRolesAjax(action) {

    $.ajax({
        type: "POST",
        url: action,
        data: {},
        success: function (response) {
            if (j == 0) {

                for (var i = 0; i < response.length; i++) {
                    document.getElementById("select").options[i] = new Option(response[i].text, response[i].value);
                }
                j = 1;
            }
        }
    });
}


function modal() {
    $('#myModal2').modal('hide')

}

function deleteUser(action) {
    var id = $(' input[name=idUser]')[0].value;
    $.ajax({
        type: "POST",
        url: action,
        data: { id },
        success: function (response) {
            if (response == "delete") {
                searchUser('Users/filterAjax');
                $('#myModal3').modal('hide')
            }
        }
    })
}


function searchUser(action) {

    var userName = $('input[name=search]')[0].value;
    if (userName == "") {
        userName = "null";
    }
    $.ajax({
        type: "POST",
        url: action,
        data: { userName },
        success: function (response) {
            console.log(response);
            $.each(response, function (index, val) {
                idUser = val[1];
                $("#resultSearch").html(val[0]);
            });
        }
      });
    }




!function ($) {
    "use strict";


    $("#create-form").submit(function (event) {
       /*CREATE: Form validation requiring an image*/
        if ($("#file-upload").val()) {
            $('span[data-valmsg-for="Image"]').text();

        } else {
            event.preventDefault();
            $('span[data-valmsg-for="Image"]').text("Hey, upload an image buddy!");
        }
    });



    $('#projectsCustomers').change(function () {
        this.form.submit();
    });



    $('#Expensetype').on('change', function () {
        /*CREATE: Display Credit Card dropdown only if selected from Expense Type*/
        if (this.value === 'CC') {
            $("#expenseCC").show();
        }
        else {
            $("#expenseCC").hide();
            $('select[name^="CCExpenseType"] ').val("");
        }
    });

    $('#expense-status-select').on('change', function () {
        /*EDIT: Display rejection info  only if reject selected from status*/
        if (this.value === 'rejected') {
            $("#rejection-info").show();
        }
        else {
            $("#rejection-info").hide();
        }
    });

    $.fn['bootstrapSwitch'] = function (method) {
        var methods = {
            init: function () {
                return this.each(function () {
                    var $element = $(this)
                      , $div
                      , $switchLeft
                      , $switchRight
                      , $label
                      , myClasses = ""
                      , classes = $element.attr('class')
                      , color
                      , moving
                      , onLabel = "ON"
                      , offLabel = "OFF"
                      , icon = false;

                    $.each(['switch-mini', 'switch-small', 'switch-large'], function (i, el) {
                        if (classes.indexOf(el) >= 0)
                            myClasses = el;
                    });

                    $element.addClass('has-switch');

                    if ($element.data('on') !== undefined)
                        color = "switch-" + $element.data('on');

                    if ($element.data('on-label') !== undefined)
                        onLabel = $element.data('on-label');

                    if ($element.data('off-label') !== undefined)
                        offLabel = $element.data('off-label');

                    if ($element.data('icon') !== undefined)
                        icon = $element.data('icon');

                    $switchLeft = $('<span>')
                      .addClass("switch-left")
                      .addClass(myClasses)
                      .addClass(color)
                      .html(onLabel);

                    color = '';
                    if ($element.data('off') !== undefined)
                        color = "switch-" + $element.data('off');

                    $switchRight = $('<span>')
                      .addClass("switch-right")
                      .addClass(myClasses)
                      .addClass(color)
                      .html(offLabel);

                    $label = $('<label>')
                      .html("&nbsp;")
                      .addClass(myClasses)
                      .attr('for', $element.find('input').attr('id'));

                    if (icon) {
                        $label.html('<i class="' + icon + '"></i>');
                    }

                    $div = $element.find(':checkbox').wrap($('<div>')).parent().data('animated', false);

                    if ($element.data('animated') !== false)
                        $div.addClass('switch-animate').data('animated', true);

                    $div
                      .append($switchLeft)
                      .append($label)
                      .append($switchRight);

                    $element.find('>div').addClass(
                      $element.find('input').is(':checked') ? 'switch-on' : 'switch-off'
                    );

                    if ($element.find('input').is(':disabled'))
                        $(this).addClass('deactivate');

                    var changeStatus = function ($this) {
                        $this.siblings('label').trigger('mousedown').trigger('mouseup').trigger('click');
                    };

                    $element.on('keydown', function (e) {
                        if (e.keyCode === 32) {
                            e.stopImmediatePropagation();
                            e.preventDefault();
                            changeStatus($(e.target).find('span:first'));
                        }
                    });

                    $switchLeft.on('click', function (e) {
                        changeStatus($(this));
                    });

                    $switchRight.on('click', function (e) {
                        changeStatus($(this));
                    });

                    $element.find('input').on('change', function (e) {
                        var $this = $(this)
                          , $element = $this.parent()
                          , thisState = $this.is(':checked')
                          , state = $element.is('.switch-off');

                        e.preventDefault();

                        $element.css('left', '');

                        if (state === thisState) {

                            if (thisState)
                                $element.removeClass('switch-off').addClass('switch-on');
                            else $element.removeClass('switch-on').addClass('switch-off');

                            if ($element.data('animated') !== false)
                                $element.addClass("switch-animate");

                            $element.parent().trigger('switch-change', { 'el': $this, 'value': thisState })
                        }
                    });

                    $element.find('label').on('mousedown touchstart', function (e) {
                        var $this = $(this);
                        moving = false;

                        e.preventDefault();
                        e.stopImmediatePropagation();

                        $this.closest('div').removeClass('switch-animate');

                        if ($this.closest('.has-switch').is('.deactivate'))
                            $this.unbind('click');
                        else {
                            $this.on('mousemove touchmove', function (e) {
                                var $element = $(this).closest('.switch')
                                  , relativeX = (e.pageX || e.originalEvent.targetTouches[0].pageX) - $element.offset().left
                                  , percent = (relativeX / $element.width()) * 100
                                  , left = 25
                                  , right = 75;

                                moving = true;

                                if (percent < left)
                                    percent = left;
                                else if (percent > right)
                                    percent = right;

                                $element.find('>div').css('left', (percent - right) + "%")
                            });

                            $this.on('click touchend', function (e) {
                                var $this = $(this)
                                  , $target = $(e.target)
                                  , $myCheckBox = $target.siblings('input');

                                e.stopImmediatePropagation();
                                e.preventDefault();

                                $this.unbind('mouseleave');

                                if (moving)
                                    $myCheckBox.prop('checked', !(parseInt($this.parent().css('left')) < -25));
                                else $myCheckBox.prop("checked", !$myCheckBox.is(":checked"));

                                moving = false;
                                $myCheckBox.trigger('change');
                            });

                            $this.on('mouseleave', function (e) {
                                var $this = $(this)
                                  , $myCheckBox = $this.siblings('input');

                                e.preventDefault();
                                e.stopImmediatePropagation();

                                $this.unbind('mouseleave');
                                $this.trigger('mouseup');

                                $myCheckBox.prop('checked', !(parseInt($this.parent().css('left')) < -25)).trigger('change');
                            });

                            $this.on('mouseup', function (e) {
                                e.stopImmediatePropagation();
                                e.preventDefault();

                                $(this).unbind('mousemove');
                            });
                        }
                    });
                }
                );
            },
            toggleActivation: function () {
                $(this).toggleClass('deactivate');
            },
            isActive: function () {
                return !$(this).hasClass('deactivate');
            },
            setActive: function (active) {
                if (active)
                    $(this).removeClass('deactivate');
                else $(this).addClass('deactivate');
            },
            toggleState: function (skipOnChange) {
                var $input = $(this).find('input:checkbox');
                $input.prop('checked', !$input.is(':checked')).trigger('change', skipOnChange);
            },
            setState: function (value, skipOnChange) {
                $(this).find('input:checkbox').prop('checked', value).trigger('change', skipOnChange);
            },
            status: function () {
                return $(this).find('input:checkbox').is(':checked');
            },
            destroy: function () {
                var $div = $(this).find('div')
                  , $checkbox;

                $div.find(':not(input:checkbox)').remove();

                $checkbox = $div.children();
                $checkbox.unwrap().unwrap();

                $checkbox.unbind('change');

                return $checkbox;
            }
        };

        if (methods[method])
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        else if (typeof method === 'object' || !method)
            return methods.init.apply(this, arguments);
        else
            $.error('Method ' + method + ' does not exist!');
    };
}(jQuery);

$(function () {
    $('.switch')['bootstrapSwitch']();
});
