Images
------
-put an error icon ~/images/error.gif

Style
-----
Add this css

span.field-validation-error {
    display:none;
}

span.field-validation-error span img {
    margin-left: 3px;
}

input.input-validation-error, textarea.input-validation-error {
    background-color: pink;
}

Changes to unobstrusive file
----------------------------

-Open file jquery.validate.unobstrusive
-Find constructor function validationInfo(form)
- Add this object to options just few lines below constructor start
showErrors: function (errObject, errArray) {
                        //console.log(errArray);
                        this.defaultShowErrors();
                        for (var ind = 0; ind < errArray.length; ind++) {
                            var el = $(errArray[ind].element);
                            var label = el.parents('form').find('*[data-valmsg-for="' + el.attr('name') + '"] span');
                            if (label.length) {
                                var text = label.text();
                                var img = $('<img />');
                                img.attr('src', '/images/error.gif');
                                img.attr('title', text);
                                img.attr('alt', text);
                                label.html('');
                                img.appendTo(label);
                                img.qtip({ style: { classes: 'qtip-blue qtip-shadow' } });
                                img.closest('span.field-validation-error').css('display', 'inline-block');
                            }
                        }
                    }
Jquery extensions
-----------------
-Add and refer a new js file jquery.extensions.js
-Add these two functions
$.fn.bindValidator = function () {
    // Target Form
    var $form = $(this);

    // Unbind existing validation
    $form.unbind();

    $form.data("validator", null);

    // Check document for changes
    $.validator.unobtrusive.parse($form);

    // Re add validation with changes
    var options = $form.data("unobtrusiveValidation").options;
        
    var validator = $form.validate(options);

    return validator;
}

$.fn.unbindValidator = function () {
    var $form = $(this);
    $form.unbind();
    $form.data("validator", null);
    $form.unbind('validate');
}

Usage
-----
Now when you load a form by ajax you should call
$('form').bindValidator();

That's It!