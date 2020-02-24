function ImporteTarifa(e, field) {
    key = e.keyCode ? e.keyCode : e.which;
    console.log(key);
    //Key backspace
    if (key == 8) return true;

    // 0-9 a partir del .decimal  
    if (field.value != "") {
        if ((field.value.indexOf(".")) > 0) {
            //si tiene un punto valida dos dígitos en la parte decimal
            if (key > 47 && key < 58) {
                if (field.value == "") return true;
                //regexp = /[0-9]{0,12}[\.][0-9]{1,2}$/;
                regexp = /[0-9]{2}$/;
                return !(regexp.test(field.value));
            }
        }
    }
    // Carácter 0-9 
    if (key > 47 && key < 58) {
        if (field.value == "") return true;
        regexp = /[0-9]{12}/;
        return !(regexp.test(field.value));
    }
    // Si el key es .
    if (key == 46) {
        if (field.value == "") return false;
        regexp = /^[0-9]+$/;
        return regexp.test(field.value);
    }
    // Otro key
    return false;
}

function ImporteTarifaHRAdicional(e, field) {
    key = e.keyCode ? e.keyCode : e.which;
    console.log(key);
    //Key backspace
    if (key == 8) return true;

    // 0-9 a partir del .decimal  
    if (field.value != "") {
        if ((field.value.indexOf(".")) > 0) {
            //si tiene un punto valida dos dígitos en la parte decimal
            if (key > 47 && key < 58) {
                if (field.value == "") return true;
                //regexp = /[0-9]{0,12}[\.][0-9]{1,2}$/;
                regexp = /[0-9]{2}$/;
                return !(regexp.test(field.value));
            }
        }
    }
    // Carácter 0-9 
    if (key > 47 && key < 58) {
        if (field.value == "") return true;
        regexp = /[0-9]{6}/;
        return !(regexp.test(field.value));
    }
    // Si el key es .
    if (key == 46) {
        if (field.value == "") return false;
        regexp = /^[0-9]+$/;
        return regexp.test(field.value);
    }
    // Otro key
    return false;
}

function ImporteArrendamiento(e, field) {
    key = e.keyCode ? e.keyCode : e.which;
    console.log(key);
    //Key backspace
    if (key == 8) return true;

    // 0-9 a partir del . decimal  
    if (field.value != "") {
        if ((field.value.indexOf(".")) > 0) {
            //si tiene un punto valida dos dígitos en la parte decimal
            if (key > 47 && key < 58) {
                if (field.value == "") return true;
                //regexp = /[0-9]{0,18}[\.][0-9]{1,2}$/;
                regexp = /[0-9]{2}$/;
                return !(regexp.test(field.value));
            }
        }
    }
    // Carácter 0-9 
    if (key > 47 && key < 58) {
        if (field.value == "") return true;
        regexp = /[0-9]{18}/;
        return !(regexp.test(field.value));
    }
    // Si el key es .
    if (key == 46) {
        if (field.value == "") return false;
        regexp = /^[0-9]+$/;
        return regexp.test(field.value);
    }
    // Otro key
    return false;
}
function descuentoMaximo(e, field) {
    key = e.keyCode ? e.keyCode : e.which;
    console.log(key);
    //Key backspace
    if (key == 8) return true;

    // 0-9 a partir del .decimal  
    if (field.value != "") {
        if ((field.value.indexOf(".")) > 0) {
            //si tiene un punto valida dos dígitos en la parte decimal
            if (key > 47 && key < 58) {
                if (field.value == "") return true;
                //regexp = /[0-9]{0,12}[\.][0-9]{1,2}$/;
                regexp = /[0-9]{2}$/;
                return !(regexp.test(field.value));
            }
        }
    }
    // Carácter 0-9 
    if (key > 47 && key < 58) {
        if (field.value == "") return true;
        regexp = /[0-9]{3}/;
        return !(regexp.test(field.value));
    }
    // Si el key es .
    if (key == 46) {
        if (field.value == "") return false;
        regexp = /^[0-9]+$/;
        return regexp.test(field.value);
    }
    // Otro key
    return false;
}