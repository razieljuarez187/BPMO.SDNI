Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

$(document).ready(function () {
    ConfiguracionInicial();
     $('#MenuSecundario').hide();
     $('#MenuSecundario').show(500);
});
function ConfiguracionInicial(){
    $("span:contains('*')").addClass('ColorValidator');
    EstilosMenu();
    EstilosMenuSecundario();
    AsignarImagen();
    SeleccionarOpcionMenuSecundario();
    $(':text, :input, textarea, select').addClass('TipoLetra').css({ 'border-radius': '5px', '-moz-border-radius': '5px', '-webkit-border-radius': '5px', '-khtml-border-radius': '5px' });
    $('.GroupBody :text, .GroupBody :input, .GroupBody textarea, .GroupBody select').css({ 'text-transform': 'uppercase' });
    $('.GroupSection :text, .GroupSection :input, .GroupSection textarea, .GroupSection select').css({ 'text-transform': 'uppercase' });
    $(':text, select').css({'height': '25px'});
    $(':text, textarea').css({'padding-right':'5px', 'padding-left':'5px'});
    $(':text, textarea, select').css({'font-family' : 'Century Gothic, Arial, Verdana, Serif', 'border-color':'#d7d7d7', 'border-style':'solid', 'border-width':'1px'});
    $('textarea').css({'margin-left': '0px'});
    $(':text[disabled], textarea[disabled], select[disabled]').css({'background-color':'#e6e6e6', 'border-color':'#d2d2d2', 'border-style':'solid', 'border-width':'1px'});
    $('div.ContenedorMensajes').css({'width':'100%','margin-bottom':'5px','text-align':'right','text-transform':'uppercase'});
    $("span.Requeridos").addClass("ColorValidator").text("(*)Campos requeridos").css({'font-size':'12px','margin-right':'5px'});
    $("span.FormatoIncorrecto").addClass("ColorValidator").text("(**)Formato incorrecto").css({ 'font-size': '12px', 'margin-right': '5px' });
    //SC_0008
    $(function () {
        $('input').keypress(function (e) {
            var code = null;
            code = (e.keyCode ? e.keyCode : e.which);
            return (code == 13) ? false : true;
        });
    });
    //end SC_0008
}
// Variable de Inicializacion de las Child Page
var initChild;

// Bloqueo de pantalla
function __blockUI() {
    $.blockUI({
        fadeIn: 1000,
        message: '<img src="../Contenido/Imagenes/Cargando.gif" />  Espere por favor...',
        css: {
            border: 'none',
            padding: '15px',
            backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            '-khtml-border-radius': '10px',
            'border-radius': '10px',
            opacity: .5,
            color: '#fff'
        }
    });     
}

/*Inicio de Request*/
function BeginRequestHandler(sender, args) {
    var dialog = $('#hdnDialog');
    if(dialog != undefined){
        $('#hdnDialog').val("");
    }

    ConfiguracionInicial();
    __blockUI();
}
/*Fin de Request*/
function EndRequestHandler(sender, args) {
    $.unblockUI();
     var dialog = $('#hdnDialog');
    if(dialog != undefined){
        if ($('#hdnDialog').val() == ""){
            MensajeCambioAdscripcionBlock("Se requiere cerrar la página actual debido a que ha cambiado su selección de Unidad Operativa-Sucursal-Taller para esta sesión.");
        }
    }
    initClient();
    if(typeof InitControls === "function")
        InitControls();
    ConfiguracionInicial();

    if(initChild != undefined && initChild != null){
        initChild();
    }
    initDialog();
}

/*Alertas GrowUI: 1= Exito, 2=Advertencia, 4= Información*/
function MensajeGrowUI(message, tipo){
    var $m;
    var title = "";
    if(tipo == "1")
    {
        $m = $('<div class="growlUIExito growlUI"></div>');
        title = "Éxito";
    }else if (tipo == "2")
    {
        $m = $('<div class="growlUIAlerta growlUI"></div>');
        title = "Advertencia";
    }else if(tipo == "4")
    {
        $m = $('<div class="growlUIInfo growlUI"></div>');
        title = "Información";
    }
    if (title) $m.append('<h1>'+title+'</h1>');
    if (message) $m.append('<h2>'+message+'</h2>');
    $.blockUI({
        message: $m, fadeIn: 700, fadeOut: 1000, centerY: false,
        timeout: 5000, showOverlay: false,
        css: {
            width:  	'350px',
            top:		'10px',
            left:   	'',
            right:  	'10px',
            border: 	'none',
            padding:	'5px',
            opacity:	0.6,
            cursor: 	'default',
            color:		'#fff',
            backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius':	 '10px',
            'border-radius': 		 '10px',
            '-khtml-border-radius':  '10px'
        }
    });
    LimpiarHdn();
}

