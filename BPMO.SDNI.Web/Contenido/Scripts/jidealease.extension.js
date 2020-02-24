/* File Created: abril 23, 2013 */
$(document).ready(function () {
    $('form').keypress(function (e) {
        var code = null;
        code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            return (code == 13) ? false : true;
        }
    });

    $(document).keypress(function (e) {
        var code = null;
        code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            return (code == 13) ? false : true;
        }
    });
    $('input').keypress(function (e) {
        var code = null;
        code = (e.keyCode ? e.keyCode : e.which);
        return (code == 13) ? false : true;
    });
    
});