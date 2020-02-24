function loadMenuPrincipalSelected() {
    var $result = $('#mnuPrincipal').find('.MenuPrincipalSeleccionado');
    $.each($result, function (index, item) {
        var $aSelected = $(item);
        var $listHref = $("#navbar-main li a:contains('" + $aSelected.text() + "')");
        $.each($listHref, function (index, href) {
            var $a = $(href);
            if ($a.hasClass("dropdown-toggle")) {
                var $parent = $($a.parent());
                $parent.addClass("active");
                return;
            }
        });
    });
}

function listenClickMenuResponsive() {
    $("#navbar-main li").click(function (event) {
        var $href = $(event.target);
        var $element = $($href.parent());
        var $parent = $($element.parent());
        if ($parent.hasClass('dropdown-menu') || $element.children().length === 1) {
            if ($href.text().toUpperCase() !== "INICIO") {
                var $resultContain = $("#mnuPrincipal li a:contains('" + $href.text() + "')");
                $.each($resultContain, function (index, item) {
                    var $newItem = $(item);
                    var $newParent = $newItem;
                    if ($newItem.hasClass("dynamic")) {
                        while (!$newParent.hasClass("static")) {
                            $newParent = $($newParent.parent());
                        }
                    }
                    var positionHref = 0;
                    var newHref = $newParent.children()[positionHref];
                    if (this != $href) {
                        if ($href.is('a')) {
                            window.location = $href.attr('href') + '?MenuSeleccionado=' + $(newHref).text();
                        }
                    }
                    return false;
                });
            } else {
                window.location = $href.attr('href') + '?MenuSeleccionado=' + $href.text();
            }
            SeleccionarMenu();
        }
    });
}

function moveButtonsAddon() {
    var $parents = $(".CampoFecha");
    $.each($parents, function (index, $item) {
        var $child = $($item);
        var $datePicker = $child.find('.ui-datepicker-trigger');
        if ($datePicker !== null) {
            var $span = $('<span>', { class: "input-group-addon" });
            $span.append($datePicker);
            $child.append($span);
        }
    });
}

function getLogoutOption() {
    var $liSession = $('<li>');
    var user = $("#lblNombre").text().toUpperCase();
    var $oldHrefLogout = $("#dvDatosSesion").find("a");
    var valueHref = $($oldHrefLogout).prop("href");
    var $newHrefLogout = $('<a href="' + valueHref + '">CERRAR SESIÓN (' + user + ')</a>');
    $liSession.append($newHrefLogout);

    return $liSession;
}

function getNavBarHeader() {
    var $spanAdscripcion = $("#lblAdscripcion");
    var value = $spanAdscripcion.text().toUpperCase();
    return value;
}

function cloneMenu() {
    $("#navbar-title").append(getNavBarHeader());
    var $children = $("#mnuPrincipal").children();
    var $menuResponsive = buildMenuResponsive($($children[0]));
    $("#navbar-main").append($menuResponsive);
}

function buildMenuResponsive($item) {
    var $ul = $('<ul>', { class: "nav navbar-nav navbar-right" });
    $ul.append(getLogoutOption());
    var $newChild, $oldHref;
    var valueHref = "", textValueHref = "";
    $.each($item.children(), function (index, $child) {
        $oldHref = $($child).children()[0];
        valueHref = $($oldHref).attr('href');
        textValueHref = $oldHref.textContent.toUpperCase();
        if ($child.tagName === "LI") {
            if ($($child).children().length > 1) {
                $newChild = $('<li>', { class: "dropdown" });
                var $href = $('<a href="' + valueHref + '" ' +
                                        'class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" ' +
                                        'aria-expanded="false">' + textValueHref + '</a>');
                $newChild.append($href);
                var $submenus = $($child).children()[1];
                $newChild.append(getSubmenus($submenus));
            } else {
                $newChild = $('<li>');
                $newChild.append($('<a href="' + valueHref + '" >' + textValueHref + '</a>'));
            }
            $ul.append($newChild);
        }
    });

    return $ul;
}

function getSubmenus($submenus) {
    var $ul = $('<ul>', { class: "dropdown-menu" });
    $.each($($submenus).children(), function (index, $item) {
        var $li = $('<li>');
        var $oldHref = $($item).children()[0];
        var valueHref = $($oldHref).prop("href");
        var textValueHref = $oldHref.textContent.toUpperCase();
        $li.append('<a href="' + valueHref + '">' + textValueHref + '</a>');
        $ul.append($li);
    });
    return $ul;
}