//btnSender:  referencia al botón que dispara el postback al Server
//mensaje: Mensaje a desplegar en la Confirmación
function MensajeConfirmacion(btnSender, mensaje) {
    var $div = $('<div title="Confirmación"></div>');
    $div.append(mensaje);
    $("#dialog:ui-dialog").dialog("destroy");
    $($div).dialog({
        closeOnEscape: true,
        modal: true,
        minWidth: 460,
        minHeight: 250,
        buttons: {
            Aceptar: function () {
                $(this).dialog("close");
                $(btnSender).click();
            }, 
            Cancelar: function(){
                $(this).dialog("close");
            }
        }
    });
}

//Mostrar Mensaje de Error, con opción Ver Detalle
function MensajeError(mensaje, detalle) {
    $("#dialog-error").remove();
    var $div = $('<div id="dialog-error" title="Error" style="display:none;"></div>');
    $div.append('<div id="dMensajeError">' + mensaje + '</div>');
    var $acordion = $('<div id="accordion"></div>');
    $acordion.append('<h3 class="detailsHeader">Ver Detalle</h3>');
    $acordion.append('<div id="dDetalle">'+detalle + '</div>');
    $div.append($acordion);

    $("#dialog:ui-dialog").dialog("destroy");
    $($div).dialog({
        closeOnEscape: true,
        modal: true,
        minWidth: 460,
        minHeight: 250,
        buttons: {
            Aceptar: function () {
                $(this).dialog("close");
            }
        }
    });

    $("#accordion").accordion({
        collapsible: true,
        event: "mouseover",
        icons: false
    });

    $("#accordion").find(".detailsHeader").click(function () {
        $(this).toggleClass("source-closed").toggleClass("source-open").next().toggle("blind");
        return false;
    }).end().find(">div").hide();

    LimpiarHdn();
}

/*Mensaje en Block: <div id="dialog-info" title="Éxito"></div>*/
function MensajeCambioAdscripcionBlock(mensaje) {
    $("#dialog-cambio").remove();
    var $div = $('<div id="dialog-cambio" title="Información" style="display:none;">' + mensaje + '</div>');
    $($div).dialog({resizable: false, closeOnEscape: false,modal: true,minWidth: 350, minHeight: 200,
        buttons: {
            Aceptar: function () {
                $(this).dialog("close");
                window.valorRetornado = "CAMBIOAS";
                window.close();
            }
        },
        beforeClose: function (event, ui) {
            return false;
        }
    });
}
function EnvioInicio(valueDialog){
    if(valueDialog == "CAMBIOAS")
        $(location).attr('href',location.protocol+"//"+location.host + "/"+"MapaSitio.UI/MenuPrincipalUI.aspx?pkt=1");
}

//Mensaje Despliegue: <div id="dmsjDespliegue" style="position: absolute; top:0px; display: none;"></div>
function MensajeTop(mensaje, tipo) {
    if (tipo == "1")
        $("#dmsjDespliegue")._addClass("exito");
    else if (tipo == "2")
        $("#dmsjDespliegue")._addClass("alerta");
    else if (tipo == "4")
        $("#dmsjDespliegue")._addClass("info");

    $("#dmsjDespliegue").text(mensaje);
    $("#dmsjDespliegue").fadeIn(800);
    setTimeout(function () { $("#dmsjDespliegue").fadeOut(800); }, 5000); //.fadeIn(800).fadeOut(500).fadeIn(500).fadeOut(300)
}

