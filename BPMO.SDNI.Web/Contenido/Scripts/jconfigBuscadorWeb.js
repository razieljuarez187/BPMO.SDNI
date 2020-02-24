/* File Created: julio 17, 2012 */
//CONFIGURACIÓN DEL Buscador Web
/*--------------------------------------------*/
var srcFrame = "", hdnShowBuscador = "#hdnShowBuscador", width = "", height = "", btnTrigger = undefined;

$.BuscadorWeb = function (opts) {
    opts = $.extend({}, $.BuscadorWeb.defaults, opts || {});
    if (opts.url === undefined || opts.xml === undefined || opts.guid === undefined || opts.btnSender === undefined) {
        alert("Buscador Web: Los siguientes datos son requeridos: url, xml, guid, btnSender");
        return;
    }
    if (opts.preCarga != null)
        opts.preCarga();
    if (!$(hdnShowBuscador).length || !$(opts.btnSender).length) {
        alert("Buscador Web, debe definir las etiquetas siguientes: HiddenField(hdnShowBuscador), Button(btnResult)");
        return;
    }
    btnTrigger = opts.btnSender;
    $(hdnShowBuscador).val("1");
    if (opts.features.dialogWidth != undefined) { width = opts.features.dialogWidth; } else { width = '700px'; }
    if (opts.features.dialogHeight != undefined) { height = opts.features.dialogHeight; } else { height = '350px'; }
    srcFrame = opts.url + "?cfg=" + opts.xml + "&pktId=" + opts.guid;
}

//Ejecuta el evento Click sobre el BtnSender para reflejar el resultado del buscador
function btnResult_Click() {
    $(btnTrigger).click();
}

//Configuración Default del Buscador
$.BuscadorWeb.defaults = {
    url: '../Buscador.UI/BuscadorUI.aspx',
    xml: undefined,
    guid: undefined,
    btnSender: undefined,
    features: {
        border: 'thick',
        dialogWidth: '700px',
        dialogHeight: '350px',
        center: 'yes',
        help: 'no',
        maximize: '0',
        minimize: 'no'
    },
    preCarga: null
};

//Verifica si se debe desplegar el buscador
function initBuscador() {
    if ($(hdnShowBuscador).val() == "1") {
        $.blockUI({
            message: $('<div id="dvContentBuscador" style="width:80%;display:none;">' +
                        '<div id="dvTitleBuscador" class="ui-widget-header ui-dialog-titlebar ui-corner-all blockTitle" style="width:125%;height: 22px; background-color: #5c5e5d !important;border-top-left-radius: 5px; border-top-right-radius: 5px; color: white;cursor:move">Buscador' +
                            '<span id="closeDialog" onclick="cerrarBuscador();" class="ui-icon ui-icon-close" role="presentation" style="width:20px; height:20px;float:right; cursor:pointer">Cerrar</span>' +
                        '</div>' +
                        '<iframe id="ifBuscador" src="' + srcFrame + '" frameborder="0" style="border-radius:0px 0px 5px 5px;" scrolling="yes" height="' + height + '" width="' + width + '">Tu navegador no soporta frames!</iframe>' +
                    '</div>'),
            css: {
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                'border-radius': '10px',
                width: (parseInt(width.replace("px", "")) + 6) + 'px',
                height: (parseInt(height.replace("px", "")) + 28) + 'px',
                top: ($(window).height() - 400) / 2 + 'px',
                left: ($(window).width() - width.replace("px", "")) / 2 + 'px'
            }
        });
        $("#dvContentBuscador").parent().draggable();
    }
}

//Cerrar el Buscador
function cerrarBuscador() {
    $.unblockUI();
    $(hdnShowBuscador).val("0");
}

// Obtener el ancho del Buscador según el catálogo
//   Se usa la suma del ancho de las columnas (xml) y se agrega 140px para el padding a los lados.
function ObtenerAnchoBuscador(xml) {
    //Ancho predeterminado
    var result = "680px";

    var lstCatalogos = xml.split("&");
    var catalogo = lstCatalogos[0];

    if (catalogo === null || catalogo.match(/^ *$/) !== null)
        return result;

    if (catalogo == "Caracteristica")
        result = '670px';
    else if (catalogo == "ClasificadorAplicacion")
        result = '540px';
    else if (catalogo == "ClasificadorMotorizacion")
        result = '540px';
    else if (catalogo == "Cliente")
        result = '730px';
    else if (catalogo == "ConceptoMovimiento")
        result = '490px';
    else if (catalogo == "ConfiguracionModeloMotorizacion")
        result = '540px';
    else if (catalogo == "Cuenta")
        result = '740px';
    else if (catalogo == "CuentaClienteIdealease")
        result = '890px';
    else if (catalogo == "CuentaClienteIdealeaseSimple")
        result = '890px';
    else if (catalogo == "DireccionCuentaClienteIdealease")
        result = '820px';
    else if (catalogo == "DireccionCuentaClienteIdealeaseSimple")
        result = '820px';
    else if (catalogo == "Distribuidor")
        result = '540px';
    else if (catalogo == "Empleado")
        result = '650px';
    else if (catalogo == "Empresa")
        result = '690px';
    else if (catalogo == "EquipoAliado")
        result = '920px';
    else if (catalogo == "EquipoBepensa")
        result = '890px';
    else if (catalogo == "HistorialUnidad")
        result = '1080px';
    else if (catalogo == "Llanta")
        result = '710px';
    else if (catalogo == "Marca")
        result = '690px';
    else if (catalogo == "Modelo")
        result = '540px';
    else if (catalogo == "MotivoBitacora")
        result = '530px';
    else if (catalogo == "Operadores")
        result = '600px';
    else if (catalogo == "OrdenServicio")
        result = '690px';
    else if (catalogo == "ProductoServicio")
        result = '735px';
    else if (catalogo == "Proveedor")
        result = '560px';
    else if (catalogo == "SubCuentaCliente")
        result = '690px';
    else if (catalogo == "Sucursal")
        result = '590px';
    else if (catalogo == "SucursalSeguridad")
        result = '590px';
    else if (catalogo == "SucursalSeguridadSimple")
        result = '590px';
    else if (catalogo == "TallerSeguridad")
        result = '790px';
    else if (catalogo == "Tarifas")
        result = '570px';
    else if (catalogo == "Tecnico")
        result = '490px';
    else if (catalogo == "TipoCliente")
        result = '485px';
    else if (catalogo == "TipoDocumento")
        result = '770px';
    else if (catalogo == "TipoServicio")
        result = '540px';
    else if (catalogo == "TipoUnidad")
        result = '560px';
    else if (catalogo == "Unidad")
        result = '880px';
    else if (catalogo == "UnidadDisponibleReservacion")
        result = '970px';
    else if (catalogo == "UnidadIdealease")
        result = '790px';
    else if (catalogo == "UnidadIdealeaseSimple")
        result = '790px';
    else if (catalogo == "UnidadOperativa")
        result = '690px';
    else if (catalogo == "UsoCFDI")
        result = '815px';

    return result;
}
/*--------------------------------------------*/