/*---------------------------------------------------------*/
/*---------------Manejo del Tiempo de Sesión---------------*/
var contadorName;
var contadorVal;
var hdnInicioContador;
function Contador(hdnContador, hdnIniContFinSession) {
     if(hdnContador != undefined && hdnIniContFinSession != undefined){
        contadorName = hdnContador;
        hdnInicioContador = parseInt($(hdnIniContFinSession).val());
    }
    contadorVal = parseInt($(contadorName).val());
    if (contadorVal == hdnInicioContador)
        InfoFinSession(1);
    if (contadorVal <= hdnInicioContador)
        $("#timeSession").val(contadorVal);
    if (contadorVal == 0) {
        FinSession();
        contadorVal = undefined;
        hdnInicioContador = undefined;
    } else {
        setTimeout("Contador()", 1000);
        contadorVal = contadorVal - 1;
        $(contadorName).val(contadorVal);
    }
}
function BtnFinSession() {
    __doPostBack('', '');
}
var typeClose;
function InfoFinSession() {
    $("#dvFinSession").remove();
    var $div = $('<div id="dvFinSession" title="Información" style="display:none;">La sesión de esta página ha permanecido inactiva.<br/>' +
        'Su sesión expirará en: <input id="timeSession" readonly="readonly" type="text" value="" style="background-color:transparent;border-style:none;width:30px;"/>segundos.</div>');
    $("#dialog:ui-dialog").dialog("destroy");
    $($div).dialog({resizable: false,closeOnEscape: false,modal: true,minWidth: 350,minHeight: 150,hide: "explode",
        buttons: {
            Cancelar: function () {
                typeClose = 1;
                $(this).dialog("close");
            }
        },
        close: function (event, ui) {
            __doPostBack('', '');
        }
    });
}
function FinSession() {
    typeClose = undefined;
    $("#dvFinSession").remove();
    var $div = $('<div id="dvFinSession" title="Información" style="display:none;line-height:20px;">El tiempo de inactividad de esta página ha alcanzado su límite.<br/>' +
        'a) Si ha tenido actividad en otras páginas del módulo su sesión reanudará..<br/>'+
        'b) Si no ha mantenido actividad dentro del módulo será redirigido al logueo para iniciar sesión nuevamente.</div>');
    $("#dialog:ui-dialog").dialog("destroy");
    $($div).dialog({resizable: false,closeOnEscape: false,modal: true,minWidth: 450,minHeight: 250,hide: "explode",
        buttons: {
            Aceptar: function () {
                typeClose = 2;
                $(this).dialog("close");
                BeginRequestHandler();
                BtnFinSession();
            }
        },
        beforeClose: function (event, ui) {
            if(typeClose == undefined)
                return false;
        }
    });
}
var post = '__doPostBack';
/*---------------------------------------------------------*/

//Restablece las variables utilizadas en el Dialog
var width = "800px", height = "600px", pagina = "", title = "", invocarMetodo = "";
var ejecutarAccion = false, valorRetornado = undefined;
function resetVarDialog() {
    this.pagina = "";
    this.title = "Catálogo";
    this.width = "800px";
    this.height = "600px"
    this.invocarMetodo = "";
    this.ejecutarAccion = false;
    this.valorRetornado = undefined;
}
//Invocar showModalDialog
/*Recibe 5 parámetros: 
* pagina: Página que se desea abrir como dialogo
* title: Título del dialog
* width: Ancho del dialog
* height: Alto del dialog
* invocarMetodo(opcional): si después de cerrar el dialogo se requiere ejecutar un método
*/
function showDialogModal(pagina, title, width, height, invocarMetodo) {
    if (pagina == undefined) {
        MensajeGrowUI("Debe proveer la url de la página a desplegar como dialogo", "2");
        return false;
    }
    resetVarDialog();
    this.pagina = pagina;
    if (title != undefined && title != "") {
        this.title = title;
    }
    if (width != undefined || height != undefined) {
        this.width = width;
        this.height = height;
    }
    if (invocarMetodo != undefined && invocarMetodo != "") {
        this.invocarMetodo = invocarMetodo;
    }
    $("input[name$='hdnShowDialogModal']").val("1");
}
//Abrir Dialog
function initDialog() {
    $dvShowDialog = $("input[name$='hdnShowDialogModal']");
    if ($dvShowDialog.length == 1 && ($dvShowDialog.val() == "1")) {

        ConfiguracionInicial();
        $.blockUI({
            message: $('<div id="dvContentDialog" style="width:80%;display:none;">' +
                '<div id="dvTitleDialog" class="ui-widget-header ui-dialog-titlebar ui-corner-all blockTitle" style="width:125%;height: 22px;background-color: #5c5e5d !important;border-top-left-radius: 5px; border-top-right-radius: 5px; color: white;cursor:move">' + this.title +
                    '<span id="closeDialog" onclick="cerrarDialog();" class="ui-icon ui-icon-close" role="presentation" style="width:20px; height:20px;float:right; cursor:pointer">Cerrar</span>' +
                '</div>' +
                '<iframe id="ifPagina" src="' + pagina + '" frameborder="0" style="border-radius:0px 0px 5px 5px;" scrolling="yes" height="' + height + '" width="' + width + '">Tu navegador no soporta frames!</iframe>' +
           '</div>'),
            css: {
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                'border-radius': '10px',
                width: width,
                height: (parseInt(height.replace("px", "")) + 22) + 'px',
                top: ($(window).height() - height.replace("px", "")) / 2 + 'px',
                left: ($(window).width() - width.replace("px", "")) / 2 + 'px'
            }
        });
        $("#dvContentDialog").parent().draggable();
    }
}
//Cerrar el dialog
function cerrarDialog() {
    $("input[name$='hdnShowDialogModal']").val("0");
    EndRequestHandler();
    if (this.invocarMetodo != undefined && this.invocarMetodo != "" && ejecutarAccion) {
        setTimeout(this.invocarMetodo, 10);
    }
}
//Agrega estilos a los elementos del menú
function EstilosMenu() {
        $('#dvNavegacion .MenuEstiloElementoEstatico').hover(function () {
        var colorFondo = $('#DatosSesion').css('backgroundColor');
            if(!$(this).hasClass('MenuPrincipalSeleccionado'))
            {
                $(this).css({'backgroundColor': colorFondo, 'color':'white'});
            }
        }, function () {
         if(!$(this).hasClass('MenuPrincipalSeleccionado'))
            {
            $(this).css({'backgroundColor':'white','color':'#56421f'});
            }
        });
        $('#dvNavegacion .MenuElementoDinamico').hover(function () {
            $(this).css({ 'background-color': '#999999', 'color': '#000000 !important', 'font-weight': 'bold' });          
        }, function () {
            $(this).css({ 'background-color': 'white', 'color': '#56421f', 'font-weight': 'normal' });           
        });
}        
//Agrega estilos a los elementos del menú secundario
function EstilosMenuSecundario() {
    $('#MenuSecundario li').each(function () {
        if (!$(this).hasClass('MenuSecundarioSeleccionado') && $(this).find('a.aspNetDisabled').length > 0) {
            $(this).addClass('MenuSecundarioDeshabilitado');
        }
        else {
            $(this).removeClass('MenuSecundarioDeshabilitado');
        }
    });

    $('#MenuSecundario li').hover(function () {
        if (!$(this).hasClass('MenuSecundarioDeshabilitado')){            
            var color = $('.MenuSecundarioSeleccionado').css('backgroundColor');
            $(this).css({ 'backgroundColor': color });
            $(this).find('a').css('color', 'white');
        }
    }, function () {
        if (!$(this).hasClass('MenuSecundarioSeleccionado') && !$(this).hasClass('MenuSecundarioDeshabilitado')) {
            $(this).css('backgroundColor', '#eaeaea');
            $(this).find('a').css('color', '#705e5d');
        }
    });
}
//Redirecciona a la página contenida en el tag anchor del li
function SeleccionarOpcionMenuSecundario() {
    $('#MenuSecundario li').click(function () {
        if (!$(this).hasClass('MenuSecundarioDeshabilitado')) {
            window.location = $(this).find('a:enabled').attr('href');
        }
    });
}
//Asigna la imagen correspondiente al tipo de notificaciones
function AsignarImagen() {
    $('div.A > img').attr('src', '../Contenido/Imagenes/icono-informacion.jpg');
    $('div.N > img').attr('src', '../Contenido/Imagenes/iconomensaje.jpg');
}

/*---------------------------------------------------------*/
/*---------------IDs no usados en el sistema---------------*/
//Agrega estilos a los elementos del menú secundario
function EstilosMenuOpciones() {
     $('#MenuOpcionesOrden li:not(.SubMenuSeleccionado)').hover(
     function () {
        $(this).addClass('MenuOpcionesOrdenlihover');                    
     }, function () {
        $(this).removeClass('MenuOpcionesOrdenlihover');
     });
} 
//Agrega estilos a los elementos del menú de los catalogos
function EstilosMenuOpcionesCatalogos() {
     $('#MenuOpcionesCatalogos li:not(.SubMenuSeleccionado, .SubMenuCatalogosDeshabilitado)').hover(
     function () {
        $(this).addClass('MenuOpcionesCatalogoslihover');                    
     }, function () {
        $(this).removeClass('MenuOpcionesCatalogoslihover');
     });
}
//Agrega estilo a los submenus del menu opciones
 function EstilosSubMenuOpciones() {
    $('#SubMenuOrdenServicio li').hover(
        function () {
            $(this).css({ 'backgroundColor': '#E9581B', 'cursor': 'pointer' });
            $(this).children().css('color', 'white');
        }, function () {
            $(this).css({ 'backgroundColor': 'white' });
            $(this).children().css('color', 'black');
        });
}
/*---------------------------------------------------------*/
            
//Funcionamiento hover para el boton regresar
function EfectoHoverDeBoton(boton)
{
     boton.hover(
     function () {
     $(this).css({ 'backgroundColor': '#E9581B', 'color': 'white', 'cursor': 'pointer' });
     },
     function () {
     $(this).css({ 'backgroundColor': 'transparent', 'color': '#56421f' });
     });
}
//Efectos hover para menús secundarios varios
function EfectoHoverSubMenu(boton)
{
     boton.hover(
     function () {
     $(this).css({ 'backgroundColor': '#E9581B', 'cursor': 'pointer' }).children().css('color','white');    
     },
     function () {
     $(this).css({ 'backgroundColor': '#e3e3e3'}).children().css( 'color','black');
     });
}


/*-------------------------------------------------------------------------------*/
/*---------------Manejo de los Estilos de la Barra de Herramientas---------------*/
function ConfiguracionBarraHerramientas() {    
    $('.MenuPrimario .SubMenuImpresion li').each(function () {
        if (!$(this).hasClass('Informacion') && $(this).find('a.aspNetDisabled').length > 0) {
            $(this).addClass('itemDeshabilitado');
        }
        else {
            $(this).removeClass('itemDeshabilitado');
        }
    });
    var itemSeleccionado = $('div.MenuPrimario a.selected');
    if (itemSeleccionado.length > 0)
        itemSeleccionado.parent().addClass('Seleccionado');

    $("div.MenuPrimario > ul.dynamic > li.dynamic, div.MenuPrimario > ul.static > li.static, div.MenuPrimario ul.level2 > li").click(function () {
        var texto = $(this).children("a.highlighted");

        if (!texto.hasClass("aspNetDisabled") && texto.prop('href') != '') {
            texto[0].click();
            $('.MenuPrimario .level2').hide();
        }
    });

    $('.Informacion').parent().parent().addClass('Informacion');
}
/*-------------------------------------------------------------------------------*/

var initDate;
var endDate;

function ConfigurarRangoFechas(fechaInicio, fechaFin) {
    initDate = fechaInicio;
    endDate = fechaFin;

    initDate.datepicker({
        dateFormat: "dd/mm/yy",
        defaultDate: "+1w",
        changeYear: true,
        changeMonth: true,
        numberOfMonths: 1,
        onClose: function(selectedDate) {
            endDate.datepicker("option", "minDate", selectedDate);
        },
        showButtonPanel: true,
        buttonImage: '../Contenido/Imagenes/calendar.gif',
        buttonImageOnly: true,
        toolTipText: "Fecha del Inicio",
        showOn: 'button'
    }).attr('readonly', true);

    endDate.datepicker({
        dateFormat: "dd/mm/yy",
        defaultDate: "+1w",
        changeYear: true,
        changeMonth: true,
        numberOfMonths: 1,
        onClose: function (selectedDate) {
            initDate.datepicker("option", "maxDate", selectedDate);
        },
        showButtonPanel: true,
        buttonImage: '../Contenido/Imagenes/calendar.gif',
        buttonImageOnly: true,
        toolTipText: "Fecha de Fin",
        showOn: 'button'
    }).attr('readonly', true);
}